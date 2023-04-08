using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Equipment : MonoBehaviour
{
    public Sprite sprite;
    public Rarity rarity;
    //public int weight;

    // Fix not showing non-monobehvaiour classes in the inspector: https://forum.unity.com/threads/abstract-class-in-list-only-shows-base-variables.999478/
    [SerializeField][SerializeReference] public List<Stat> stats = new List<Stat>();

    public float rating;
    // How to make paragraphs shown in the editor: https://answers.unity.com/questions/424874/showing-a-textarea-field-for-a-string-variable-in.html
/*    [TextArea(1, 30)]
    public string description;*/

    // special eff is method // need to set method or get teh special effects method instead 
}

public enum EquipmentCategory
{
    Helmet,
    Plate,
    Gloves,
    Boots,
    Accessory
}

public enum Rarity
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4,
    Legendary = 5
}

public static class RarityUtility
{
    public static readonly Rarity[] rarities = (Rarity[])Enum.GetValues(typeof(Rarity));

    public static readonly int[] probabilities = new int[]
    {
        47, // Common
        28, // Uncommon
        15, // Rare
        7,  // Epic
        3   // Legendary
    };

    // Credit to: https://www.youtube.com/watch?v=Nu-HEbb_z54 for the inspiration
    public static Rarity RandomRarity()
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
                return rarities[i];
            }
        }

        // If it managed to go over 
        return Rarity.Common;
    }
}