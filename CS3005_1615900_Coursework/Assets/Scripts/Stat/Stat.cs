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
}

public static class StatUtility
{
    public static readonly Type[] statTypes = new Type[6] { typeof(HP), typeof(EN),
        typeof(ATK), typeof(DEF),
        typeof(ATKSPD), typeof(ENRGN)};

    public static readonly int[] hpRanges = new int[]
    {
        1, 50
    };

    public static readonly int[] enRanges = new int[]
    {
        1, 50
    };

    public static readonly int[] atkRanges = new int[]
    {
        1, 25
    };

    public static readonly int[] defRanges = new int[]
    {
        1, 25
    };

    public static readonly int[] atkspdRanges = new int[]
    {
        1, 30
    };

    public static readonly int[] enrgnRanges = new int[]
    {
        1, 27
    };

    public static Stat RandomStat()
    {
        int randomClass = UnityEngine.Random.Range(0, statTypes.Length);
        if (randomClass == 0)
        {
            return new HP(UnityEngine.Random.Range(hpRanges[0], hpRanges[1]));
        }
        else if (randomClass == 1)
        {
            return new EN(UnityEngine.Random.Range(enRanges[0], enRanges[1]));
        }
        else if (randomClass == 2)
        {
            return new ATK(UnityEngine.Random.Range(atkRanges[0], atkRanges[1]));
        }
        else if (randomClass == 3)
        {
            return new DEF(UnityEngine.Random.Range(defRanges[0], defRanges[1]));
        }
        else if (randomClass == 4)
        {
            return new ATKSPD(UnityEngine.Random.Range(atkspdRanges[0], atkspdRanges[1]));
        }
        else if (randomClass == 5)
        {
            return new ENRGN(UnityEngine.Random.Range(enrgnRanges[0], enrgnRanges[1]));
        }

        return null;
    }
}