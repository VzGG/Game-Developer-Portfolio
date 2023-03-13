using UnityEngine;

[System.Serializable]
public class EN : Stat
{
    public EN(float givenEN)
    {
        this.Value = givenEN;
        this.Name = "EN";
        this.RatingPerStat = 4f;
        this.Description = "The character's energy, a character cannot live without one.";
    }
}