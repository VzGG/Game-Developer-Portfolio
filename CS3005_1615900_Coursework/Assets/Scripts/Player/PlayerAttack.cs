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
    private float jumpAttackLandedBonus = 0f;                                               // After landing with the sword air attack 3, increase the total damage by this. This bonus is increased the longer the player is in mid air and while in sword air attack 3.
    private const float midAirAttackInterval = 0.35f;                                       // The attack rate while mid air.
    private const int attackStartingCounter = 1;
    private const float attackInterval = 0.25f;
    [SerializeField] Energy myEnergy;
    [SerializeField] Health myHealth;
    [SerializeField] BoxCollider2D myHitBox;                                                // The player's hitbox which is a box collider. The first enemy gameobject within it gets registered as the target.
    [Space]
    [Header("My Target")]
    //[SerializeField] Health myTarget;
    [SerializeField] List<Health> myTargets = new List<Health>();
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
    public BoxCollider2D GetMyHitBox() { return this.myHitBox; }

    #endregion

    /// <summary>
    /// Add a target to the list that which can be damaged. There should be no copies of the same target.
    /// </summary>
    /// <param name="health"></param>
    public void AddToMyTargets(Health health)
    {
        if (health.GetHealth() <= 0f) { return; }

        // Cannot add duplicate targets, only unique ones.
        for (int i = 0; i < myTargets.Count; i++)
            if (myTargets[i].gameObject.GetInstanceID() == health.gameObject.GetInstanceID())
                return;

        myTargets.Add(health);
    }

    /// <summary>
    /// Remove a target from the list. Used for removing targets when enemies are outside the player's targer range
    /// or when the target dies.
    /// </summary>
    /// <param name="health"></param>
    public void RemoveToMyTargets(Health health)
    {
        // Remove the unique enemy from the given index.
        for (int i = 0; i < myTargets.Count; i++)
            if (myTargets[i].gameObject.GetInstanceID() == health.gameObject.GetInstanceID())
                myTargets.RemoveAt(i);
    }


    /// <summary>
    /// Change the player's current animation to Bow attack animation which launches an arrow projectile and decreases the player's energy.
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="myEnergy"></param>
    public void BowAttack(Animator animator, Energy myEnergy, bool isMidAir, Rigidbody2D myRB2D, PlayerController playerController, bool isNextAttackBow)
    {
        // Only able to do so when player pick up has a bow picked up.
        if (!hasBow) { return; }

        if (isMidAir)
        {
            if (isNextAttackBow)
            {
                StartCoroutine(MidAirBowAttack(animator, myRB2D, playerController));
            }
        }
        else
        {
            animator.SetTrigger("isBowAttacking");
        }
    }

    IEnumerator MidAirBowAttack(Animator anim, Rigidbody2D myRB2D, PlayerController playerController)
    {
        Debug.Log("I am attacking bow mid air attack");
        // Display bow attack animation.
        anim.SetTrigger("isBowAttacking");
        // Add a jump effect.
        myRB2D.velocity = new Vector2(0f, 3f);


        // Do we still need this IEnumerator?
        // YES - to get the mid air attack interval - need to have cooldown attacks/attack rate

        yield return new WaitForSeconds(midAirAttackInterval);

        //playerController.SetIsUsingActionAnim(false);
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
    public void SwordAttack(Rigidbody2D myRB2D, Animator animator, Energy myEnergy, bool isMidAir, PlayerController playerController, bool isNextAttackSword)
    {
        // Display attack animation for mid air attacks.
        if (isMidAir)
        {
            if (isNextAttackSword)
            {
                StartCoroutine(MidAirAttack(myRB2D, animator, myEnergy, playerController));
            }
                
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
        //playerController.SetIsUsingActionAnim(false);
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
        aP.SetPlayerController(this.GetComponent<PlayerController>());

        // Add an SFX for releasing the arrow
        audioSource.PlayOneShot(audioClips[2]);
        // USe energy when we successfully launched a projectile
        myEnergy.UseEnergy(bowEnergy);
    }

    // Called in the animation tab - from Character's animation. Multiplier is set in the animation tab also.
    private void AnimationEventAttack(float multiplier)
    {
        if (myTargets == null) { Debug.Log("Attacking the air..."); return; }

        // Anyone in the hitbox (they should all be in the list of targets) should get damaged.
        for (int i = 0; i < myTargets.Count; i++)
        {
            myTargets[i].gameObject.GetComponent<EnemyController>().EnemyTakeDamage( (myDamage * multiplier) + jumpAttackLandedBonus, gameObject, new Vector2(0.5f, 3.5f));

            if (myTargets[i].GetHealth() <= 0)
                myTargets.RemoveAt(i);
        }
        
    }
    // Called in the animation tab - the player's sword air attack 3.
    private void IncreaseJump3LandAttackValue()
    {
        jumpAttackLandedBonus += 0.5f;
    }

    // Called in the animation tab - the player's sword air attack 3 landed.
    private void ResetJump3LandAttackValue()
    {
        jumpAttackLandedBonus = 0f;
    }

    // Called in the animation tab - from Character's landed animation attack.
    private void ResizeToLargeHitBox()
    {
        myHitBox.offset = Vector2.zero;
        myHitBox.size = new Vector2(0.5f, myHitBox.size.y);
    }

    // Called in the animation tab - from Character's landed animation attack.
    private void ResizeToSmallHitBox()
    {
        const float offsetX = 0.1804348f;
        const float sizeX = 0.2337848f;
        myHitBox.offset = new Vector2(offsetX, myHitBox.offset.y);
        myHitBox.size = new Vector2(sizeX, myHitBox.size.y);
    }

    #endregion


}
