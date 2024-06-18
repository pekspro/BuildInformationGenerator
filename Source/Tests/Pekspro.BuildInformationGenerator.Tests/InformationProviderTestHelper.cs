using System;

namespace Pekspro.BuildInformationGenerator.Tests;

public class InformationProviderTestHelper : InformationProvider
{
    public override ValueSource<DateTimeOffset> GetBuildTime(in BuildInformationToGenerate buildInfoToGenerate, in CompilationInformation compilationInformation)
    {
        if (buildInfoToGenerate.Fake)
        {
            return base.GetBuildTime(buildInfoToGenerate, compilationInformation);
        }

        return new ValueSource<DateTimeOffset>(new DateTimeOffset(2022, 10, 19, 20, 10, 6, TimeSpan.FromHours(-2)), "Value was taken from the system clock.");
    }

    public override ValueSource<string> GetGitCommitId(in BuildInformationToGenerate buildInfoToGenerate, in CompilationInformation compilationInformation)
    {
        if (buildInfoToGenerate.Fake)
        {
            return base.GetGitCommitId(buildInfoToGenerate, compilationInformation);
        }

        return new ValueSource<string>("c0dec0dec0dec0dec0dec0dec0dec0dec0dec0de\nLine 2", "Value was taken from git commit id.");
    }

    override public ValueSource<string> GetGitBranch(in BuildInformationToGenerate buildInfoToGenerate)
    {
        if (buildInfoToGenerate.Fake)
        {
            return base.GetGitBranch(buildInfoToGenerate);
        }

        return new ValueSource<string>("my-magic-branch\nLine 2", "Value was taken from git branch.");
    }

    public override ValueSource<string> GetAssemblyVersion(in BuildInformationToGenerate buildInfoToGenerate, in CompilationInformation compilationInformation)
    {
        if (buildInfoToGenerate.Fake)
        {
            return base.GetAssemblyVersion(buildInfoToGenerate, compilationInformation);
        }

        return new ValueSource<string>("5.12.90\nLine 2", "Value was taken from assembly version attribute.");
    }

    public override ValueSource<string> GetOSVersion(in BuildInformationToGenerate buildInfoToGenerate)
    {
        if (buildInfoToGenerate.Fake)
        {
            return base.GetOSVersion(buildInfoToGenerate);
        }

        return new ValueSource<string>("Windows 49.0.22000.282\nLine 2", "Value was taken from Environment.OSVersion.");
    }

    public override ValueSource<string> GetDotNetVersion(in BuildInformationToGenerate buildInfoToGenerate)
    {
        if (buildInfoToGenerate.Fake)
        {
            return base.GetDotNetVersion(buildInfoToGenerate);
        }

        return new ValueSource<string>("49.0.100\nLine 2", "Value was taken from dotnet --version.");
    }

    public override string GetWorkloadList(in BuildInformationToGenerate buildInfoToGenerate)
    {
        if (buildInfoToGenerate.Fake)
        {
            return base.GetWorkloadList(buildInfoToGenerate);
        }

        return @"
Installed Workload Id      Manifest Version       Installation Source
----------------------------------------------------------------------------------------
maccatalyst                17.2.8053/8.0.100      VS 17.10.35004.147, VS 17.11.35005.142
ios                        17.2.8053/8.0.100      VS 17.10.35004.147, VS 17.11.35005.142
aspire                     8.0.0/8.0.100          VS 17.10.35004.147
android                    34.0.113/8.0.100       VS 17.10.35004.147, VS 17.11.35005.142
wasm-tools                 8.0.5/""8.0.100""      VS 17.10.35004.147
maui-windows               8.0.40/8.0.""100""     VS 17.10.35004.147, VS 17.11.35005.142
";
    }
}

public class EvilInformationProviderTestHelper : InformationProviderTestHelper
{
    public const string EvilString = """This is an evil string with a lot\aof new \ttabs\vand other evil "characters". ;-)""";

    public override ValueSource<string> GetGitBranch(in BuildInformationToGenerate buildInfoToGenerate)
    {
        return new ValueSource<string>(EvilString, "The value was generated in a dark place.");
    }
}
