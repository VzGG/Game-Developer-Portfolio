using UnityEngine;

[System.Serializable]
public class HP : Stat
{
    public HP()
    {
        this.Name = "HP";
        this.RatingPerStat = 4f;
        this.Description = "The character's health, a character cannot live without one.";
    }
    public HP(float givenHP)
    {
        this.Value = givenHP;
        this.Name = "HP";
        this.RatingPerStat = 4f;
        this.Description = "The character's health, a character cannot live without one.";
    }

    public override void RandomStatFlat()
    {
        this.Value = Random.Range(HPUtility.hpRanges[0], HPUtility.hpRanges[1]);
    }

    public override void RandomStatPercent()
    {
        this.Value = Random.Range(HPUtility.hpRanges[2], HPUtility.hpRanges[3]);
        this._ratingerPerStatBonus = 0.45f;
    }
}

public static class HPUtility
{
    public static readonly int[] hpRanges = new int[] 
    { 
        1, 50,
        5, 45 // But how do we adjust the Rating per stat?
    };
}