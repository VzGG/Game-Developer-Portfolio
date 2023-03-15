using UnityEngine;

[System.Serializable]
public class ATKSPD : Stat
{
    public ATKSPD()
    {
        this.Name = "ATK SPD";
        this.RatingPerStat = 8f;
        this.Description = "The character's attack speed, it determines how fast a character attacks";
    }
    public ATKSPD(float givenATKSPD)
    {
        this.Value = givenATKSPD;
        this.Name = "ATK SPD";
        this.RatingPerStat = 8f;
        this.Description = "The character's attack speed, it determines how fast a character attacks";
    }

    public override void RandomStatFlat()
    {
        this.Value = Random.Range(ATKSPDUtility.atkspdRanges[0], ATKSPDUtility.atkspdRanges[1]);
    }

    public override void RandomStatPercent()
    {
        this.Value = Random.Range(ATKSPDUtility.atkspdRanges[2], ATKSPDUtility.atkspdRanges[3]);
        this._ratingerPerStatBonus = 0.45f;
    }
}

public static class ATKSPDUtility
{
    public static readonly int[] atkspdRanges = new int[] 
    {
        1, 30,
        5, 35
    };
}