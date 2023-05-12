using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_ACCESSORY_01 : SPECIAL
{
    private float _damageReductionBonus = 20f;
    private float _evasionRateBonus = 12.5f;                // 12.5% to evade
    private float _healthRegenBonus = 8f;
    private float _criticalChanceBonus = 12.5f;
    private float _criticalDamageMultiplierBonus = 0.75f;

    public SPECIAL_ACCESSORY_01()
    {
        this.Value = 10f;                                   // +10 atk bonus
        this.Description = "Special effect: grants 10 bonus damage, 20% damage reduction, 12.5% evasion, 8 health regen, 12.5% chance to critical to deal 75% more damage";
        this.RatingPerStat = 30f;
        this._ratingerPerStatBonus = 0;
    }

    public override void SpecialEffect(MyStat myStat)
    {
        base.SpecialEffect(myStat);

        myStat.ATK += this.Value;

        myStat.CriticalChance += this._criticalChanceBonus;
        myStat.CriticalDamage += this._criticalDamageMultiplierBonus;

        myStat.HealthRegen += this._healthRegenBonus;

        myStat.EvasionRate += this._evasionRateBonus;

        myStat.DamageReduction += this._damageReductionBonus;

    }

    public override void RemoveSpecialEffect(MyStat myStat)
    {
        base.RemoveSpecialEffect(myStat);

        myStat.ATK -= this.Value;

        myStat.CriticalChance -= this._criticalChanceBonus;
        myStat.CriticalDamage -= this._criticalDamageMultiplierBonus;

        myStat.HealthRegen -= this._healthRegenBonus;

        myStat.EvasionRate -= this._evasionRateBonus;

        myStat.DamageReduction -= this._damageReductionBonus;
    }
}
