using System;
using UnityEngine;

[System.Serializable]
public abstract class Stat
{
    [SerializeField] protected string Name;
    [SerializeField] public float Value;
    [SerializeField] protected float RatingPerStat;
    [SerializeField] protected string Description;
    protected float _ratingerPerStatBonus;

    public string GetName() { return this.Name; }
    public float GetRatingPerStat() { return this.RatingPerStat; }
    public string GetDescription() { return this.Description; }
    public float GetRatingPerStatBonus() { return this._ratingerPerStatBonus; }
    public abstract void RandomStatFlat();
    public abstract void RandomStatPercent();
    public virtual void SpecialEffect(System.Object obj) { }
    public virtual void SpecialEffect(params System.Object[] obj) { }
    public virtual void RemoveSpecialEffect(System.Object obj) { }
    public virtual void RemoveSpecialEffect(params System.Object[] obj) { }
}

public static class StatUtility
{
    public static readonly Type[] statTypes = new Type[6] { typeof(HP), typeof(EN),
        typeof(ATK), typeof(DEF),
        typeof(ATKSPD), typeof(ENRGN)};

    public static Stat RandomStat(int index)
    {
        int randomClass = UnityEngine.Random.Range(0, statTypes.Length);

        // Generate a random stat class either: HP, EN, ATK, DEF, ENRGN, ATKSPD
        Stat stat = (Stat)Activator.CreateInstance(statTypes[randomClass]);

        if (index == (int)Rarity.Epic)
        {
            stat.RandomStatPercent();
        }
        //else if (index == (int)Rarity.Legendary)
        //{
        //    // Implement legendary stats
        //    // Add something to the value - surely all legendary special effects have stats to it
        //}
        else
        {
            stat.RandomStatFlat();
        }
        

        return stat;
    }
}