using Oswald.Manager;
using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oswald.Items
{
    public class EnergyPotion : Item
    {
        public override void Effect(DialogueManager dialogueManager, PlayerController playerController, AudioSource audioSource, AudioClip audioClip)
        {
            StartCoroutine(IncreaseRegen(dialogueManager, playerController, audioSource, audioClip, _duration));
        }

        IEnumerator IncreaseRegen(DialogueManager dialogueManager, PlayerController playerController, AudioSource audioSource, AudioClip audioClip, float duration)
        {
            audioSource.PlayOneShot(audioClip);
            HideGameObject();

            Energy energy = playerController.GetEnergy();

            energy.SetEnergyRegen(energy.GetEnergyRegen() + _itemValue);

            yield return new WaitForSeconds(duration);

            energy.SetEnergyRegen(energy.GetEnergyRegen() - _itemValue);

            gameObject.SetActive(false);
        }
    }
}