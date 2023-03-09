using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public Sprite sprite;
    public Rarity rarity;
    public int weight;
    public float[] values;
    // How to make paragraphs shown in the editor: https://answers.unity.com/questions/424874/showing-a-textarea-field-for-a-string-variable-in.html
    [TextArea(1, 30)]
    public string description;

}
