using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_GLOVE_01 : SPECIAL
{
    private float _criticalMultiplierBonus = 1.5f;

    public SPECIAL_GLOVE_01()
    {
        this.Value = 25f;               // 25% chance
        this.Description = $"Special: grant {Value}% chance to critical hits that deal {_criticalMultiplierBonus * 100f}% more damage.";
        this.RatingPerStat = 12f;
        this._ratingerPerStatBonus = 0;
    }

    public override void SpecialEffect(MyStat myStat)
    {
        base.SpecialEffect(myStat);

        myStat.CriticalChance += this.Value;
        myStat.CriticalDamage += this._criticalMultiplierBonus;
    }

    public override void RemoveSpecialEffect(MyStat myStat)
    {
        base.RemoveSpecialEffect(myStat);

        myStat.CriticalChance -= this.Value;
        myStat.CriticalDamage -= this._criticalMultiplierBonus;
    }
}
