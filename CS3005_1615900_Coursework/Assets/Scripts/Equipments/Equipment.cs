using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Equipment : MonoBehaviour, IInteractableEnvironment
{
    public Sprite sprite;
    public EquipmentCategory category;
    public Rarity rarity;
    //public int weight;
    [SerializeField] private BoxCollider2D _boxCollider2DForUI;
    [SerializeField] private GameObject _promptUI;
    [SerializeField] private GameObject _equipmentDetailUI;
    [SerializeField] private Text _statMetaText;
    [SerializeField] private Text _statText;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _ratingText;
    [SerializeField] private Text _descriptionText;

    // To-do: consider making Sound Manager?
    [SerializeField] private AudioClip _audioClip;

    private Color _nameTextColour
    {
        get
        {
            if (rarity == Rarity.Legendary)
                return Color.yellow;
            else if (rarity == Rarity.Epic)
                return Color.red;
            else if (rarity == Rarity.Rare)
                return Color.blue;
            else if (rarity == Rarity.Uncommon)
                return Color.green;
            else
                return Color.white;
        }
    }

    private string _description
    { 
        get
        {
            if (rarity == Rarity.Legendary)
            {
                return stats[4].GetDescription();
            }
            else
            {
                string desc = "";

                if (category == EquipmentCategory.Helmet)
                    desc = "Helps the wearer survive the fights longer.";
                else if (category == EquipmentCategory.Plate)
                    desc = "Makes the wearer receive less lethal attacks.";
                else if (category == EquipmentCategory.Gloves)
                    desc = "Makes the wearer hit faster.";
                else if (category == EquipmentCategory.Boots)
                    desc = "Makes the wearer last the fights longer";
                else if (category == EquipmentCategory.Accessory)
                    desc = "Makes the wearer recover faster.";

                return desc;
            }
        }
    }



    // Fix not showing non-monobehvaiour classes in the inspector: https://forum.unity.com/threads/abstract-class-in-list-only-shows-base-variables.999478/
    [SerializeField][SerializeReference] public List<Stat> stats = new List<Stat>();

    public float rating;
    // How to make paragraphs shown in the editor: https://answers.unity.com/questions/424874/showing-a-textarea-field-for-a-string-variable-in.html

    // public float weight;

    /// <summary>
    /// Set the sprite of the equipment
    /// </summary>
    public void SetSpriteRendererSprite()
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void RandomPosition()
    {
        float randX = UnityEngine.Random.Range(1.5f, 5f);
        float randY = UnityEngine.Random.Range(1.5f, 5f);
        transform.position = new Vector2(randX, randY);
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    private void AddThisEquipment(UnityEngine.Object obj)
    {
        // You can use var for typing less - type var instead of Oswald.Player.PlayerController type
        var playerController = (Oswald.Player.PlayerController)obj;

        MyEquipment playerEquipment = playerController.GetComponent<MyEquipment>();
        playerEquipment.AddEquipment(this, playerController);

        _boxCollider2DForUI.enabled = false;

        // Play SFX
        playerController.GetComponent<AudioSource>().PlayOneShot(_audioClip);
    }

    #region Collision Detection

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ShowInteractionUI();

            // Set player's reference to this IInteractable
            Oswald.Player.PlayerController player = collision.gameObject.GetComponent<Oswald.Player.PlayerController>();
            player.interactableEnvironment = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HideInteractionUI();

            // Remove reference so we cannot use the interaction method
            Oswald.Player.PlayerController player = collision.gameObject.GetComponent<Oswald.Player.PlayerController>();
            player.interactableEnvironment = null;
        }
    }

    #endregion Collision Detection

    #region Interface methods

    public void Interaction(UnityEngine.Object obj)
    {
        AddThisEquipment(obj);
        HideInteractionUI();
    }

    public void ShowInteractionUI()
    {
        // Show the UI
        _promptUI.SetActive(true);
        _equipmentDetailUI.SetActive(true);

        _statMetaText.text = "";
        _statText.text = "";

        foreach (Stat stat in this.stats)
        {
            _statMetaText.text += $"{stat.GetName()}\n";

            _statText.text += $"{stat.Value}\n";
        }

        _nameText.text = $"{this.name}";
        _nameText.color = _nameTextColour;
        _ratingText.text = $"{this.rating}";

        _descriptionText.text = _description;
    }

    public void HideInteractionUI()
    {
        _promptUI.SetActive(false);
        _equipmentDetailUI.SetActive(false);
    }

    #endregion Interface methods
}

public enum EquipmentCategory
{
    Helmet,
    Plate,
    Gloves,
    Boots,
    Accessory
}

public static class EquipmentCategoryUtility
{
    public static readonly EquipmentCategory[] equipmentCategories = (EquipmentCategory[])Enum.GetValues(typeof(EquipmentCategory));

    public static EquipmentCategory RandomEquipmentCategory()
    {
        int randomIndex = UnityEngine.Random.Range(0, equipmentCategories.Length);

        return equipmentCategories[randomIndex];
    }

    public static EquipmentCategory StatToCategory(Stat stat)
    {
        if (stat.GetType() == typeof(SPECIAL_HELMET_01))
        {
            return EquipmentCategory.Helmet;
        }
        else if (stat.GetType() == typeof(SPECIAL_PLATE_01))
        {
            return EquipmentCategory.Plate;
        }
        else if (stat.GetType() == typeof(SPECIAL_GLOVE_01))
            return EquipmentCategory.Gloves;
        else if (stat.GetType() == typeof(SPECIAL_BOOTS_01))
            return EquipmentCategory.Boots;
        else
            return EquipmentCategory.Accessory;
    }
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
        360   // Legendary
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