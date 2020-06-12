using Newtonsoft.Json;

namespace SalaryCounter.Model.Dto
{
    /// <summary>
    ///     Salary bonus levels specification
    /// </summary>
    public sealed class SalaryRatio
    {
        /// <summary>
        ///     Bonus coefficient for experience
        /// </summary>
        [JsonProperty]public decimal ExperienceBonus { get; set; }
        /// <summary>
        ///     Maximum level of bonus for experience
        /// </summary>
        [JsonProperty]public decimal ExperienceBonusMaximum { get; set; }
        /// <summary>
        ///     Bonus coefficient for subordinates
        /// </summary>
        [JsonProperty]public decimal SubordinateBonus { get; set; }
    }
}