using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_ACCESSORY_01 : SPECIAL
{
    private float _damageReductionBonus = 20f;
    private float _energyCostReductionBonus = 20f;                // 20% energy cost reduction
    private float _healthRegenBonus = 8f;
    private float _criticalChanceBonus = 12.5f;
    private float _criticalDamageMultiplierBonus = 0.75f;

    public SPECIAL_ACCESSORY_01()
    {
        this.Value = 10f;                                   // +10 atk bonus
        this.Description = $"Special effect: grants {Value} bonus damage, {_damageReductionBonus}% damage reduction, {_energyCostReductionBonus}% energy cost reduction, {_healthRegenBonus} health regen, {_criticalChanceBonus}% chance to critical to deal {_criticalDamageMultiplierBonus * 100f}% more damage";
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

        myStat.EnergyCostReduction += this._energyCostReductionBonus;

        myStat.DamageReduction += this._damageReductionBonus;

    }

    public override void RemoveSpecialEffect(MyStat myStat)
    {
        base.RemoveSpecialEffect(myStat);

        myStat.ATK -= this.Value;

        myStat.CriticalChance -= this._criticalChanceBonus;
        myStat.CriticalDamage -= this._criticalDamageMultiplierBonus;

        myStat.HealthRegen -= this._healthRegenBonus;

        myStat.EnergyCostReduction -= this._energyCostReductionBonus;

        myStat.DamageReduction -= this._damageReductionBonus;
    }
}
