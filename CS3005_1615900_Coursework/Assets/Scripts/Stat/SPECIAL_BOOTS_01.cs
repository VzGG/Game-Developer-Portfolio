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

    // Should only be called once
    public override void SpecialEffect(object obj)
    {
        base.SpecialEffect(obj);

        Health playerHealth = (Health)obj;
        playerHealth.canEvadeDamage = true;
        playerHealth.evasionRate += this.Value;
    }

    public override void RemoveSpecialEffect(object obj)
    {
        base.RemoveSpecialEffect(obj);

        Health playerHealth = (Health)obj;

        // What happens if there are legendaries overlapping each other?
        // Use set function and add to current!

        playerHealth.evasionRate -= this.Value;
        if (playerHealth.evasionRate <= 0f)
            playerHealth.canEvadeDamage = false;
    }
}
