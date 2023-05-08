using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_GLOVE_01 : SPECIAL
{
    private float _criticalMultiplierBonus = 1.5f;

    public SPECIAL_GLOVE_01()
    {
        this.Value = 25f;
        this.Description = "Special: grant 25% chance to critical hits that deal 150% more damage.";
        this.RatingPerStat = 12f;
        this._ratingerPerStatBonus = 0;
    }

    public override void SpecialEffect(object obj)
    {
        base.SpecialEffect(obj);

        // Let PlayerAttack know that critical is now on, and player can now attack with a chance of critical hits
        // Critical hits deal 250% damage
        PlayerAttack playerAttack = (PlayerAttack)obj;
        playerAttack.canCritical = true;
        playerAttack.criticalChance += this.Value;
        playerAttack.criticalDamageMultiplier += this._criticalMultiplierBonus;
    }

    public override void RemoveSpecialEffect(object obj)
    {
        base.RemoveSpecialEffect(obj);

        PlayerAttack playerAttack = (PlayerAttack)obj;
        playerAttack.criticalChance -= this.Value;
        playerAttack.criticalDamageMultiplier -= 0f;
        if (playerAttack.criticalChance <= 0f || playerAttack.criticalDamageMultiplier <= 1f)
            playerAttack.canCritical = false;
    }
}
