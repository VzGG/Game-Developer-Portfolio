using UnityEngine;

[System.Serializable]
public class EN : Stat
{
    public EN()
    {
        this.Name = "EN";
        this.RatingPerStat = 4f;
        this.Description = "The character's energy, a character cannot live without one.";

        this.Value = Random.Range(ENUtility.enRanges[0], ENUtility.enRanges[1]);
    }
    public EN(float givenEN)
    {
        this.Value = givenEN;
        this.Name = "EN";
        this.RatingPerStat = 4f;
        this.Description = "The character's energy, a character cannot live without one.";
    }
}

public static class ENUtility
{
    public static readonly int[] enRanges = new int[] { 1, 50 };
}