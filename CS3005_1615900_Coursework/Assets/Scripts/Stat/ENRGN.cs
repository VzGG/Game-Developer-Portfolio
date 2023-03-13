using UnityEngine;

[System.Serializable]
public class ENRGN : Stat
{
    public ENRGN(float givenENRGN)
    {
        this.Value = givenENRGN;
        this.Name = "EN RGN";
        this.RatingPerStat = 8f;
        this.Description = "The character's energy regeneration, it determines when a character can and cannot perform actions.";
    }
}