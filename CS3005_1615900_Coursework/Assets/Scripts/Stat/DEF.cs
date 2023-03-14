using UnityEngine;

[System.Serializable]
public class DEF : Stat
{
    public DEF()
    {
        this.Name = "DEF";
        this.RatingPerStat = 7f;
        this.Description = "The character's defense, it is how much a character receives less damage";

        this.Value = Random.Range(DEFUtility.defRanges[0], DEFUtility.defRanges[1]);
    }
    public DEF(float givenDEF)
    {
        this.Value = givenDEF;
        this.Name = "DEF";
        this.RatingPerStat = 7f;
        this.Description = "The character's defense, it is how much a character receives less damage";
    }
}

public static class DEFUtility
{
    public static readonly int[] defRanges = new int[] { 1, 25 };
}