using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace Pekspro.BuildInformationGenerator;

[Generator]
public class BuildInformationGenerator : IIncrementalGenerator
{
    private const string ExtensionsAttribute = "Pekspro.BuildInformationGenerator.BuildInformationAttribute";

    public DateTimeOffset BuildStartTime { get; private set; }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        BuildStartTime = DateTimeOffset.Now;

        IncrementalValueProvider<CompilationInformation> compilation = context.CompilationProvider.Select((c, _) => CreateCompilationInformation(c));

        IncrementalValuesProvider<BuildInformationToGenerate> valuesProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    ExtensionsAttribute,
                    predicate: (node, _) => node is ClassDeclarationSyntax,
                    transform: CreateBuildInformationToGenerate)
            .WithTrackingName(TrackingNames.InitialExtraction)
            .Where(static m => m is not null)
            .Select(static (m, _) => m!.Value)
            .WithTrackingName(TrackingNames.RemovingNulls);

        var combinedProvider = valuesProvider.Combine(compilation);

        context.RegisterSourceOutput(combinedProvider,
             static (spc, toGenerate) => Execute(in toGenerate.Left, toGenerate.Right, spc));
    }

    static void Execute(in BuildInformationToGenerate toGenerate, CompilationInformation compilationInformation, SourceProductionContext context)
    {
        var result = SourceGenerationHelper.GenerateExtensionClass(toGenerate, compilationInformation, InformationProvider.Instance);

        context.AddSource(toGenerate.Name + ".g.cs", SourceText.From(result, Encoding.UTF8));
    }

    private CompilationInformation CreateCompilationInformation(Compilation compilation)
    {
        var assembly = compilation.Assembly;
        CompilationInformation compilationInformation = new CompilationInformation(BuildStartTime,
            GetVersionFromAssembly(assembly, "AssemblyVersionAttribute"),
            GetVersionFromAssembly(assembly, "AssemblyInformationalVersionAttribute"));
        return compilationInformation;
    }

    private static string GetVersionFromAssembly(ISymbol assembly, string attributeName)
    {
        var attribute = assembly.GetAttributes().FirstOrDefault(attr => attr.AttributeClass?.Name == attributeName);
        
        var version = attribute?.ConstructorArguments[0].Value!.ToString() ?? string.Empty;

        return version;
    }

    static BuildInformationToGenerate? CreateBuildInformationToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        INamedTypeSymbol? symbol = context.TargetSymbol as INamedTypeSymbol;
        if (symbol is null)
        {
            // nothing to do if this type isn't available
            return null;
        }

        ct.ThrowIfCancellationRequested();

        // Get the path to the source file
        string filePath = context.SemanticModel.SyntaxTree.FilePath;

        string name = symbol.Name;
        string? nameSpace = null;
        if (!symbol.ContainingNamespace.IsGlobalNamespace)
        {
            nameSpace = symbol.ContainingNamespace?.ToDisplayString();
        }

        bool addBuildTime = false;
        bool addLocalBuildTime = false;
        bool addAssemblyVersion = false;
        bool addOSVersion = false;
        bool addGitICommitId = false;
        bool addGitBranch = false;
        bool addDotNetVersion = false;
        bool addWorkloadMaui = false;
        bool addWorkloadWasmTools = false;
        bool fakeIfDebug = true;
        bool fakeIfRelease = false;
        bool fake = false;

        foreach (AttributeData attributeData in symbol.GetAttributes())
        {
            if (attributeData.AttributeClass?.Name != "BuildInformationAttribute" ||
                attributeData.AttributeClass.ToDisplayString() != ExtensionsAttribute)
            {
                continue;
            }

            foreach (KeyValuePair<string, TypedConstant> namedArgument in attributeData.NamedArguments)
            {
                if (namedArgument.Key == nameof(BuildInformationAttribute.AddBuildTime)
                    && namedArgument.Value.Value is bool buildTime)
                {
                    addBuildTime = buildTime;
                }

                if (namedArgument.Key == nameof(BuildInformationAttribute.AddLocalBuildTime)
                    && namedArgument.Value.Value is bool localTime)
                {
                    addLocalBuildTime = localTime;
                }

                if (namedArgument.Key == nameof(BuildInformationAttribute.AddAssemblyVersion)
                    && namedArgument.Value.Value is bool assembly)
                {
                    addAssemblyVersion = assembly;
                }

                if (namedArgument.Key == nameof(BuildInformationAttribute.AddOSVersion)
                    && namedArgument.Value.Value is bool os)
                {
                    addOSVersion = os;
                }

                if (namedArgument.Key == nameof(BuildInformationAttribute.AddGitCommitId)
                    && namedArgument.Value.Value is bool git)
                {
                    addGitICommitId = git;
                }

                if (namedArgument.Key == nameof(BuildInformationAttribute.AddGitBranch)
                    && namedArgument.Value.Value is bool gitBranch)
                {
                    addGitBranch = gitBranch;
                }

                if (namedArgument.Key == nameof(BuildInformationAttribute.AddDotNetSdkVersion)
                    && namedArgument.Value.Value is bool net)
                {
                    addDotNetVersion = net;
                }

                if (namedArgument.Key == nameof(BuildInformationAttribute.AddWorkloadMaui)
                    && namedArgument.Value.Value is bool maui)
                {
                    addWorkloadMaui = maui;
                }

                if (namedArgument.Key == nameof(BuildInformationAttribute.AddWorkloadWasmTools)
                    && namedArgument.Value.Value is bool wasmTools)
                {
                    addWorkloadWasmTools = wasmTools;
                }

                if (namedArgument.Key == nameof(BuildInformationAttribute.FakeIfDebug)
                    && namedArgument.Value.Value is bool fakeDebug)
                {
                    fakeIfDebug = fakeDebug;
                }

                if (namedArgument.Key == nameof(BuildInformationAttribute.FakeIfRelease)
                    && namedArgument.Value.Value is bool fakeRelease)
                {
                    fakeIfRelease = fakeRelease;
                }
            }
        }

        if (fakeIfDebug)
        {
            if (context.SemanticModel.SyntaxTree.Options.PreprocessorSymbolNames.Contains("DEBUG"))
            {
                fake = true;
            }
        }

        if (fakeIfRelease)
        {
            if (context.SemanticModel.SyntaxTree.Options.PreprocessorSymbolNames.Contains("RELEASE"))
            {
                fake = true;
            }
        }

        return new BuildInformationToGenerate
            (
                name,
                nameSpace,
                filePath,
                addBuildTime,
                addLocalBuildTime,
                addGitICommitId,
                addGitBranch,
                addAssemblyVersion,
                addOSVersion,
                addDotNetVersion,
                addWorkloadMaui,
                addWorkloadWasmTools,
                fake
            );
    }
}
