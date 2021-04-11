using System;
using NukeBuilder.Enumerations;

namespace NukeBuilder.Extensions
{
    static class SwaggerCodegenLanguageExtensions
    {
        public static string AsToolArgument(this SwaggerCodegenLanguage language)
        {
            return language switch
            {
                SwaggerCodegenLanguage.TypescriptAngular => "typescript-angular",
                _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
            };
        }
    }
}
