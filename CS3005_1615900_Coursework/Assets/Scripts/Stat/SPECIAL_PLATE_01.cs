using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPECIAL_PLATE_01 : SPECIAL
{
    public SPECIAL_PLATE_01()
    {
        this.Value = 60f;
        this.Description = "SPECIAL: LEGENDARY PLATE BUILT FOR ALMOST TANKING EVERYTHING WITH LITTLE TO NONE SCRATCHES";
        this.RatingPerStat = 4;
        this._ratingerPerStatBonus = 0;
    }

    public override void SpecialEffect(System.Object obj)
    {
        base.SpecialEffect(obj);

        // Apply 65% damage reduction
        // DEF stat is just like a health bar.
        Armour armour = (Armour)obj;

        armour.canDamageReduction = true;
        armour.damageReduction = this.Value;
    }
}