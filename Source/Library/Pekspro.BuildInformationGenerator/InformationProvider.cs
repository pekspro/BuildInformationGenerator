using System.Diagnostics;

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
        "BUILD_SOURCEBRANCHNAME", 

        // GitLab
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
                return new ValueSource<string>(branchName, $"Value was taken from then {branchEnvVariableName} environment variable.");
            }
        }
        
        //string result = ExecuteProcess("git", "rev-parse --abbrev-ref HEAD", buildInfoToGenerate.FilePath);
        string result = ExecuteProcess("git", "branch --show-current", buildInfoToGenerate.FilePath);

        if (string.IsNullOrWhiteSpace(result))
        {
            string status = ExecuteProcess("git", "status", "./");
            Console.WriteLine($"Status: *{status}*");

            if (status.Contains("detached"))
            {
                throw new Exception("Git is in detached mode. Branch name is not available. If you are running this in CI/CD pipeline, try set the branch name to the BUILD_SOURCEBRANCHNAME environment variable.");
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
