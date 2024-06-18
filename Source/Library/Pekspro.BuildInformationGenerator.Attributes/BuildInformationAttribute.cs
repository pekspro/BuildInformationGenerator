namespace Pekspro.BuildInformationGenerator
{
    /// <summary>
    /// Indicates that build information should be generated in the class.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    [System.Diagnostics.Conditional("PEKSPRO_BUILDINFORMATION_PRESERVE_ATTRIBUTES")]
    public class BuildInformationAttribute : System.Attribute
    {
        /// <summary>
        /// Whether build time (in UTC) should be included.
        /// Is set to false by default.
        /// </summary>
        public bool AddBuildTime { get; set; }

        /// <summary>
        /// Whether build local build time should be included.
        /// Is set to false by default.
        /// </summary>
        public bool AddLocalBuildTime { get; set; }

        /// <summary>
        /// Whether the assembly version should included.
        /// Is set to false by default.
        /// </summary>
        public bool AddAssemblyVersion { get; set; }

        /// <summary>
        /// Whether to include the OS version of the building machine.
        /// Is set false by default.
        /// </summary>
        public bool AddOSVersion { get; set; }

        /// <summary>
        /// Whether to include the git commit id in the generated class.
        /// Is set false by default.
        /// </summary>
        public bool AddGitCommitId { get; set; }

        /// <summary>
        /// Whether to include the git branch in the generated class.
        /// Is set false by default.
        /// </summary>
        public bool AddGitBranch { get; set; }

        /// <summary>
        /// Whether to include the .NET SDK version in the generated class.
        /// Is set false by default.
        /// </summary>
        public bool AddDotNetSdkVersion { get; set; }
        
        /// <summary>
        /// Weather to include the .NET MAUI workload version in the generated class.
        /// Is set false by default.
        /// </summary>
        public bool AddWorkloadMaui { get; set; }

        /// <summary>
        /// Weather to include the .NET Wasm-tools workload version in the generated class.
        /// Is set false by default.
        /// </summary>
        public bool AddWorkloadWasmTools { get; set; }

        /// <summary>
        /// If values should be faked when the DEBUG symbol is defined.
        /// Is set true by default.
        /// </summary>
        public bool FakeIfDebug { get; set; } = true;

        /// <summary>
        /// If values should be faked when the RELEASE symbol is defined.
        /// Is set false by default.
        /// </summary>
        public bool FakeIfRelease { get; set; }
    }
}
