using System;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Pekspro.BuildInformationGenerator.Tests;

public class SourceGenerationHelperSnapshotTests
{
    private InformationProviderTestHelper _informationProvider = new InformationProviderTestHelper();
    private CompilationInformation _compilationInformation = new(DateTimeOffset.MinValue, "", "");

    [Fact]
    public Task Empty()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task BuildTimeFake()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: true,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task BuildTimeReal()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: true,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: false
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }


    [Fact]
    public Task LocalTimeFake()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: true,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task LocalTimeReal()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: true,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: false
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }


    [Fact]
    public Task GitCommitIdFake()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: true,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task GitCommitIdReal()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: true,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: false
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }


    [Fact]
    public Task GitBranchFake()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: true,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task GitBranchReal()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: true,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: false
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }


    [Fact]
    public Task AssemblyVersionFake()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: true,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task AssemblyVersionReal()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: true,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: false
            );

        CompilationInformation compilationInformation = new(DateTimeOffset.MinValue, "", "");

        var result = SourceGenerationHelper.GenerateExtensionClass(value, compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }


    [Fact]
    public Task OSVersionFake()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: true,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task OSVersionReal()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: true,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: false
            );

        CompilationInformation compilationInformation = new(DateTimeOffset.MinValue, "", "");

        var result = SourceGenerationHelper.GenerateExtensionClass(value, compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }


    [Fact]
    public Task DotnetVersionFake()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: true,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task DotnetVersionReal()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: true,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: false
            );

        CompilationInformation compilationInformation = new(DateTimeOffset.MinValue, "", "");

        var result = SourceGenerationHelper.GenerateExtensionClass(value, compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }


    [Fact]
    public Task WorkloadMauiFake()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: true,
            addWorkloadWasmTools: false,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task WorkloadMauiReal()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: true,
            addWorkloadWasmTools: false,
            fake: false
            );

        CompilationInformation compilationInformation = new(DateTimeOffset.MinValue, "", "");

        var result = SourceGenerationHelper.GenerateExtensionClass(value, compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }


    [Fact]
    public Task WorkloadWasmToolsFake()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: true,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task WorkloadWasmToolsReal()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: false,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: true,
            fake: false
            );

        CompilationInformation compilationInformation = new(DateTimeOffset.MinValue, "", "");

        var result = SourceGenerationHelper.GenerateExtensionClass(value, compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }



    [Fact]
    public Task AllFake()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: true,
            addLocalBuildTime: true,
            addGitCommitId: true,
            addGitBranch: true,
            addAssemblyVersion: true,
            addOSVersion: true,
            addDotNetSdkVersion: true,
            addWorkloadMaui: true,
            addWorkloadWasmTools: true,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task AllReal()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: true,
            addLocalBuildTime: true,
            addGitCommitId: true,
            addGitBranch: true,
            addAssemblyVersion: true,
            addOSVersion: true,
            addDotNetSdkVersion: true,
            addWorkloadMaui: true,
            addWorkloadWasmTools: true,
            fake: false
            );

        CompilationInformation compilationInformation = new(DateTimeOffset.MinValue, "", "");

        var result = SourceGenerationHelper.GenerateExtensionClass(value, compilationInformation, _informationProvider);

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }

    [Fact]
    public Task EvilGitBranch()
    {
        var value = new BuildInformationToGenerate(
            "ClassName",
            "My.Namespace",
            @"C:\code\file.cs",
            addBuildTime: false,
            addLocalBuildTime: false,
            addGitCommitId: false,
            addGitBranch: true,
            addAssemblyVersion: false,
            addOSVersion: false,
            addDotNetSdkVersion: false,
            addWorkloadMaui: false,
            addWorkloadWasmTools: false,
            fake: true
            );

        var result = SourceGenerationHelper.GenerateExtensionClass(value, _compilationInformation, new EvilInformationProviderTestHelper());

        return Verifier.Verify(result)
            .ScrubGeneratedCodeAttribute()
            .UseDirectory("Snapshots");
    }
}
