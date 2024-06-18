namespace Pekspro.BuildInformationGenerator;

public readonly record struct CompilationInformation
{
    public readonly DateTimeOffset BuildStartTime;

    public readonly string AssemblyVersion;

    public readonly string AssemblyInformationalVersion;

    public CompilationInformation(DateTimeOffset buildStartTime, string assemblyVersion, string assemblyInformationalVersion)
    {
        BuildStartTime = buildStartTime;
        AssemblyVersion = assemblyVersion;
        AssemblyInformationalVersion = assemblyInformationalVersion;
    }
}
