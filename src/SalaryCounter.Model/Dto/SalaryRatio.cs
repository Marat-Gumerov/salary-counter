using Newtonsoft.Json;
using SalaryCounter.Model.Attribute;
using SalaryCounter.Model.Util;

namespace SalaryCounter.Model.Dto
{
    /// <summary>
    ///     Salary bonus levels specification
    /// </summary>
    [ModelExample(typeof(SalaryRatioExample))]
    public sealed class SalaryRatio
    {
        /// <summary>
        ///     Bonus coefficient for experience
        /// </summary>
        [JsonProperty]
        public decimal ExperienceBonus { get; set; }

        /// <summary>
        ///     Maximum level of bonus for experience
        /// </summary>
        [JsonProperty]
        public decimal ExperienceBonusMaximum { get; set; }

        /// <summary>
        ///     Bonus coefficient for subordinates
        /// </summary>
        [JsonProperty]
        public decimal SubordinateBonus { get; set; }
    }

    internal class SalaryRatioExample : Example<SalaryRatio>
    {
        protected override SalaryRatio Get(IExampleService exampleService) => new SalaryRatio
        {
            ExperienceBonus = 0.05m,
            ExperienceBonusMaximum = 0.4m,
            SubordinateBonus = 0.005m
        };
    }
}
