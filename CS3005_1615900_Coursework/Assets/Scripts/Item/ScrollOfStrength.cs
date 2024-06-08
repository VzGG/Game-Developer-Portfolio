using Oswald.Manager;
using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oswald.Items
{
    public class ScrollOfStrength : Item
    {
        [SerializeField] private int _dialogueNumber = 4;
        public override void Effect(DialogueManager dialogueManager, PlayerController playerController, AudioSource audioSource, AudioClip audioClip)
        {
            StartCoroutine(IncreaseDamage(dialogueManager, playerController, audioSource, audioClip, _duration));
        }
        IEnumerator IncreaseDamage(DialogueManager dialogueManager, PlayerController playerController, AudioSource audioSource, AudioClip audioClip, float duration)
        {
            audioSource.PlayOneShot(audioClip);
            HideGameObject();

            Instantiate(dialogueManager.InstantiateDialogue(_dialogueNumber));

            PlayerAttack playerAttack = playerController.GetComponent<PlayerAttack>();

            playerAttack.SetMyDamage(playerAttack.GetMyDamage() + _itemValue);
            playerAttack.SetBowDamage(playerAttack.GetMyBowDamage() + _itemValue);

            yield return new WaitForSeconds(duration);

            playerAttack.SetMyDamage(playerAttack.GetMyDamage() - _itemValue);
            playerAttack.SetBowDamage(playerAttack.GetMyBowDamage() - _itemValue);

            gameObject.SetActive(false);
        }
    }
}