using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Animation Event concept, tutorial, and inspired by:
 * 
 * GameDev.tv Team, Davidson, R., Pettie, G. (2019) ‘Complete C# Unity Game Developer 2D – Glitch Garden’ ***2019 Course. 2021 course provides different learning materials*** [Course] 
 * Available at: https://www.udemy.com/course/unitycourse/ 
 */


public class BossSpellProjectile : MonoBehaviour
{
    public float spellDamage = 0f;
    public CapsuleCollider2D targetBody;               // For targeting player
    public Health characterHealth;                     // For targeting player
    public void SetSpellDamage(float spellDamage) { this.spellDamage = spellDamage; }                           // Set by the boss controller when casting a spell animation
    public void SetTargetBody(CapsuleCollider2D capsuleCollider2D) { this.targetBody = capsuleCollider2D; }     // Set by the boss controller when casting a spell animation
    public void SetCharacterHealth(Health health) { this.characterHealth = health; }                            // Set by the boss controller when casting a spell animation

    [SerializeField] BoxCollider2D spellHitBox;

    // Called by animation event

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    void DestroySelf()
    {
        spellHitBox.enabled = false;
        // Destroy its parent and itself
        Destroy(this.gameObject.transform.parent.gameObject, 3f);
    }

    // Called by animation event
    void SpellDamageAttack()
    {
        audioSource.PlayOneShot(audioClip);

        if (spellHitBox.IsTouching(targetBody))
        {
            // Take spell damage
            characterHealth.TakeDamage(spellDamage);
            // Debug.Log("SPELL HIT CHARACTER");
        }

    }
}
