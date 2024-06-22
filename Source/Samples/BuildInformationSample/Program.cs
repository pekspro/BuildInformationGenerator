using Pekspro.BuildInformationGenerator;
using System;

Console.WriteLine("Hello, World!");
Console.WriteLine($"Build time: {BuildInfo.BuildTime}");
Console.WriteLine($"Local build time: {BuildInfo.LocalBuildTime}");
Console.WriteLine($"Assembly version: {BuildInfo.AssemblyVersionString}");
Console.WriteLine($"OS Version: {BuildInfo.OSVersion}");
Console.WriteLine($"Commit id: {BuildInfo.Git.CommitId}");
Console.WriteLine($"Short commit id: {BuildInfo.Git.ShortCommitId}");
Console.WriteLine($"Branch: {BuildInfo.Git.Branch}");
Console.WriteLine($".NET SDK: {BuildInfo.DotNetSdkVersion}");
Console.WriteLine($"Workload MAUI: {BuildInfo.Workloads.MauiVersion}");
Console.WriteLine($"Workload wasm tools: {BuildInfo.Workloads.WasmToolsVersion}");

[BuildInformation(
    FakeIfDebug = false,
    AddBuildTime = true,
    AddLocalBuildTime = true,
    AddAssemblyVersion = true,
    AddOSVersion = true,
    AddGitCommitId = true,
    AddGitBranch = true,
    AddDotNetSdkVersion = true,
    AddWorkloadMauiVersion = true,
    AddWorkloadWasmToolsVersion = true
    )]
partial class BuildInfo
{
}
