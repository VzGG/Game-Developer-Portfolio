using Oswald.Manager;
using Oswald.Player;
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
    public abstract class Item : MonoBehaviour
    {
        [SerializeField] string _itemName;
        [SerializeField] string itemType;
        [SerializeField] protected int _itemValue;                           // Define these in the inspector
        [SerializeField] protected float _duration = 0f;

        [SerializeField] private Collider2D _collider2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public abstract void Effect(DialogueManager dialogueManager, PlayerController playerController, AudioSource audioSource, AudioClip audioClip);

        public string GetItemName() { return _itemName; }
        public string GetItemType() { return itemType; }
        public int GetItemValue() { return _itemValue; }
        public float GetDuration() { return _duration; }

        public void HideGameObject()
        {
            _collider2D.enabled = false;
            _spriteRenderer.enabled = false;
        }
    }
}