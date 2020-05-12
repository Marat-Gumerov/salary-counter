namespace SalaryCounter.Service.Model
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class SalaryRatio
    {
        public virtual decimal ExperienceBonus { get; set; }
        public virtual decimal ExperienceBonusMaximum { get; set; }
        public virtual decimal SubordinateBonus { get; set; }
    }
}