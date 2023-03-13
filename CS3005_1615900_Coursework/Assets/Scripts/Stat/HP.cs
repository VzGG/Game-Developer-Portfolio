using UnityEngine;

[System.Serializable]
public class HP : Stat
{
    public HP(float givenHP)
    {
        this.Value = givenHP;
        this.Name = "HP";
        this.RatingPerStat = 4f;
        this.Description = "The character's health, a character cannot live without one.";
    }
}