using System;
using UnityEngine;

[System.Serializable]
public abstract class Stat
{
    [SerializeField] protected string Name;
    [SerializeField] public float Value;
    [SerializeField] protected float RatingPerStat;
    [SerializeField] protected string Description;

    public string GetName() { return this.Name; }
    public float GetRatingPerStat() { return this.RatingPerStat; }
    public string GetDescription() { return this.Description; }
    public void SetValue(float givenValue) { this.Value = givenValue; }
}

public static class StatUtility
{
    public static readonly Type[] statTypes = new Type[6] { typeof(HP), typeof(EN),
        typeof(ATK), typeof(DEF),
        typeof(ATKSPD), typeof(ENRGN)};

    public static Stat RandomStat(Rarity rarity)
    {
        int randomClass = UnityEngine.Random.Range(0, statTypes.Length);

        // Generate a random stat class either: HP, EN, ATK, DEF, ENRGN, ATKSPD
        // Create a new object of them and set the value inside the constructor
        Stat stat = (Stat)Activator.CreateInstance(statTypes[randomClass]);

        //TO-DO: when we get equipment an epic rarity, only add 1 percentage stat

        return stat;
    }
}