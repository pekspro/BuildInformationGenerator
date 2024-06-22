namespace Pekspro.BuildInformationGenerator;

public readonly record struct BuildInformationToGenerate
{
    public readonly string Name;
    public readonly string FullFileName;
    public readonly string FilePath;
    public readonly string? Namespace;
    public readonly bool AddBuildTime;
    public readonly bool AddLocalBuildTime;
    public readonly bool AddGitCommitId;
    public readonly bool AddGitBranch;
    public readonly bool AddAssemblyVersion;
    public readonly bool AddOSVersion;
    public readonly bool AddDotNetSdkVersion;
    public readonly bool AddWorkloadMauiVersion;
    public readonly bool AddWorkloadWasmToolsVersion;

    public readonly bool Fake;
    
    public BuildInformationToGenerate(
        string name,
        string? ns,
        string fullFileName,
        bool addBuildTime,
        bool addLocalBuildTime,
        bool addGitCommitId,
        bool addGitBranch,
        bool addAssemblyVersion,
        bool addOSVersion,
        bool addDotNetSdkVersion,
        bool addWorkloadMauiVersion,
        bool addWorkloadWasmToolsVersion,
        bool fake
        )
    {
        Name = name;
        Namespace = ns;
        FullFileName = fullFileName;

        if (fullFileName != string.Empty)
        {
            FilePath = Path.GetDirectoryName(fullFileName);
        }
        else
        {
            FilePath = string.Empty;
        }

        AddBuildTime = addBuildTime;
        AddLocalBuildTime = addLocalBuildTime;
        AddAssemblyVersion = addAssemblyVersion;
        AddGitCommitId = addGitCommitId;
        AddGitBranch = addGitBranch;
        AddOSVersion = addOSVersion;
        AddDotNetSdkVersion = addDotNetSdkVersion;
        AddWorkloadMauiVersion = addWorkloadMauiVersion;
        AddWorkloadWasmToolsVersion = addWorkloadWasmToolsVersion;
        Fake = fake;        
    }
}