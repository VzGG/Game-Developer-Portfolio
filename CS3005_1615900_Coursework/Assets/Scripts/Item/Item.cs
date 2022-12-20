using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oswald.Items
{
    /// <summary>
    /// This item script is attached to the item gameObject, the player/character can only touch this using the layer matrix
    /// 
    /// This item script contains the name, and what it does
    /// </summary>
    public class Item : MonoBehaviour
    {
        [SerializeField] string itemName;
        [SerializeField] string itemType;
        [SerializeField] int itemValue;

        public string GetItemName() { return itemName; }
        public string GetItemType() { return itemType; }
        public int GetItemValue() { return itemValue; }
    }
}


