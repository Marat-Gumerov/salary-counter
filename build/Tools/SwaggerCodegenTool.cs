using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using NukeBuilder.Enumerations;
using NukeBuilder.Extensions;
using static Nuke.Common.IO.FileSystemTasks;

namespace NukeBuilder.Tools
{
    class SwaggerCodegenTool
    {
        const string Version = "3.0.25";
        readonly AbsolutePath SolutionDirectory;
        readonly AbsolutePath TemporaryDirectory;

        internal SwaggerCodegenTool(AbsolutePath solutionDirectory, AbsolutePath temporaryDirectory)
        {
            SolutionDirectory = solutionDirectory;
            TemporaryDirectory = temporaryDirectory;
        }

        internal async Task Generate(SwaggerCodegenLanguage language, string specUrl, AbsolutePath workingDirectory)
        {
            EnsureExistingDirectory(workingDirectory);
            var languageString = language.AsToolArgument();
            var templateDir = SolutionDirectory / "codegen-templates" / languageString;
            await Run(_ => _
                .Add("generate")
                .Add($"-l {languageString}")
                .Add($"-i {specUrl}")
                .When(Directory.Exists(templateDir), __ => __
                    .Add($"-t {templateDir}")),
                workingDirectory);
        }

        async Task Run(Func<Arguments, Arguments> argumentsConfigurator,
            AbsolutePath? workingDirectory = null, IReadOnlyDictionary<string, string> environment = null)
        {
            var jar = await GetJar();
            var arguments = argumentsConfigurator(new Arguments()
                .Add($"-jar {jar}"));
            var java = ToolResolver.GetPathTool("java") ?? throw new Exception("Java not found");
            java.Invoke(arguments.RenderForExecution(), workingDirectory, environment);
        }

        async Task<AbsolutePath> GetJar()
        {
            var toolPath = TemporaryDirectory / "tools" /
                $"swagger-codegen-cli-{Version}.jar";
            if (File.Exists(toolPath)) return toolPath;
            EnsureExistingDirectory(toolPath.Parent);
            var client = new HttpClient();
            await using var file = File.OpenWrite(toolPath);
            const string mavenHost = "https://repo1.maven.org/maven2";
            const string packageLocation = "io/swagger/codegen/v3/swagger-codegen-cli";
            var packageFileName = $"swagger-codegen-cli-{Version}.jar";
            var getStream = await client.GetStreamAsync(
                $"{mavenHost}/{packageLocation}/{Version}/{packageFileName}");
            await getStream.CopyToAsync(file);
            return toolPath;
        }
    }
}
