using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SalaryCounter.Api.Extension
{
    internal static class ObjectExtension
    {
        public static Stream ToStream(this object value)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                },
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return new MemoryStream(Encoding.UTF8.GetBytes(json));
        }
    }
}