using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Pekspro.BuildInformationGenerator.Tests;

public class InMemoryTests
{
    [Fact]
    public Task InGlobalNamespace()
    {
        const string input = @"
using Pekspro.BuildInformationGenerator;

[BuildInformation(FakeIfDebug = true, FakeIfRelease = true)]
public partial class MyBuildInformation
{

}
";
        var (diagnostics, output) = TestHelpers.GetGeneratedOutput<BuildInformationGenerator>(input);

        Assert.Empty(diagnostics);
        return Verifier.Verify(output).ScrubGeneratedCodeAttribute().UseDirectory("Snapshots");
    }

    [Fact]
    public Task InNamespace()
    {
        const string input = @"
using Pekspro.BuildInformationGenerator;

namespace MyNamespace
{
    [BuildInformation(FakeIfDebug = true, FakeIfRelease = true)]
    public partial class MyBuildInformation
    {

    }
}
";
        var (diagnostics, output) = TestHelpers.GetGeneratedOutput<BuildInformationGenerator>(input);

        Assert.Empty(diagnostics);
        return Verifier.Verify(output).ScrubGeneratedCodeAttribute().UseDirectory("Snapshots");
    }

    [Fact]
    public Task InSubNamespace()
    {
        const string input = @"
using Pekspro.BuildInformationGenerator;

namespace MyNamespace
{
    namespace MySubNamespace
    {
        [BuildInformation(FakeIfDebug = true, FakeIfRelease = true)]
        public partial class MyBuildInformation
        {

        }
    }
}
";
        var (diagnostics, output) = TestHelpers.GetGeneratedOutput<BuildInformationGenerator>(input);

        Assert.Empty(diagnostics);
        return Verifier.Verify(output).ScrubGeneratedCodeAttribute().UseDirectory("Snapshots");
    }
}
