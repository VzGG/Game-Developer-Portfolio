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
    public virtual void SpecialEffect(MyStat myStat) { }
    public virtual void RemoveSpecialEffect(MyStat myStat) { }
}

public static class StatUtility
{
    public static readonly Type[] statTypes = new Type[6] { typeof(HP), typeof(EN),
        typeof(ATK), typeof(DEF),
        typeof(ATKSPD), typeof(ENRGN)};

    public static Stat RandomStat(EquipmentCategory equipmentCategory)
    {
        Type statType;

        if (equipmentCategory == EquipmentCategory.Helmet)
        {
            // Probability
            int[] probabilities = new int[] 
            {
                50, // HP = 50% prob
                10, // EN = 10% prob
                4,  // ATK = 4%
                20, // DEF = 20%
                7,  // ATKSPD = 7%
                9   // ENRGN = 9% prob  
            };
            statType = RandomStatRarity(probabilities);

        }
        else if (equipmentCategory == EquipmentCategory.Plate)
        {
            int[] probabilities = new int[]
            {
                20, // HP
                10, // EN
                4,  // ATK
                50, // DEF
                7,  // ATKSPD
                9   // ENRGN
            };
            statType = RandomStatRarity(probabilities);
        }
        else if (equipmentCategory == EquipmentCategory.Gloves)
        {
            int[] probabilities = new int[]
            {
                4, // HP
                7, // EN
                20,  // ATK
                9, // DEF
                50,  // ATKSPD
                10   // ENRGN
            };
            statType = RandomStatRarity(probabilities);
        }
        else if (equipmentCategory == EquipmentCategory.Boots)
        {
            int[] probabilities = new int[]
            {
                7, // HP = 50% prob
                50, // EN = 20% prob
                10,  // ATK = 4%
                4, // DEF = 10%
                9,  // ATKSPD = 7%
                20   // ENRGN = 9% prob  
            };
            statType = RandomStatRarity(probabilities);
        }
        else
        {
            // Accessory
            int[] probabilities = new int[]
            {
                4, // HP = 50% prob
                7, // EN = 20% prob
                20,  // ATK = 4%
                9, // DEF = 10%
                10,  // ATKSPD = 7%
                50   // ENRGN = 9% prob  
            };
            statType = RandomStatRarity(probabilities);
        }

        // Generate a random distribution stat, if category:
        // helmet = HP appears 50% more,
        // plate = DEF appears 50% more
        // gloves = ATK SPD appears 50% more
        // boots = EN appears 50% more
        // Accessory = ENRGN appears 50% more

        Stat stat = (Stat)Activator.CreateInstance(statType);
        stat.RandomStatFlat();
        return stat;
    }

    public static Stat RandomStat()
    {
        int randomClass = UnityEngine.Random.Range(0, statTypes.Length);
        Stat stat = (Stat)Activator.CreateInstance(statTypes[randomClass]);
        stat.RandomStatFlat();
        return stat;
    }

    /// <summary>
    /// Epic stats only!
    /// </summary>
    /// <param name="givenStat"></param>
    public static Stat RandomStat(Type statType)
    {
        Stat stat = (Stat)Activator.CreateInstance(statType);
        stat.RandomStatPercent();
        return stat;
    }

    // Credit to: https://www.youtube.com/watch?v=Nu-HEbb_z54 for the inspiration
    private static Type RandomStatRarity(int[] probabilities)
    {
        float totalWeight = 0;
        for (int i = 0; i < probabilities.Length; i++)
        {
            totalWeight += (int)probabilities[i];
        }

        Debug.Log("Total weight: " + totalWeight);

        float probability = UnityEngine.Random.Range(0, totalWeight);
        float runningTotal = 0;
        // Determine which rarity to give
        for (int i = 0; i < probabilities.Length; i++)
        {
            runningTotal += (int)probabilities[i];
            if (probability < runningTotal)
            {
                return statTypes[i];
            }
        }

        // If it managed to go over, give HP! 
        return statTypes[0];
    }
}