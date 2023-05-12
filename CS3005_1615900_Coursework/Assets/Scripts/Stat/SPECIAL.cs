using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SPECIAL : Stat
{
    public SPECIAL()
    {
        this.Name = "SPECIAL";
        
    }

    public override void RandomStatFlat()
    {
        throw new System.NotImplementedException();
    }

    public override void RandomStatPercent()
    {
        throw new System.NotImplementedException();
    }

    public override void SpecialEffect(MyStat myStat)
    {
        base.SpecialEffect(myStat);
    }
}
