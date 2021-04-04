using System.ComponentModel;
using Nuke.Common.Tooling;

namespace NukeBuilder.Enumerations
{
    [TypeConverter(typeof(TypeConverter<Configuration>))]
    public class Configuration : Enumeration
    {
        public static Configuration Debug = new()
        {
            Value = nameof(Debug)
        };
        public static Configuration Release = new()
        {
            Value = nameof(Release)
        };

        public static implicit operator string(Configuration value) => value.Value;

        public static implicit operator Configuration(string value) => new()
        {
            Value = value
        };
    }
}
