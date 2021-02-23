using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using NukeBuilder.Enumerations;
using NukeBuilder.Extensions;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace NukeBuilder
{
    [CheckBuildProjectConfigurations]
    [UnsetVisualStudioEnvironmentVariables]
    class Build : NukeBuild
    {
        /// Support plugins are available for:
        ///   - JetBrains ReSharper        https://nuke.build/resharper
        ///   - JetBrains Rider            https://nuke.build/rider
        ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
        ///   - Microsoft VSCode           https://nuke.build/vscode
        public static int Main() => Execute<Build>(x => x.Compile);

        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        readonly Configuration Configuration =
            IsLocalBuild ? Configuration.Debug : Configuration.Release;

        [Parameter("Test with coverage")] readonly bool Cover;

        [Solution] readonly Solution Solution;

        static AbsolutePath SourceDirectory => RootDirectory / "src";

        static AbsolutePath TestsDirectory => SourceDirectory / "SalaryCounter.ServiceTest";
        static AbsolutePath OutputDirectory => RootDirectory / "output";

        static AbsolutePath CoverageResults => RootDirectory / "coverage" / "report.xml";

        [UsedImplicitly]
        Target Clean => _ => _
            .Before(Restore)
            .Executes(() =>
            {
                SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                EnsureCleanDirectory(OutputDirectory);
            });

        Target Restore => _ => _
            .Executes(() => DotNetRestore(s => s
                .SetProjectFile(Solution)));

        Target Compile => _ => _
            .DependsOn(Restore)
            .Executes(() => DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()));

        [UsedImplicitly]
        Target UnitTest => _ => _
            .DependsOn(Compile)
            .Requires(() => Configuration.ToString() == Configuration.Debug)
            .Executes(() => DotNetTest(s => s
                .SetProcessWorkingDirectory(Solution.Directory)
                .SetProjectFile(TestsDirectory /
                                "SalaryCounter.ServiceTest.csproj")
                .EnableNoBuild()
                .When(Cover, _ => _
                    .EnableCollectCoverage()
                    .SetCoverletOutput(CoverageResults)
                    .SetCoverletOutputFormat(CoverletOutputFormat.opencover)
                    .AddProperty("SkipAutoProps", true)
                    .AddProperty("ExcludeByAttribute",
                        "Obsolete,GeneratedCode,CompilerGenerated".EscapeMsBuildCommas()))));
    }
}