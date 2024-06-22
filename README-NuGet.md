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
Console.WriteLine($"Commit id: {MyBuildInformation.Git.CommitId}");
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

  <PackageReference Include="Pekspro.BuildInformationGenerator" Version="0.2.0" 
    PrivateAssets="all" ExcludeAssets="runtime" />

</Project>
```

Setting `PrivateAssets="all"` means any projects referencing this one won't get a reference to the _Pekspro.BuildInformationGenerator_ package.

Setting `ExcludeAssets="runtime"` ensures the _Pekspro.BuildInformationGenerator.Attributes.dll_ file is not copied to your build output (it is not required at runtime).

## Links

You can find more information and can report issues on [GitHub](https://github.com/pekspro/BuildInformationGenerator).
