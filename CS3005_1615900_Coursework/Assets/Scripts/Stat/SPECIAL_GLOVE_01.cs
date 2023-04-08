using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_GLOVE_01 : SPECIAL
{
    private float criticalMultiplier = 2.5f;

    public SPECIAL_GLOVE_01()
    {
        this.Value = 25f;
        this.Description = "SPECIAL: LEGENDARY GLOVES";
        this.RatingPerStat = 15;
        this._ratingerPerStatBonus = 0;
    }

    public override void SpecialEffect(object obj)
    {
        base.SpecialEffect(obj);

        // Let PlayerAttack know that critical is now on, and player can now attack with a chance of critical hits
        // Critical hits deal 250% damage
        PlayerAttack playerAttack = (PlayerAttack)obj;
        playerAttack.canCritical = true;
        playerAttack.criticalChance = this.Value;
        //playerAttack.criticalMultiplier = this.criticalMultiplier;
    }
}
