using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentRandomizer : MonoBehaviour
{
    [SerializeField] private Sprite[] _presetSprites;

    [SerializeField] private Equipment[] spawnedEquipment;

    [SerializeField] private GameObject prefabEquipment;
    void GenerateEquipment()
    {

        int randomEquipmentSpawned = UnityEngine.Random.Range(3, 8);



        for (int i = 0; i < randomEquipmentSpawned; i++)
        {
            GameObject gameObject = Instantiate(prefabEquipment);
            Equipment equipment = gameObject.GetComponent<Equipment>();
            equipment.values = new float[3];
            equipment.values[0] = UnityEngine.Random.Range(1, 1001);
            int randomSprite = UnityEngine.Random.Range(0, _presetSprites.Length);
            equipment.sprite = _presetSprites[randomSprite];

            Equipment.Rarity rarity = (Equipment.Rarity)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Equipment.Rarity)).Length);
            equipment.rarity = rarity;
        }


    }

    private void LoadSprites()
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Sprites/Items");
        _presetSprites = new Sprite[loadedSprites.Length];
        _presetSprites = loadedSprites;
    }

    private void Start()
    {
        LoadSprites();

        GenerateEquipment();
    }
}
