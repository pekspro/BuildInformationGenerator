using Pekspro.BuildInformationGenerator;
using System;

Console.WriteLine("Hello, World!");
Console.WriteLine($"Build time: {BuildInfo.BuildTime}");
Console.WriteLine($"Local build time: {BuildInfo.LocalBuildTime}");
Console.WriteLine($"Assembly version: {BuildInfo.AssemblyVersionString}");
Console.WriteLine($"OS Version: {BuildInfo.OSVersion}");
Console.WriteLine($"Commit id: {BuildInfo.GitCommitId}");
Console.WriteLine($"Short commit id: {BuildInfo.GitShortCommitId}");
Console.WriteLine($"Branch: {BuildInfo.GitBranch}");
Console.WriteLine($".NET SDK: {BuildInfo.DotNetSdkVersion}");
Console.WriteLine($"Workload MAUI: {BuildInfo.WorkloadMauiVersion}");
Console.WriteLine($"Workload wasm tools: {BuildInfo.WorkloadWasmToolsVersion}");

[BuildInformation(
    FakeIfDebug = false, 
    AddBuildTime = true,
    AddLocalBuildTime = true,
    AddAssemblyVersion = true,
    AddOSVersion = true,
    AddGitCommitId = true,
    AddGitBranch = true,
    AddDotNetSdkVersion = true,
    AddWorkloadMaui = true, 
    AddWorkloadWasmTools = true
    )]
partial class BuildInfo
{

}
