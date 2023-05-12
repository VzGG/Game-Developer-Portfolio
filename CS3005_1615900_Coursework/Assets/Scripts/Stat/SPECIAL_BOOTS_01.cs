using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_BOOTS_01 : SPECIAL
{
    public SPECIAL_BOOTS_01()
    {
        this.Value = 30f;
        this.Description = "Special: grants 30% chance to nullify damage taken.";
        this.RatingPerStat = 9;
        this._ratingerPerStatBonus = 0;
    }

    // Should only be called once
    public override void SpecialEffect(MyStat myStat)
    {
        base.SpecialEffect(myStat);

        myStat.EvasionRate += this.Value;
    }

    public override void RemoveSpecialEffect(MyStat myStat)
    {
        base.RemoveSpecialEffect(myStat);

        myStat.EvasionRate -= this.Value;
    }
}
