export class SalaryRatio {

    constructor(
        public experienceBonus: number,
        public experienceBonusMaximum: number,
        public subordinateBonus: number
    ) {
    }

    static fromData(other: any): SalaryRatio {
        return new SalaryRatio(
            Number(other.experienceBonus) || 0,
            Number(other.experienceBonusMaximum) || 0,
            Number(other.subordinateBonus) || 0,
        );
    }
}
