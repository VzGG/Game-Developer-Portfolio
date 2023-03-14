using UnityEngine;

[System.Serializable]
public class ATKSPD : Stat
{
    public ATKSPD()
    {
        this.Name = "ATK SPD";
        this.RatingPerStat = 8f;
        this.Description = "The character's attack speed, it determines how fast a character attacks";

        this.Value = Random.Range(ATKSPDUtility.atkspdRanges[0], ATKSPDUtility.atkspdRanges[1]);
    }
    public ATKSPD(float givenATKSPD)
    {
        this.Value = givenATKSPD;
        this.Name = "ATK SPD";
        this.RatingPerStat = 8f;
        this.Description = "The character's attack speed, it determines how fast a character attacks";
    }
}

public static class ATKSPDUtility
{
    public static readonly int[] atkspdRanges = new int[] {1, 30};
}