using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Animation Event tutorial and inspired by:
 * GameDev.tv Team, Davidson, R., Pettie, G. (2019) 'Complete C# Unity Game Developer 2D - Glitch Garden' ***2019 Course. 2021 course version provides different learning materials*** [Course]
 * Available at: https://www.udemy.com/course/unitycourse/
 * 
 */

/// <summary>
/// Contains all attack behaviours i.e., normal grounded attacks, midair attacks and bow attacks. 
/// Also contain animation events which are called within the Animator's animation event tab.
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [Header("My normal attack")]
    [SerializeField] private int attackCurrentCounter = attackStartingCounter;
    [SerializeField] float attackEnergy = 5f;                                               // How much the attack cost.
    [SerializeField] float myDamage = 5f;                                                   // How much damage can the player deal to the target.
    private const float midAirAttackInterval = 0.35f;                                       // The attack rate while mid air.
    private const int attackStartingCounter = 1;
    [SerializeField] Energy myEnergy;
    [SerializeField] Health myHealth;
    [SerializeField] BoxCollider2D myHitBox;                                                // The player's hitbox which is a box collider. The first enemy gameobject within it gets registered as the target.
    [Space]
    [Header("My Target")]
    [SerializeField] Health myTarget;
    [Space]
    [Header("Sound")]
    [SerializeField] AudioSource audioSource;                                               // The audio player.
    [SerializeField] AudioClip[] audioClips;                                                // Define this in the Editor.
    [Space]
    [Header("My special attack")]
    [SerializeField] GameObject arrowProjectile;
    [SerializeField] private bool hasBow = false;
    [SerializeField] private float bowDamage = 0f;
    [SerializeField] private float bowEnergy = 20f;


    #region Getter and Setters

    public void SetBowDamage(float bowDamage) { this.bowDamage = bowDamage; }
    public float GetMyBowDamage() { return this.bowDamage; }
    public void SetMyDamage(float damage) { this.myDamage = damage; }
    public float GetMyDamage() { return myDamage; }
    public void SetHasBow(bool status) { this.hasBow = status; }
    public void SetMyTarget(Health target) { this.myTarget = target; }
    public BoxCollider2D GetMyHitBox() { return this.myHitBox; }

    #endregion

    /// <summary>
    /// Change the player's current animation to Bow attack animation which launches an arrow projectile and decreases the player's energy.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="myEnergy"></param>
    public void BowAttack(Animator animator, Energy myEnergy)
    {
        // Only able to do so when player pick up has a bow picked up.
        if (!hasBow) { return; }

        // Display the bow attack animation and in that also decreases the user's energy.
        animator.SetTrigger("isBowAttacking");
    }

    /// <summary>
    /// Change the player's current animation to an attack animation, which uses energy. If the player is mid air while attacking, it changes to mid air attack animations. 
    /// Otherwise, grounded attack animations are shown.
    /// </summary>
    /// <param name="myRB2D"></param>
    /// <param name="animator"></param>
    /// <param name="myEnergy"></param>
    /// <param name="isMidAir"></param>
    /// <param name="playerController"></param>
    public void Attack(Rigidbody2D myRB2D, Animator animator, Energy myEnergy, bool isMidAir, PlayerController playerController)
    {
        // Display attack animation for mid air attacks.
        if (isMidAir)
        {
            StartCoroutine(MidAirAttack(myRB2D, animator, myEnergy, playerController));
        }
        // Display grounded attack animations.
        else
        {
            GroundedAttack(animator, myEnergy);
        }
    }

    IEnumerator MidAirAttack(Rigidbody2D myRB2D, Animator animator, Energy myEnergy, PlayerController playerController)
    {
        Debug.Log("I am attacking mid air");
        // Change to mid air attack animation and add an minor jump boost while doing so.
        animator.SetTrigger("isAttack");
        myRB2D.velocity = new Vector2(0f, 3f);
        myEnergy.UseEnergy(attackEnergy);

        // Wait for the given time to finish before executing below.
        yield return new WaitForSeconds(midAirAttackInterval);

        myRB2D.gravityScale = 1f;
        // At the end of 1 second set isUsingActionAnim to false.
        playerController.SetIsUsingActionAnim(false);
    }

    private void GroundedAttack(Animator animator, Energy myEnergy)
    {
        // Reset the counter when we use the final and 3rd attack animation to start using the first attack animation again.
        if (attackCurrentCounter > 3)
            attackCurrentCounter = attackStartingCounter;

        animator.SetTrigger("isAttacking" + attackCurrentCounter.ToString());
        myEnergy.UseEnergy(attackEnergy);

        if (!audioSource.isPlaying && attackCurrentCounter <= 2)
        {
            audioSource.PlayOneShot(audioClips[0]);
        }
        else if (!audioSource.isPlaying && attackCurrentCounter == 3)
        {
            audioSource.PlayOneShot(audioClips[1]);
        }
        // Increase attack counter to show different attack animations.
        attackCurrentCounter++;
    }

    #region Called in the Animator's Animation Event - specifically the sword attack and bow attack animations

    // Called in the animation tab - from Character's animation.
    private void AnimationEventAttack_Bow()
    {
        // Spawn the projectile
        GameObject newArrowProjectile = Instantiate(arrowProjectile, transform.position, Quaternion.identity);
        ArrowProjectile  aP = newArrowProjectile.GetComponent<ArrowProjectile>();                               // Refactoring to optimize frame rate - https://learn.unity.com/tutorial/fixing-performance-problems?courseId=5c87de35edbc2a091bdae346&uv=5.x#5c7f8528edbc2a002053b594
        aP.SetBowDamage(bowDamage);
        aP.SetPlayerScale(this.transform.localScale);

        // Add an SFX for releasing the arrow
        audioSource.PlayOneShot(audioClips[2]);
        // USe energy when we successfully launched a projectile
        myEnergy.UseEnergy(bowEnergy);
    }

    // Called in the animation tab - from Character's animation.
    private void AnimationEventAttack()
    {
        // When we press the attack button, the animation event activates this method
        // At this point we should have an enemy/target that is in our hitbox
        // If so, then attack
        if (myTarget == null) { Debug.Log("Attacking the air...");  return; }

        if (myTarget.GetHealth() <= 0)
        {
            myTarget = null;
            return;
        }
        myTarget.TakeDamage(myDamage);
    }

    #endregion


}
