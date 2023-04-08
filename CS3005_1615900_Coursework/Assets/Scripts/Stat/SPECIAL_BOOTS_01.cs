using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_BOOTS_01 : SPECIAL
{
    public SPECIAL_BOOTS_01()
    {
        this.Value = 30f;
        this.Description = "SPECIAL: LEGENDARY BOOTS that has a chance to nullify damage taken.";
        this.RatingPerStat = 9;
        this._ratingerPerStatBonus = 0;
    }

    public override void SpecialEffect(object obj)
    {
        base.SpecialEffect(obj);

        Health playerHealth = (Health)obj;
        playerHealth.canEvadeDamage = true;
        playerHealth.evasionRate = this.Value;
    }
}
