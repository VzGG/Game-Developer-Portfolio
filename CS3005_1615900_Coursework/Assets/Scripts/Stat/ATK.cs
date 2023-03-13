using UnityEngine;

[System.Serializable]
public class ATK : Stat
{
    public ATK(float givenATK)
    {
        this.Value = givenATK;
        this.Name = "ATK";
        this.RatingPerStat = 9f;
        this.Description = "The character's attack, it is how much a character deals damage to one another.";
    }
}