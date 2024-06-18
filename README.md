# Pekspro.BuildInformationGenerator

![Build status](https://github.com/pekspro/BuildInformationGenerator/actions/workflows/BuildAndTest.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Pekspro.BuildInformationGenerator.svg)](https://www.nuget.org/packages/Pekspro.BuildInformationGenerator/)

This project simplifies the process of adding build information to your .NET projects. It uses a source generator to embed details like build time, commit ID and branch directly into your code.

By default, the values are faked in debug mode. This can be changed in the `[BuildInformation]` attribute with the `FakeIfDebug` property.

## Usage

Create a new partial class in your project and add the `[BuildInformation]` (from the `Pekspro.BuildInformationGenerator` namespace) attribute and define which information you want. For example:

```csharp
[BuildInformation(AddBuildTime = true, AddGitCommit = true)]
partial class MyBuildInformation
{

}
```

Constants will automatically be added to this class that you can use like this:

```csharp
Console.WriteLine($"Build time: {MyBuildInformation.BuildTime}");
Console.WriteLine($"Commit id: {MyBuildInformation.GitCommitId}");
```

## Installation

Add the package to your application with:

```bash
dotnet add package Pekspro.BuildInformationGenerator
```

This adds a `<PackageReference>` to your project. It's recommended that you also add the attributes `PrivateAssets` and `ExcludeAssets` like below to exclude the source generator to your final assembly:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <PackageReference Include="Pekspro.BuildInformationGenerator" Version="0.0.1" 
    PrivateAssets="all" ExcludeAssets="runtime" />

</Project>
```

Setting `PrivateAssets="all"` means any projects referencing this one won't get a reference to the _Pekspro.BuildInformationGenerator_ package.

Setting `ExcludeAssets="runtime"` ensures the _Pekspro.BuildInformationGenerator.Attributes.dll_ file is not copied to your build output (it is not required at runtime).


## Configuration

The `[BuildInformation]` attribute has a number of properties you can set to control the generated class.

| Property               | Description                                         |
| ---------------------- | --------------------------------------------------- |
| `AddBuildTime`         | Build time (in UTC).                                |
| `AddLocalBuildTime`    | Local build time.                                   |
| `AddAssemblyVersion`   | Assembly version.                                   |
| `AddOSVersion`         | OS version of the machine where the build happens.  |
| `AddGitCommitId`       | Commit id.                                          |
| `AddGitBranch`         | Branch name.                                        |
| `AddDotNetSdkVersion`  | .NET SDK version.                                   |
| `AddWorkloadMaui`      | Workload for .NET MAUI.                             |
| `AddWorkloadWasmTools` | Workload for WebAssembly tools.                     |

If everything is set to true, the generated class will look like this:

```csharp
static partial class BuildInfoAll
{
    /// <summary>
    /// Build time: 2024-06-17 18:32:36
    /// Value was taken from the system clock.
    /// </summary>
    public static readonly global::System.DateTime BuildTime = new global::System.DateTime(638542459567895832L, global::System.DateTimeKind.Utc);

    /// <summary>
    /// Local build time: 2024-06-17 20:32:36 (+02:00)
    /// Value was taken from the system clock.
    /// </summary>
    public static readonly global::System.DateTimeOffset LocalBuildTime = new global::System.DateTimeOffset(638542531567895832L, new global::System.TimeSpan(72000000000));

    /// <summary>
    /// The commit id in Git at the time of build: cc567da972b6a564d631397ac6c2bc76960c6b67
    /// Value was taken from the AssemblyInformationalVersion attribute.
    /// </summary>
    public const string GitCommitId = "cc567da972b6a564d631397ac6c2bc76960c6b67";

    /// <summary>
    /// The short commit id in Git at the time of build: cc567da9
    /// Value was taken from the AssemblyInformationalVersion attribute.
    /// </summary>
    public const string GitShortCommitId = "cc567da9";

    /// <summary>
    /// Git branch: main
    /// Value was taken from the git branch command.
    /// </summary>
    public const string GitBranch = "main";

    /// <summary>
    /// Assembly version: 0.0.1.0
    /// Value was taken from assembly version attribute.
    /// </summary>
    public const string AssemblyVersionString = "0.0.1.0";

    /// <summary>
    /// OS version of the building machine: Microsoft Windows NT 10.0.22631.0
    /// Value was taken from Environment.OSVersion.
    /// </summary>
    public const string OSVersion = "Microsoft Windows NT 10.0.22631.0";

    /// <summary>
    /// .NET SDK version: 8.0.300
    /// Value was taken from the dotnet --version command.
    /// </summary>
    public const string DotNetSdkVersion = "8.0.300";

    /// <summary>
    /// Workload MAUI version: 8.0.40/8.0.100
    /// Value was taken from the dotnet workload list command.
    /// </summary>
    public const string WorkloadMauiVersion = "8.0.40/8.0.100";

    /// <summary>
    /// Workload wasm-tools version: 8.0.5/8.0.100
    /// Value was taken from the dotnet workload list command.
    /// </summary>
    public const string WorkloadWasmToolsVersion = "8.0.5/8.0.100";
}
```

### Performance

You can also specify if you want to have faked or real values:

| Property        | Default | Description                           | 
| --------------- | ------- | ------------------------------------- | 
| `FakeIfDebug`   | true    | Fake values if `DEBUG` is defined.    | 
| `FakeIfRelease` | false   | Fake values if `RELEASE` is defined.  |


## Requirements

This source generator requires the .NET 7 SDK. You can target earlier frameworks like .NET Core 3.1 etc, but the _SDK_ must be at least 7.0.100.

## Credits

This project is heavily inspired by the [NetEscapades.EnumGenerators](https://github.com/andrewlock/NetEscapades.EnumGenerators) project.
