using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SalaryCounter.Model.Extension
{
    /// <summary>
    /// JsonSerializerSettings extensions
    /// </summary>
    public static class JsonSerializerSettingsExtensions
    {
        /// <summary>
        ///     Configure <see cref="JsonSerializerSettings"/>
        /// </summary>
        public static JsonSerializerSettings Configure(this JsonSerializerSettings settings)
        {
            settings.Converters.Add(new StringEnumConverter());
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            return settings;
        }
    }
}