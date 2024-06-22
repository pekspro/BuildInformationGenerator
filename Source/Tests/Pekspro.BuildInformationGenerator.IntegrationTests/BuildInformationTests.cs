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
        Assert.Equal(FakeValues.FakeGitCommitId, BuildInformationGitCommitId.Git.CommitId);
        Assert.Equal(BuildInformationGitCommitId.Git.ShortCommitId, FakeValues.FakeGitCommitId.Substring(0, 8));

        Assert.Empty(typeof(BuildInformationGitCommitId).GetFields());
        Assert.Equal(2, typeof(BuildInformationGitCommitId.Git).GetFields().Length);
    }

    [Fact]
    public void GitBranch()
    {
        Assert.Equal(FakeValues.FakeGitBranch, BuildInformationGitBranch.Git.Branch);

        Assert.Empty(typeof(BuildInformationGitBranch).GetFields());
        Assert.Single(typeof(BuildInformationGitBranch.Git).GetFields());
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
        Assert.Equal(FakeValues.FakeWorkloadMaui, BuildInformationWorkloadMauiVersion.Workloads.MauiVersion);

        Assert.Empty(typeof(BuildInformationWorkloadMauiVersion).GetFields());
        Assert.Single(typeof(BuildInformationWorkloadMauiVersion.Workloads).GetFields());
    }

    [Fact]
    public void WorkloadWasmToolsVersion()
    {
        Assert.Equal(FakeValues.FakeWorkloadWasmTools, BuildInformationWorkloadWasmToolsVersion.Workloads.WasmToolsVersion);

        Assert.Empty(typeof(BuildInformationWorkloadWasmToolsVersion).GetFields());
        Assert.Single(typeof(BuildInformationWorkloadWasmToolsVersion.Workloads).GetFields());
    }

    [Fact]
    public void All()
    {
        Assert.Equal(FakeValues.FakeBuildTime.UtcDateTime, BuildInformationAll.BuildTime);
        Assert.Equal(FakeValues.FakeBuildTime, BuildInformationAll.LocalBuildTime);
        Assert.Equal(FakeValues.FakeGitCommitId, BuildInformationAll.Git.CommitId);
        Assert.Equal(FakeValues.FakeGitBranch, BuildInformationAll.Git.Branch);
        Assert.Equal(FakeValues.FakeAssemblyVersion, BuildInformationAll.AssemblyVersionString);
        Assert.Equal(FakeValues.FakeOSVersion, BuildInformationAll.OSVersion);
        Assert.Equal(FakeValues.FakeDotNetVersion, BuildInformationAll.DotNetSdkVersion);
        Assert.Equal(FakeValues.FakeWorkloadMaui, BuildInformationAll.Workloads.MauiVersion);
        Assert.Equal(FakeValues.FakeWorkloadWasmTools, BuildInformationAll.Workloads.WasmToolsVersion);

        Assert.Equal(5, typeof(BuildInformationAll).GetFields().Length);
        Assert.Equal(3, typeof(BuildInformationAll.Git).GetFields().Length);
        Assert.Equal(2, typeof(BuildInformationAll.Workloads).GetFields().Length);
    }
}
