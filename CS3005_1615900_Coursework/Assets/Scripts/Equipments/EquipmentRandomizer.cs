using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentRandomizer : MonoBehaviour
{
    [SerializeField] private Sprite[] helmetSprites;
    [SerializeField] private Sprite[] plateSprites;
    [SerializeField] private Sprite[] gloveSprites;
    [SerializeField] private Sprite[] bootSprites;
    [SerializeField] private Sprite[] accessorySprites;


    [SerializeField] private Sprite[] _presetSprites;
    [SerializeField] private Equipment[] _spawnedEquipment;
    [SerializeField] private GameObject _prefabEquipment;

    [SerializeField] bool isThereLegendary = false;
    [SerializeField] GameObject legendaryEquipmentObj;

    // These are the legendary types, use reflection to create an instance of each type.
    Type[] legendaryTypes = new Type[]
    {
            typeof(SPECIAL_HELMET_01),
            typeof(SPECIAL_GLOVE_01),
            typeof(SPECIAL_BOOTS_01),
            typeof(SPECIAL_PLATE_01),
            typeof(SPECIAL_ACCESSORY_01)
    };

    public void LoadSprites()
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Sprites/Items");
        _presetSprites = new Sprite[loadedSprites.Length];
        _presetSprites = loadedSprites;
    }

    private Sprite RandomSprite(EquipmentCategory category, Rarity rarity)
    {
        Sprite randomSprite = null;
        if (category == EquipmentCategory.Helmet)
        {
            randomSprite = RandomSpriteHelper(helmetSprites, rarity);
        }
        else if (category == EquipmentCategory.Plate)
        {
            randomSprite = RandomSpriteHelper(plateSprites, rarity);
        }
        else if (category == EquipmentCategory.Gloves)
        {
            randomSprite = RandomSpriteHelper(gloveSprites, rarity);
        }
        else if (category == EquipmentCategory.Boots)
        {
            randomSprite = RandomSpriteHelper(bootSprites, rarity);
        }
        else if (category == EquipmentCategory.Accessory)
        {
            randomSprite = RandomSpriteHelper(accessorySprites, rarity);
        }

        return randomSprite;
    }

    private Sprite RandomSpriteHelper(Sprite[] spriteCategory, Rarity rarity)
    {
        if (rarity == Rarity.Legendary)
        {
            // We place the legendary sprite at the end of the list
            return spriteCategory[spriteCategory.Length - 1];
        }

        // Do not include the legendary sprite from the random selection

        // exclusive = 4 is not included therefore 3 is max here
        // 0, 1, 2, 3
        // exclusive = 3 is not included ... 2 is maxa here
        int randomIndex = UnityEngine.Random.Range(0, spriteCategory.Length - 1);
        return spriteCategory[randomIndex];
    }

    /// <summary>
    /// Should be called when chest is opened but for now call it when an enemy is defeated.
    /// </summary>
    /// <param name="spawnPosition"></param>
    public void GenerateEquipment(Vector2 spawnPosition)
    {
        //int randomEquipmentSpawned = UnityEngine.Random.Range(10, 100);

        // 1 for now -> each enemy is guranteed to spawn an item for now
        int randomEquipmentSpawned = 1;

        _spawnedEquipment = new Equipment[randomEquipmentSpawned];

        // Spawn a number of equipment gameobject with random rarity.
        for (int i = 0; i < randomEquipmentSpawned; i++)
        {
            GameObject gameObject = Instantiate(_prefabEquipment);
            Equipment equipment = gameObject.GetComponent<Equipment>();

   
            equipment.category = EquipmentCategoryUtility.RandomEquipmentCategory();
            equipment.rarity = RarityUtility.RandomRarity();


 
            equipment.sprite = RandomSprite(equipment.category, equipment.rarity);
            equipment.SetSpriteRendererSprite();
            //equipment.RandomPosition();
            // Set the position on the enemy for now
            equipment.SetPosition(spawnPosition);

            // Add random n random stats - depending on rarity, Common = add 1 random stat, legendary = add 4 random stat + 1 legendary stat/effect
            for (int j = 0; j < (int)equipment.rarity; j++)
            {
                // For legendary stat

                //

                // NEED ONE OF THIS FOR EPIC!
                    // For epic, provide a percentage based bonus instead of flat ones!

                //

                if (j == ((int)Rarity.Legendary-1))
                {
                    int randomClassIndex = UnityEngine.Random.Range(0, legendaryTypes.Length);
                    Stat legendaryStat = (Stat)Activator.CreateInstance(legendaryTypes[randomClassIndex]);
                    equipment.stats.Add(legendaryStat);
                    equipment.rating += legendaryStat.Value * legendaryStat.GetRatingPerStat() * (1f + legendaryStat.GetRatingPerStatBonus());

                    // Make sure to set the category and sprite correctly - overwrite the sprite when it is a legendary category
                    equipment.category = EquipmentCategoryUtility.StatToCategory(legendaryStat);
                    equipment.sprite = RandomSprite(equipment.category, equipment.rarity);
                    equipment.SetSpriteRendererSprite();
                }
                else
                {
                    Stat randomStat = StatUtility.RandomStat(j);
                    equipment.stats.Add(randomStat);
                    // Determine equipment rating
                    equipment.rating += randomStat.Value * randomStat.GetRatingPerStat() * (1f + randomStat.GetRatingPerStatBonus());
                }
            }

            gameObject.name = $"{equipment.rarity.ToString().ToUpper()} {equipment.category.ToString().ToUpper()}";

            _spawnedEquipment[i] = equipment;

            // Determine equipment weight - weight is between n and n-max

            // If legendary, add a special stat

            // To do: make categories have their own common stats found often
                    // Example: Helmet category has more chance of landing HP, EN, DEF
                    // Example: Plate category has more chance of landing DEF
                    // Example: Glove cateogry has more chance of landing ATK SPD, ATK 
                    // Example: Boots has more chance of landing EN RGN, ATK
                    // Example: Accessory has more chance of landing EN, HP, ATK
                    // ATK stat is more landed on Gloves and accesory.

            // To-do: spawn them from a box or enemy!
                // Make a item selector, and spawn that when we select that item
        }


    }
}