using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_PLATE_01 : SPECIAL
{
    public SPECIAL_PLATE_01()
    {
        this.Value = 60f;
        this.Description = $"Special: grants {Value}% damage reduction to all attacks received.";
        this.RatingPerStat = 4.25f;
        this._ratingerPerStatBonus = 0;
    }

    public override void SpecialEffect(MyStat myStat)
    {
        base.SpecialEffect(myStat);

        myStat.DamageReduction += this.Value;
    }

    public override void RemoveSpecialEffect(MyStat myStat)
    {
        base.RemoveSpecialEffect(myStat);

        myStat.DamageReduction -= this.Value;
    }
}