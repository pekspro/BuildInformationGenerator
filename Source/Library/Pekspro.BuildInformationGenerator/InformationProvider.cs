﻿using System.Diagnostics;

namespace Pekspro.BuildInformationGenerator;

public class InformationProvider
{
    public static InformationProvider Instance { get; } = new InformationProvider();

    public virtual ValueSource<DateTimeOffset> GetBuildTime(in BuildInformationToGenerate buildInfoToGenerate, in CompilationInformation compilationInformation)
    {
        if (buildInfoToGenerate.Fake)
        {
            return new ValueSource<DateTimeOffset>(FakeValues.FakeBuildTime, FakeValues.FakeValueSource);
        }

        return new ValueSource<DateTimeOffset>(compilationInformation.BuildStartTime, "Value was taken from the system clock.");
    }

    public virtual ValueSource<string> GetGitCommitId(in BuildInformationToGenerate buildInfoToGenerate, in CompilationInformation compilationInformation)
    {
        if (buildInfoToGenerate.Fake)
        {
            return new ValueSource<string>(FakeValues.FakeGitCommitId, FakeValues.FakeValueSource);
        }

        bool IsValidCommitId(string commitId)
        {
            if (commitId.Length != 40)
            {
                return false;
            }

            foreach (char chr in commitId)
            {
                if (!((chr >= '0' && chr <= '9') || (chr >= 'a' && chr <= 'f') || (chr >= 'A' && chr <= 'F')))
                {
                    return false;
                }
            }

            return true;
        }

        // Try extract from AssemblyInformationalVersion first. Supported in .NET 8 and later.
        // https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assemblyinformationalversionattribute
        if (compilationInformation.AssemblyInformationalVersion != null)
        {
            int lastPlus = compilationInformation.AssemblyInformationalVersion.LastIndexOf('+');

            if (lastPlus != -1)
            {
                string commitId = compilationInformation.AssemblyInformationalVersion.Substring(lastPlus + 1);

                if (IsValidCommitId(commitId))
                {
                    return new ValueSource<string>(commitId, "Value was taken from the AssemblyInformationalVersion attribute.");
                }
            }
        }

        // This also works, but a little but slower: string result = ExecuteProcess("git", "log -1 --format=%H", buildInfoToGenerate.FilePath);
        string result = ExecuteProcess("git", "rev-parse HEAD", buildInfoToGenerate.FilePath);

        // Make sure just 0-9 and a-f, A-F
        if (!IsValidCommitId(result))
        {
            return new ValueSource<string>("", "git command was executed, but no value returned.");
        }

        return new ValueSource<string>(result, "Value was taken from the git command.");
    }

    static readonly string[] BranchEnvironmentVariableNames = new string[]
    {
        // Azure DevOps
        // https://learn.microsoft.com/en-us/azure/devops/pipelines/build/variables
        "BUILD_SOURCEBRANCHNAME", 

        // GitLab
        // https://docs.gitlab.com/ee/ci/variables/predefined_variables
        "CI_COMMIT_BRANCH"
    };

    public virtual ValueSource<string> GetGitBranch(in BuildInformationToGenerate buildInfoToGenerate)
    {
        if (buildInfoToGenerate.Fake)
        {
            return new ValueSource<string>(FakeValues.FakeGitBranch, FakeValues.FakeValueSource);
        }

        foreach (var branchEnvVariableName in BranchEnvironmentVariableNames)
        {
#pragma warning disable RS1035 
            string branchName = Environment.GetEnvironmentVariable(branchEnvVariableName);
#pragma warning restore RS1035

            if (!string.IsNullOrWhiteSpace(branchName))
            {
                return new ValueSource<string>(branchName, $"Value was taken from the {branchEnvVariableName} environment variable.");
            }
        }
        
        string result = ExecuteProcess("git", "branch --show-current", buildInfoToGenerate.FilePath);

        if (string.IsNullOrWhiteSpace(result))
        {
            string status = ExecuteProcess("git", "status", buildInfoToGenerate.FilePath);

            if (status.Contains("detached"))
            {
                result = ExecuteProcess("git", "name-rev --name-only --refs=refs/heads/* --no-undefined --always HEAD", buildInfoToGenerate.FilePath);

                return new ValueSource<string>(result, $"Git is running in detached state. Value was taken with the git name-rev command.");
            }

            throw new Exception("Git branch not found. Is Git initialized?");
        }

        return new ValueSource<string>(result, "Value was taken from the git branch command.");
    }

    public virtual ValueSource<string> GetAssemblyVersion(in BuildInformationToGenerate buildInfoToGenerate, in CompilationInformation compilationInformation)
    {
        if (buildInfoToGenerate.Fake)
        {
            return new ValueSource<string>(FakeValues.FakeAssemblyVersion, FakeValues.FakeValueSource);
        }

        return new ValueSource<string>(compilationInformation.AssemblyVersion, "Value was taken from assembly version attribute.");
    }

    public virtual ValueSource<string> GetOSVersion(in BuildInformationToGenerate buildInfoToGenerate)
    {
        if (buildInfoToGenerate.Fake)
        {
            return new ValueSource<string>(FakeValues.FakeOSVersion, FakeValues.FakeValueSource);
        }

#pragma warning disable RS1035
        return new ValueSource<string>(Environment.OSVersion.ToString(), "Value was taken from Environment.OSVersion.");
#pragma warning restore RS1035
    }

    public virtual ValueSource<string> GetDotNetVersion(in BuildInformationToGenerate buildInfoToGenerate)
    {
        if (buildInfoToGenerate.Fake)
        {
            return new ValueSource<string>(FakeValues.FakeDotNetVersion, FakeValues.FakeValueSource);
        }

        string result = ExecuteProcess("dotnet", "--version", buildInfoToGenerate.FilePath);

        return new ValueSource<string>(result, "Value was taken from the dotnet --version command.");
    }

    public virtual string GetWorkloadList(in BuildInformationToGenerate buildInfoToGenerate)
    {
        if (buildInfoToGenerate.Fake)
        {
            return string.Empty;
        }

        string workloadInfo = ExecuteProcess("dotnet", "workload list", buildInfoToGenerate.FilePath);

        return workloadInfo;
    }

    public ValueSource<string> GetMauiVersion(in BuildInformationToGenerate buildInfoToGenerate, string workloadInfo)
    {
        if (buildInfoToGenerate.Fake)
        {
            return new ValueSource<string>(FakeValues.FakeWorkloadMaui, FakeValues.FakeValueSource);
        }

        return new ValueSource<string>(GetVersionString("\nmaui", workloadInfo), "Value was taken from the dotnet workload list command.");
    }

    public ValueSource<string> GetWasmToolsVersion(in BuildInformationToGenerate buildInfoToGenerate, string workloadInfo)
    {
        if (buildInfoToGenerate.Fake)
        {
            return new ValueSource<string>(FakeValues.FakeWorkloadWasmTools, FakeValues.FakeValueSource);
        }

        return new ValueSource<string>(GetVersionString("\nwasm-tools", workloadInfo), "Value was taken from the dotnet workload list command.");
    }

    static string GetVersionString(string workloadId, string workloadInfo)
    {
        int startPos = workloadInfo.IndexOf(workloadId);

        if (startPos == -1)
        {
            return "";
        }

        startPos += workloadId.Length + 1;

        // Skip to spaces
        while (workloadInfo[startPos] != ' ' && startPos < workloadInfo.Length)
        {
            startPos++;
        }

        // Then skip spaces
        while (workloadInfo[startPos] == ' ' && startPos < workloadInfo.Length)
        {
            startPos++;
        }

        int endPos = startPos;

        // Find the end
        while (workloadInfo[endPos] != ' ' && endPos < workloadInfo.Length)
        {
            endPos++;
        }

        if (startPos == endPos)
        {
            return "";
        }

        return workloadInfo.Substring(startPos, endPos - startPos);
    }

    private static string ExecuteProcess(string fileName, string arguments, string workingDirectory)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();
        string result = process.StandardOutput.ReadToEnd();

        return result.Trim();
    }
}
