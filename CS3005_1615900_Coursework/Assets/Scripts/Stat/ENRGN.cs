using UnityEngine;

[System.Serializable]
public class ENRGN : Stat
{
    public ENRGN()
    {
        this.Name = "EN RGN";
        this.RatingPerStat = 8f;
        this.Description = "The character's energy regeneration, it determines when a character can and cannot perform actions.";

        this.Value = Random.Range(ENRGNUtility.enrgnRanges[0], ENRGNUtility.enrgnRanges[1]);
    }
    public ENRGN(float givenENRGN)
    {
        this.Value = givenENRGN;
        this.Name = "EN RGN";
        this.RatingPerStat = 8f;
        this.Description = "The character's energy regeneration, it determines when a character can and cannot perform actions.";
    }
}

public static class ENRGNUtility
{
    public static readonly int[] enrgnRanges = new int[] { 1, 27 };
}