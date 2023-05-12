using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Enemy;
using Oswald.Manager;
using System;

/// <summary>
/// Reused by all living objects i.e., player, enemies,etc.
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Health Properties")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 100f;

    [Header("VFX Properties")]
    [SerializeField] GameObject BloodVFX;

    [Header("Sound Properties")]
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip deathSFX;

    [Header("Player-VFX-ONLY")]
    [SerializeField] Color originalColor;
    [SerializeField] Color hurtColor;
    [SerializeField] float hurtDuration = 0.5f;

    public bool canRegen { private get; set; } = false;

    private float _healthRegen = 0f;
    public float HealthRegen 
    { 
        get
        {
            return _healthRegen;
        }
        set
        {
            _healthRegen = value;

            if (_healthRegen <= 0f)
                canRegen = false;
            else
                canRegen = true;
        }
    }

    // For evasion
    public bool canEvadeDamage { private get; set; } = false;

    [SerializeField] private float _evasionRate = 0f;
    public float EvasionRate { 
        get
        {
            return _evasionRate;
        }
        set
        {
            _evasionRate = value;

            if (_evasionRate > _evasionRateCap)
            {
                _evasionRate = _evasionRateCap;
            }

            if (_evasionRate <= 0f)
                canEvadeDamage = false;
            else
                canEvadeDamage = true;
        } 
    }
    private float _evasionRateCap = 90f;

    public bool isEvading { get; private set; } = false;
    public Color evadeColor;
    public float evadeDuration = 0.1f;

    public void SetHealth(float health) { this.health = health; }
    public void SetMaxHealth(float maxHealth) { this.maxHealth = maxHealth; }
    public float GetHealth() { return this.health; }
    public float GetMaxHealth() { return this.maxHealth; }
    public float GetHealthPercentage() { return health / maxHealth; }

    public void CanEvadeDamage()
    {
        Debug.Log("Evading allowed?: " + canEvadeDamage + "\nEvasion rate: " + EvasionRate);
        if (!canEvadeDamage) { return; }

        int chanceToEvade = UnityEngine.Random.Range(0, 100);
        if (chanceToEvade < EvasionRate)
        {
            // Do nothing because we evade
            isEvading = true;
        }
    }

    public void ResetEvade()
    {
        isEvading = false;
    }

    public void TakeDamage(float damage)
    {
        this.health = Mathf.Max(this.health - damage, 0f);
    }

    public void HealthRegeneration()
    {
        if (!canRegen) { return; }

        health = Mathf.Clamp(health + HealthRegen * Time.deltaTime, 0f, maxHealth);
    }

    #region Boss phase starter

    [Space]
    [Header("BOSS ONLY")]
    [SerializeField] bool isBoss = false;
    public bool isHitDuringEnrage = false;

    [Space]
    [Header("BOSS Phase ONLY")]
    public float accumulatedDamage = 0f;
    public float accumulatedDamageThreshold = 0f;
    public bool isNormalPhase = true;
    void BossPhase(float damage)
    {
        if (!isBoss) { return; }                            // Don't run when it's not a boss
        if (!isNormalPhase) { return; }                     // Don't accumulate damage when NOT in normal phase
        accumulatedDamage += damage;                        // Start adding the damage to the accumulated damage
        accumulatedDamageThreshold = maxHealth / 4f;        // Set the threshold - 1/4 of max health
        if (accumulatedDamage >= accumulatedDamageThreshold)// Check if we pass the threshold
        {
            // Debug.Log("Boss now in enrage phase!");
            isNormalPhase = false;
            BossController bC = GetComponent<BossController>();
            bC.actualEPP.SetActive(true);        // Set the platform in the air to true
            bC.actualEPPI.SetActive(true);       // Set the platform invisible in the air to true - for anti stuck

            // Set the accumulated damage to 0 WHEN
            // the boss has finished its SPELL ANIMATION

            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; // Stop it from falling
            // On start of enrage phase - disable collider2d THEN enable again on teleport finish
            GetComponent<BoxCollider2D>().enabled = false;
        }

        // Get accumulated damage
        // if accumulated damage is equal to 1/4 or greater than health of boss AND not in spell phase
        // THEN
        // Signal boss controller with bool to tell its time to change phase - animation
        // play teleport animation
        // teleport boss
        // Trigger new attack of boss - spell animation
        // Perform 3-6 of spell animation (Random)
        // Return to main animator controller (walk, attack, idle)
        // Allow to accumulate damage again, and call this method again


    }


    #endregion

    #region VFX and SFX

    public void BloodEffectVFX()
    {
        Instantiate(BloodVFX, transform.position, transform.rotation);
    }

    public void PlayHurtSFX()
    {
        audioSource.PlayOneShot(hurtSFX);
    }

    public void PlayDeadSFX()
    {
        audioSource.PlayOneShot(deathSFX);
    }

    public IEnumerator HurtVFX(SpriteRenderer spriteRenderer)
    {
        // Change colour to hurt colour
        spriteRenderer.color = hurtColor;
        // Wait n seconds
        yield return new WaitForSeconds(hurtDuration);
        // Change colour back to original colour
        spriteRenderer.color = originalColor;
    }

    public IEnumerator EvadeVFX(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = evadeColor;

        yield return new WaitForSeconds(evadeDuration);

        spriteRenderer.color = originalColor;
    }

    #endregion

    public IEnumerator WaitingToDie()
    {
        yield return new WaitForSeconds(3f);
        FindObjectOfType<LevelsManager>().LoadScene("Game Over");
        Destroy(gameObject);
    }
}