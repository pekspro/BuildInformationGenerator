using Pekspro.BuildInformationGenerator.Tests;
using Xunit;

namespace Pekspro.BuildInformationGenerator.IntegrationTests;

public class BuildInformationTests
{
    [Fact]
    public void BuildTime()
    {
        Assert.Equal(FakeValues.FakeBuildTime.UtcDateTime, BuildInformationBuildTime.BuildTime);

        Assert.Single(typeof(BuildInformationBuildTime).GetFields());
    }

    [Fact]
    public void LocalBuildTime()
    {
        Assert.Equal(FakeValues.FakeBuildTime, BuildInformationLocalBuildTime.LocalBuildTime);

        Assert.Single(typeof(BuildInformationLocalBuildTime).GetFields());
    }

    [Fact]
    public void GitCommitId()
    {
        Assert.Equal(FakeValues.FakeGitCommitId, BuildInformationGitCommitId.GitCommitId);
        Assert.Equal(BuildInformationGitCommitId.GitShortCommitId, FakeValues.FakeGitCommitId.Substring(0, 8));

        Assert.Equal(2, typeof(BuildInformationGitCommitId).GetFields().Length);
    }

    [Fact]
    public void GitBranch()
    {
        Assert.Equal(FakeValues.FakeGitBranch, BuildInformationGitBranch.GitBranch);

        Assert.Single(typeof(BuildInformationGitBranch).GetFields());
    }

    [Fact]
    public void AssemblyVersion()
    {
        Assert.Equal(FakeValues.FakeAssemblyVersion, BuildInformationAssemblyVersion.AssemblyVersionString);

        Assert.Single(typeof(BuildInformationAssemblyVersion).GetFields());
    }

    [Fact]
    public void OSVersion()
    {
        Assert.Equal(FakeValues.FakeOSVersion, BuildInformationOSVersion.OSVersion);

        Assert.Single(typeof(BuildInformationOSVersion).GetFields());
    }

    [Fact]
    public void DotNetSdkVersion()
    {
        Assert.Equal(FakeValues.FakeDotNetVersion, BuildInformationDotNetSdkVersion.DotNetSdkVersion);

        Assert.Single(typeof(BuildInformationDotNetSdkVersion).GetFields());
    }

    [Fact]
    public void WorkloadMauiVersion()
    {
        Assert.Equal(FakeValues.FakeWorkloadMaui, BuildInformationWorkloadMauiVersion.WorkloadMauiVersion);

        Assert.Single(typeof(BuildInformationWorkloadMauiVersion).GetFields());
    }

    [Fact]
    public void WorkloadWasmToolsVersion()
    {
        Assert.Equal(FakeValues.FakeWorkloadWasmTools, BuildInformationWorkloadWasmToolsVersion.WorkloadWasmToolsVersion);

        Assert.Single(typeof(BuildInformationWorkloadWasmToolsVersion).GetFields());
    }

    [Fact]
    public void All()
    {
        Assert.Equal(FakeValues.FakeBuildTime.UtcDateTime, BuildInformationAll.BuildTime);
        Assert.Equal(FakeValues.FakeBuildTime, BuildInformationAll.LocalBuildTime);
        Assert.Equal(FakeValues.FakeGitCommitId, BuildInformationAll.GitCommitId);
        Assert.Equal(FakeValues.FakeGitBranch, BuildInformationAll.GitBranch);
        Assert.Equal(FakeValues.FakeAssemblyVersion, BuildInformationAll.AssemblyVersionString);
        Assert.Equal(FakeValues.FakeOSVersion, BuildInformationAll.OSVersion);
        Assert.Equal(FakeValues.FakeDotNetVersion, BuildInformationAll.DotNetSdkVersion);
        Assert.Equal(FakeValues.FakeWorkloadMaui, BuildInformationAll.WorkloadMauiVersion);
        Assert.Equal(FakeValues.FakeWorkloadWasmTools, BuildInformationAll.WorkloadWasmToolsVersion);

        Assert.Equal(10, typeof(BuildInformationAll).GetFields().Length);
    }
}
