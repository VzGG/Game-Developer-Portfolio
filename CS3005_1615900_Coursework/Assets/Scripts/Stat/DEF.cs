using UnityEngine;

[System.Serializable]
public class DEF : Stat
{
    public DEF()
    {
        this.Name = "DEF";
        this.RatingPerStat = 7f;
        this.Description = "The character's defense, it is how much a character receives less damage";
    }
    public DEF(float givenDEF)
    {
        this.Value = givenDEF;
        this.Name = "DEF";
        this.RatingPerStat = 7f;
        this.Description = "The character's defense, it is how much a character receives less damage";
    }

    public override void RandomStatFlat()
    {
        this.Value = Random.Range(DEFUtility.defRanges[0], DEFUtility.defRanges[1]);
    }

    public override void RandomStatPercent()
    {
        this.Value = Random.Range(DEFUtility.defRanges[2], DEFUtility.defRanges[3]);
        this._ratingerPerStatBonus = 0.45f;
    }
}

public static class DEFUtility
{
    public static readonly int[] defRanges = new int[] 
    { 
        1, 25,
        5, 35
    };
}