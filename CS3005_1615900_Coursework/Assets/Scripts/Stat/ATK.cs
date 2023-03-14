using UnityEngine;

[System.Serializable]
public class ATK : Stat
{
    public ATK()
    {
        this.Name = "ATK";
        this.RatingPerStat = 9f;
        this.Description = "The character's attack, it is how much a character deals damage to one another.";

        this.Value = Random.Range(ATKUtility.atkRanges[0], ATKUtility.atkRanges[1]);
    }
    public ATK(float givenATK)
    {
        this.Value = givenATK;
        this.Name = "ATK";
        this.RatingPerStat = 9f;
        this.Description = "The character's attack, it is how much a character deals damage to one another.";
    }
}

public static class ATKUtility
{
    public static readonly int[] atkRanges = new int[] { 1, 25 };
}