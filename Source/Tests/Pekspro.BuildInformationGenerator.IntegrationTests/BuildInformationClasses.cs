namespace Pekspro.BuildInformationGenerator.Tests
{
    using Pekspro.BuildInformationGenerator;

    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true, AddBuildTime = true)]
    public partial class BuildInformationBuildTime
    {

    }

    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true, AddLocalBuildTime = true)]
    public partial class BuildInformationLocalBuildTime
    {

    }

    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true, AddGitCommitId = true)]
    public partial class BuildInformationGitCommitId
    {

    }

    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true, AddGitBranch = true)]
    public partial class BuildInformationGitBranch
    {

    }

    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true, AddAssemblyVersion = true)]
    public partial class BuildInformationAssemblyVersion
    {

    }

    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true, AddOSVersion = true)]
    public partial class BuildInformationOSVersion
    {

    }

    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true, AddDotNetSdkVersion = true)]
    public partial class BuildInformationDotNetSdkVersion
    {

    }

    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true, AddWorkloadMaui = true)]
    public partial class BuildInformationWorkloadMauiVersion
    {

    }

    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true, AddWorkloadWasmTools = true)]
    public partial class BuildInformationWorkloadWasmToolsVersion
    {

    }

    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true, AddBuildTime = true, AddLocalBuildTime = true, AddGitCommitId = true, AddGitBranch = true, AddAssemblyVersion = true, AddOSVersion = true, AddDotNetSdkVersion = true, AddWorkloadMaui = true, AddWorkloadWasmTools = true)]
    public partial class BuildInformationAll
    {

    }
}

