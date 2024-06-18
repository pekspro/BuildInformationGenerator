using System;

namespace Pekspro.BuildInformationGenerator;

public static class FakeValues
{
    public static readonly DateTimeOffset FakeBuildTime = new DateTimeOffset(2038, 1, 19, 3, 14, 15, TimeSpan.FromHours(-4));

    public const string FakeAssemblyVersion = "3.141.5";

    public const string FakeOSVersion = "Windows 95.1.2.3";

    public const string FakeDotNetVersion = "6.0.100";

    public const string FakeGitBranch = "fake-main";

    public const string FakeGitCommitId = "0123456789abcef0123456789abcef0123456789";

    public const string FakeWorkloadMaui = "1.0.1/0.1.0-fake";

    public const string FakeWorkloadWasmTools = "2.0.2/0.2.0-fake";

    public const string FakeValueSource = "Faked value.";
}
