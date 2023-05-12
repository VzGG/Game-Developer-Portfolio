using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_HELMET_01 : SPECIAL
{
    public SPECIAL_HELMET_01()
    {
        this.Value = 50;
        this.Description = "Special: grants 50 health regen.";
        this.RatingPerStat = 7;
        this._ratingerPerStatBonus = 0;
    }

    public override void SpecialEffect(MyStat myStat)
    {
        base.SpecialEffect(myStat);

        myStat.HealthRegen += this.Value;
    }

    public override void RemoveSpecialEffect(MyStat myStat)
    {
        base.RemoveSpecialEffect(myStat);

        myStat.HealthRegen -= this.Value;
    }
}
