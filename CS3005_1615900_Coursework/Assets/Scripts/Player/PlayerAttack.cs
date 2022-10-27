using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Animation Event tutorial and inspired by:
 * GameDev.tv Team, Davidson, R., Pettie, G. (2019) 'Complete C# Unity Game Developer 2D - Glitch Garden' ***2019 Course. 2021 course version provides different learning materials*** [Course]
 * Available at: https://www.udemy.com/course/unitycourse/
 * 
 */
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private const int attackStartingCounter = 1;
    [SerializeField] private int attackCurrentCounter = attackStartingCounter;
    [SerializeField] Energy myEnergy;
    [SerializeField] float attackEnergy = 5f;
    [SerializeField] float myDamage = 5f;
    [SerializeField] Health myHealth;
    [Space]
    [Header("My Target")]
    [SerializeField] Health myTarget;
    [Space]
    [Header("Sound")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips; // Define this in the Editor
    [Space]
    [Header("Time manager")]
    [SerializeField] TimeManager timeManager;
    [Space]
    [Header("My special attack")]
    [SerializeField] GameObject arrowProjectile;
    [SerializeField] private bool hasBow = false;
    [SerializeField] private float bowDamage = 0f;
    [SerializeField] private float bowEnergy = 20f;


    public void SetBowDamage(float bowDamage) { this.bowDamage = bowDamage; }
    public float GetMyBowDamage() { return this.bowDamage; }
    public void SetMyDamage(float damage) { this.myDamage = damage; }
    public float GetMyDamage() { return myDamage; }

    public void SetHasBow(bool status) { this.hasBow = status; }

    // Used by the PlayerArriveLocation.cs script to initialise the time managers.
    public void SetMyTimeManager(TimeManager timeManager) { this.timeManager = timeManager; }

 
    public bool BowAttack(Energy myEnergy)
    {
        // Only able to do so when player pick up has a bow picked up
        if (!hasBow) { return false; }

        bool isBowAttacking = false;
        if (myEnergy.GetEnergy() > 0)
        {
            isBowAttacking = Input.GetMouseButtonDown(1);
            // Right mouse click
            if (isBowAttacking)
            {
                // Play animation of bow attack
                animator.SetTrigger("isBowAttacking");

                // Upon the final animation of bow attack - release the projectile -> this is the ANimationEventAttack_Bow()
                // and reduce energy by n amount
            }
        }

        return isBowAttacking;
    }

    public bool Attack(Energy myEnergy)
    {
        bool isAttacking = false;
        if (myEnergy.GetEnergy() > 0)
        {
            isAttacking = Input.GetMouseButtonDown(0);
            // 0 = left click mouse, 1 = right
            if (isAttacking)
            {
                // When we attack, along with stopping the movement script, also stop the player from pressing any buttons
                // doing so stops the player from making the animation being cancelled by repeating the attack animation's first few frames over and over and not running the whole frames

                if (attackCurrentCounter > 3)
                    attackCurrentCounter = attackStartingCounter;   // Reset to the starting counter - starting attack animation

                // If we attack - find the appropriate attack animation  depending on the counter and increment the counter
                // While in the animation of attacking, the player cannot/should not move, in animation editor tab, add events to disable the playerMovement class)
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
                attackCurrentCounter++;



            }
        }

        return isAttacking;
    }

    // Called in the animation tab - from Character's animation
    private void AnimationEventAttack_Bow()
    {
        // Debug.Log("Attacking with bow!!!");

        // Spawn the projectile
        GameObject newArrowProjectile = Instantiate(arrowProjectile, transform.position, Quaternion.identity);
        ArrowProjectile  aP = newArrowProjectile.GetComponent<ArrowProjectile>();                               // Refactoring to optimize frame rate - https://learn.unity.com/tutorial/fixing-performance-problems?courseId=5c87de35edbc2a091bdae346&uv=5.x#5c7f8528edbc2a002053b594
        aP.SetBowDamage(bowDamage);
        aP.SetPlayerScale(this.transform.localScale);


        /*        newArrowProjectile.GetComponent<ArrowProjectile>().SetBowDamage(bowDamage);
                newArrowProjectile.GetComponent<ArrowProjectile>().SetPlayerScale(this.transform.localScale);*/
        // Add an SFX for releasing the arrow
        audioSource.PlayOneShot(audioClips[2]);
        // USe energy
        myEnergy.UseEnergy(bowEnergy);
    }

    // Called in the animation tab - from Character's animation
    private void AnimationEventAttack()
    {
        // When we press the attack button, the animation event activates this method
        // At this point we should have an enemy that is in our hitbox
        // If so, then attack
        
        if (myTarget == null) { Debug.Log("Attacking the air...");  return; }

        if (myTarget.GetHealth() <= 0)
        {
            myTarget = null;
            return;
        }
        myTarget.TakeDamage(myDamage);
        // Debug.Log("Attacking: " + myTarget);
    }

    [SerializeField] BoxCollider2D myHitBox;

    #region Finding An Enemy In Our Character's Hitbox
    // This should call the collider in our character that has OnTrigger to true - should be a small collider just a bit forward on the player's facing direction
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Adds 1 target for us to attack, used for when using any attack animation
        if (collision.tag == "Enemy" && myHitBox.IsTouching(collision.gameObject.GetComponent<BoxCollider2D>()))
        {
            myTarget = collision.gameObject.GetComponent<Health>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // This removes our current target when our hitbox discovers that the enemy leaves our hitbox
        if (collision.tag == "Enemy" && !myHitBox.IsTouching(collision.gameObject.GetComponent<BoxCollider2D>())  )
        {
            myTarget = null;
        }
    }
    #endregion


    #region Enemy Projectile Hitting The player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile") && collision.gameObject.tag == "EnemyProjectile")
        {
            Debug.Log("being hit");
            // Debug.Log("Hit by: " + collision.gameObject.name);
            EnemyArrowProjectile enemyArrowProjectile = collision.gameObject.GetComponent<EnemyArrowProjectile>();
            myHealth.TakeDamage(enemyArrowProjectile.GetDamage());
        }
    }

    #endregion


}
