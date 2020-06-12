using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SalaryCounter.Model.Extension
{
    /// <summary>
    ///     Object extensions
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        ///     Converts an object to Json stream
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Stream ToJsonStream(this object value) =>
            new MemoryStream(Encoding.UTF8.GetBytes(value.ToJson()));

        /// <summary>
        ///     Converts an object to Json
        /// </summary>
        private static string ToJson(this object value) =>
            JsonConvert.SerializeObject(value, new JsonSerializerSettings().Configure());
    }
}