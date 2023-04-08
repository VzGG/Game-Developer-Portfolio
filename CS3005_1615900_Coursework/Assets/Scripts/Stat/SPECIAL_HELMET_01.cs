using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_HELMET_01 : SPECIAL
{
    public SPECIAL_HELMET_01()
    {
        this.Value = 50;
        this.Description = "<Legendary description WIP>";
        this.RatingPerStat = 7;
        this._ratingerPerStatBonus = 0;
    }

    public override void SpecialEffect(System.Object obj)
    {
        base.SpecialEffect(obj);

        // Apply hp regen by +50
        Health health = (Health)obj;
        health.canRegen = true;
        health.healthRegen = this.Value;
       
        //health.SetHealth(health.GetHealth() + this.Value * Time.deltaTime);
        //if (health.GetHealth() > health.GetMaxHealth())
        //{
        //    health.SetHealth(health.GetMaxHealth());
        //}
    }
}
