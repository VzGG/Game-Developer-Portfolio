using UnityEngine;

[System.Serializable]
public class EN : Stat
{
    public EN()
    {
        this.Name = "EN";
        this.RatingPerStat = 4f;
        this.Description = "The character's energy, a character cannot live without one.";
    }
    public EN(float givenEN)
    {
        this.Value = givenEN;
        this.Name = "EN";
        this.RatingPerStat = 4f;
        this.Description = "The character's energy, a character cannot live without one.";
    }

    public override void RandomStatFlat()
    {
        this.Value = Random.Range(ENUtility.enRanges[0], ENUtility.enRanges[1]);
    }

    public override void RandomStatPercent()
    {
        this.Value = Random.Range(ENUtility.enRanges[2], ENUtility.enRanges[3]);
        this._ratingerPerStatBonus = 0.45f;
    }
}

public static class ENUtility
{
    public static readonly int[] enRanges = new int[] 
    { 
        1, 50, 
        5, 45 
    };
}