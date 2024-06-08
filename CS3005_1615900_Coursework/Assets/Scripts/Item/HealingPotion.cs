using Oswald.Manager;
using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oswald.Items
{
    public class HealingPotion : Item
    {
        public override void Effect(DialogueManager dialogueManager, PlayerController playerController, AudioSource audioSource, AudioClip audioClip)
        {
            StartCoroutine(IncreaseRegen(dialogueManager, playerController, audioSource, audioClip, _duration));
        }

        IEnumerator IncreaseRegen(DialogueManager dialogueManager, PlayerController playerController, AudioSource audioSource, AudioClip audioClip, float duration)
        {
            audioSource.PlayOneShot(audioClip);
            HideGameObject();

            playerController.GetHealth().HealthRegen += _itemValue;

            yield return new WaitForSeconds(duration);

            playerController.GetHealth().HealthRegen -= _itemValue;

            gameObject.SetActive(false);
        }
    }
}