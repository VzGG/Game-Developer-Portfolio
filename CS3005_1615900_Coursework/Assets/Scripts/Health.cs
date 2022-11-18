using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reused by all living objects i.e., player, enemies,etc.
/// 
/// Not used by Boss
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Health Properties")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 100f;
    [Header("Animator Properties")]
    [SerializeField] Animator animator;
    [SerializeField] private string animationParameterName;
    [SerializeField] bool isHurt = false;
    [SerializeField] string animationParameterName2;            // This should be isRunning for both Player and Gobli or any enemies
    [SerializeField] float isHurtDuration = 0.5f;
    [SerializeField] bool hasFlinch = false;                    // Player does not have flinch animation, but all have flinch
    [SerializeField] RuntimeAnimatorController animatorController;
    [SerializeField] GameObject BloodVFX;
    [Space]
    [Header("Sound Properties")]
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip deathSFX;
    [Space]
    [Header("Player-VFX-ONLY")]
    [SerializeField] SpriteRenderer playerSpriteRenderer;
    [SerializeField] Color originalColor;
    [SerializeField] Color hurtColor;
    [SerializeField] float hurtDuration = 0.5f;
    [Space]
    [Header("EVIL WIZARD ONLY")]
    [SerializeField] bool isEvilWizard = false;

    [Space]
    [Header("BOSS ONLY")]
    [SerializeField] bool isBoss = false;

    public bool isHitDuringEnrage = false;

    [Space]
    [SerializeField] EnemyController enemyController;           // Set in the editor


    public void TakeDamage(float damage)
    {
        this.health = Mathf.Max(this.health - damage, 0f);
    }



    // This is called only once whenever it takes damage, not like it is called in an update
    //public void TakeDamage(float damage) 
    //{ 

    //    // Boss hurt vfx
    //    if (this.gameObject.tag == "Enemy" && isBoss)
    //    {
    //        StartCoroutine(HurtVFX());
    //    }


    //    // This means the take damage method is called when the boss is hit during ENRAGED - OUR CONDITION to stop casting the spell of the boss
    //    // If its the boss and is not in normal phase - normal phase = true is default
    //    if (isBoss && !isNormalPhase)
    //    {
    //        // Debug.Log("BOSS IS HIT DURING ENRAGED - RETURN TO normal phase");
    //        isHitDuringEnrage = true;

    //    }

    //    // Put it below to stop calling this when health is zero or less - should only be called if its a boss
    //    BossPhase(damage);

 

    //}

    #region Boss phase starter

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


    public IEnumerator HurtVFX()
    {
        
        // Change colour to hurt colour
        playerSpriteRenderer.color = hurtColor;
        // Wait n seconds
        yield return new WaitForSeconds(hurtDuration);
        // Change colour back to original colour
        playerSpriteRenderer.color = originalColor;

        //Debug.Log("IS HURT: " + this.gameObject.name);

    }

    public IEnumerator WaitingToDie()
    {
        yield return new WaitForSeconds(3f);
        FindObjectOfType<LevelsManager>().LoadScene("Game Over");
        Destroy(gameObject);
    }


    public void SetHealth(float health) { this.health = health; }

    // Getter
    public float GetHealth() { return this.health; }
    public float GetHealthPercentage() { return health / maxHealth; }

}
