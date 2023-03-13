using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentRandomizer : MonoBehaviour
{
    [SerializeField] private Sprite[] _presetSprites;
    //[SerializeField] private Equipment[] _spawnedEquipment;
    [SerializeField] private GameObject _prefabEquipment;

    private void Start()
    {
        LoadSprites();
        GenerateEquipment();
    }

    private void LoadSprites()
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Sprites/Items");
        _presetSprites = new Sprite[loadedSprites.Length];
        _presetSprites = loadedSprites;
    }
    void GenerateEquipment()
    {

        int randomEquipmentSpawned = UnityEngine.Random.Range(10, 100);


        for (int i = 0; i < randomEquipmentSpawned; i++)
        {
            GameObject gameObject = Instantiate(_prefabEquipment);
            Equipment equipment = gameObject.GetComponent<Equipment>();

            int randomSprite = UnityEngine.Random.Range(0, _presetSprites.Length);
            equipment.sprite = _presetSprites[randomSprite];

            equipment.rarity = RarityUtility.RandomRarity();
            // Add random n random stats
            for (int j = 0; j < (int)equipment.rarity; j++)
            {
                //Stat randomStat = RandomStat();
                Stat randomStat = StatUtility.RandomStat();
                equipment.stats.Add(randomStat);
                // Determine equipment rating
                equipment.rating += (randomStat.Value * randomStat.GetRatingPerStat());
            }

            // Determine equipment weight - weight is between n and n-max

            // If legendary, add a special stat
        }


    }

    // Uncomment if RarityUtility.RandomRarity is not a good random option
    //// Credit to: https://www.youtube.com/watch?v=Nu-HEbb_z54 for the inspiration
    //private Rarity RandomRarity()
    //{
    //    float totalWeight = 0;
    //    for (int i = 0; i < RarityUtility.probabilities.Length; i++)
    //    {
    //        totalWeight += (int)RarityUtility.probabilities[i];
    //    }

    //    Debug.Log("Total weight: " + totalWeight);

    //    float probability = UnityEngine.Random.Range(0, totalWeight);
    //    float runningTotal = 0;
    //    // Determine which rarity to give
    //    for (int i = 0; i < RarityUtility.probabilities.Length; i++)
    //    {
    //        runningTotal += (int)RarityUtility.probabilities[i];
    //        if (probability < runningTotal)
    //        {
    //            return RarityUtility.rarities[i];
    //        }
    //    }

    //    // If it managed to go over 
    //    return Rarity.Common;
    //}

    // Uncomment if StatUtility.RandomStat is not a good random option
    //private Stat RandomStat()
    //{
    //    int randomClass = UnityEngine.Random.Range(0, StatUtility.statTypes.Length);
    //    if (randomClass == 0)
    //    {
    //        return new HP(UnityEngine.Random.Range(StatUtility.hpRanges[0], StatUtility.hpRanges[1]));
    //    }
    //    else if (randomClass == 1)
    //    {
    //        return new EN(UnityEngine.Random.Range(StatUtility.enRanges[0], StatUtility.enRanges[1]));
    //    }
    //    else if (randomClass == 2)
    //    {
    //        return new ATK(UnityEngine.Random.Range(StatUtility.atkRanges[0], StatUtility.atkRanges[1]));
    //    }
    //    else if (randomClass == 3)
    //    {
    //        return new DEF(UnityEngine.Random.Range(StatUtility.defRanges[0], StatUtility.defRanges[1]));
    //    }
    //    else if (randomClass == 4)
    //    {
    //        return new ATKSPD(UnityEngine.Random.Range(StatUtility.atkspdRanges[0], StatUtility.atkspdRanges[1]));
    //    }
    //    else if (randomClass == 5)
    //    {
    //        return new ENRGN(UnityEngine.Random.Range(StatUtility.enrgnRanges[0], StatUtility.enrgnRanges[1]));
    //    }

    //    return null;
    //}
}