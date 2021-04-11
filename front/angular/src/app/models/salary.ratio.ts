export class SalaryRatio {
    experienceBonus: number | undefined;
    experienceBonusMaximum: number | undefined;
    subordinateBonus: number | undefined;

    constructor() {
    }

    static fromData(other: any): SalaryRatio {
        let ratio = new SalaryRatio();
        ratio.experienceBonus = Number(other.experienceBonus) || 0;
        ratio.experienceBonusMaximum = Number(other.experienceBonusMaximum) || 0;
        ratio.subordinateBonus = Number(other.subordinateBonus) || 0;
        return ratio;
    }
}
