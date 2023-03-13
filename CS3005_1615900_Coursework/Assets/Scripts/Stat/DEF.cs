using UnityEngine;

[System.Serializable]
public class DEF : Stat
{
    public DEF(float givenDEF)
    {
        this.Value = givenDEF;
        this.Name = "DEF";
        this.RatingPerStat = 7f;
        this.Description = "The character's defense, it is how much a character receives less damage";
    }
}