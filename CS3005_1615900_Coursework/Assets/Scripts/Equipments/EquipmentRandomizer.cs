using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentRandomizer : MonoBehaviour
{
    [SerializeField] private Sprite[] _presetSprites;
    [SerializeField] private Equipment[] _spawnedEquipment;
    [SerializeField] private GameObject _prefabEquipment;

    [SerializeField] bool isThereLegendary = false;
    [SerializeField] GameObject legendaryEquipmentObj;

    private void Start()
    {
        //LoadSprites();
        //GenerateEquipment();
        //AddEquipmentToPlayer();


    }
    public void AddEquipmentToPlayer()
    {
        

        for (int i = 0; i < 5; i++)
        {
            int randomEquipmentIndex = UnityEngine.Random.Range(0, _spawnedEquipment.Length);
            FindObjectOfType<MyEquipment>().myEquipment.Add(_spawnedEquipment[randomEquipmentIndex]);
        }

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (isThereLegendary)
            {
                // Add this inside the PlayerController;
                //FindObjectOfType<Oswald.Player.PlayerController>().characterEquipment = legendaryEquipmentObj;

                isThereLegendary = false;
            }
        }

    }

    public void AddLegendaryEquipment(MyEquipment equipment)
    {
        if (!isThereLegendary) { return; }
        equipment.myEquipment[4] = legendaryEquipmentObj.GetComponent<Equipment>();
    }

    public void LoadSprites()
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Sprites/Items");
        _presetSprites = new Sprite[loadedSprites.Length];
        _presetSprites = loadedSprites;
    }
    public void GenerateEquipment()
    {
        // These are the legendary types, use reflection to create an instance of each type.
        Type[] legendaryTypes = new Type[] 
        {
            typeof(SPECIAL_HELMET_01),
            typeof(SPECIAL_GLOVE_01),
            typeof(SPECIAL_BOOTS_01),
            typeof(SPECIAL_PLATE_01)
        };
        

        int randomEquipmentSpawned = UnityEngine.Random.Range(10, 100);

        _spawnedEquipment = new Equipment[randomEquipmentSpawned];

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
                if (j == ((int)Rarity.Legendary-1))
                {
                    int randomClassIndex = UnityEngine.Random.Range(0, legendaryTypes.Length);
                    Stat legendaryStat = (Stat)Activator.CreateInstance(legendaryTypes[randomClassIndex]);
                    equipment.stats.Add(legendaryStat);
                    equipment.rating += legendaryStat.Value * legendaryStat.GetRatingPerStat() * (1f + legendaryStat.GetRatingPerStatBonus());

                    // DELETE BELOW LATER -> this is only used to attach a legendary to the player.
                    isThereLegendary = true;
                    legendaryEquipmentObj = gameObject;
                    //return;
                }
                else
                {
                    //Stat randomStat = RandomStat();
                    Stat randomStat = StatUtility.RandomStat(j);
                    equipment.stats.Add(randomStat);
                    // Determine equipment rating
                    equipment.rating += randomStat.Value * randomStat.GetRatingPerStat() * (1f + randomStat.GetRatingPerStatBonus());
                }



            }

            _spawnedEquipment[i] = equipment;

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