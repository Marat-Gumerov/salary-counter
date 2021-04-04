using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Tooling;

namespace NukeBuilder.Tools
{
    class SwaggerCodegenTool
    {
        const string Version = "3.0.25";
        readonly AbsolutePath SolutionDirectory;

        internal SwaggerCodegenTool(AbsolutePath solutionDirectory) =>
            SolutionDirectory = solutionDirectory;

        internal async Task Run(Func<Arguments, Arguments> argumentsConfigurator,
            AbsolutePath? workingDirectory = null, Dictionary<string, string>? environment = null)
        {
            var jar = await GetJar();
            var arguments = argumentsConfigurator(new Arguments()
                .Add($"-jar {jar}"));
            var java = ToolResolver.GetPathTool("java") ?? throw new Exception("Java not found");
            java.Invoke(arguments.RenderForExecution(), workingDirectory, environment);
        }

        async Task<AbsolutePath> GetJar()
        {
            var toolPath = SolutionDirectory / ".tmp" / "tools" /
                $"swagger-codegen-cli-{Version}.jar";
            if (File.Exists(toolPath)) return toolPath;
            FileSystemTasks.EnsureExistingDirectory(toolPath.Parent);
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
