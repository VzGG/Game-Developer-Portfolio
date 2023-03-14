using UnityEngine;

[System.Serializable]
public class HP : Stat
{
    public HP()
    {
        this.Name = "HP";
        this.RatingPerStat = 4f;
        this.Description = "The character's health, a character cannot live without one.";

        this.Value = Random.Range(HPUtility.hpRanges[0], HPUtility.hpRanges[1]);
    }
    public HP(float givenHP)
    {
        this.Value = givenHP;
        this.Name = "HP";
        this.RatingPerStat = 4f;
        this.Description = "The character's health, a character cannot live without one.";
    }
}

public static class HPUtility
{
    public static readonly int[] hpRanges = new int[] { 1, 50 };
}