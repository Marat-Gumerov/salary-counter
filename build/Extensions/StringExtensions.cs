namespace NukeBuilder.Extensions
{
    static class StringExtensions
    {
        public static string EscapeMsBuildCommas(this string value) => value.Replace(",", "%2c");
    }
}