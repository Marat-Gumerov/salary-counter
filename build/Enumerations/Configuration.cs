using Nuke.Common.Tooling;

namespace NukeBuilder.Enumerations
{
    public class Configuration : Enumeration
    {
        readonly string Name;

        Configuration(string name) => Name = name;

        public static Configuration Debug => new(nameof(Debug));
        public static Configuration Release => new(nameof(Release));

        public static implicit operator string(Configuration value)
        {
            return value.Name;
        }
    }
}