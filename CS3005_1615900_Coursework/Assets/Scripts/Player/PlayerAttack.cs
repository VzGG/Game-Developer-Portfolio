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

    private bool isAttacking = false;

    // This player movement script is set in PlayerMovement script
    private PlayerMovement playerMovement;


    public void SetBowDamage(float bowDamage) { this.bowDamage = bowDamage; }
    public float GetMyBowDamage() { return this.bowDamage; }
    public void SetMyDamage(float damage) { this.myDamage = damage; }
    public float GetMyDamage() { return myDamage; }

    public void SetHasBow(bool status) { this.hasBow = status; }

    // Used by the PlayerArriveLocation.cs script to initialise the time managers.
    public void SetMyTimeManager(TimeManager timeManager) { this.timeManager = timeManager; }

    public void SetIsAttackingTrue() { isAttacking = true; }
    public void SetIsAttackingFalse() { isAttacking = false; }

    public bool GetIsAttacking() { return this.isAttacking; }
    // To set the reference, called in PlayerMovement
    public void SetPlayerMovement(PlayerMovement playerMovement) { this.playerMovement = playerMovement; }
 
    // Update is called once per frame
    void Update()
    {
        if (myHealth.GetHealth() <= 0) { return; }      // Stops attacking when we don't have health
        if (timeManager == null) { return; }            // STOP null reference error
        if (timeManager.GetIsTimeStopped() == true) { 
            // Debug.Log("Attempting to attack in a pause state."); 
            return; }

        if (this.isAttacking) { return; }
        // Cannot attack when we are sliding. Cannot also have animation buffers.
        if (playerMovement.GetIsSliding()) { return; }

        Attack();
        BowAttack();
    }

    private void BowAttack()
    {
        if (!hasBow) { return; }

        if (myEnergy.GetEnergy() > 0)
        {
            // Right mouse click
            if (Input.GetMouseButtonDown(1))
            {
                // Play animation of bow attack
                animator.SetTrigger("isBowAttacking");

                // Upon the final animation of bow attack - release the projectile -> this is the ANimationEventAttack_Bow()
                // and reduce energy by n amount
            }
        }

        // Only able to do so when player pick up has a bow picked up
    }

    // TO-DO: at a certain frame in the animation tab, we need an animation event where if it hits while in animation then we damage them, also push back
    private void Attack()
    {
        if (myEnergy.GetEnergy() > 0)
        {
            // 0 = left click mouse, 1 = right
            if (Input.GetMouseButtonDown(0))
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
        //Debug.Log("attack hitbox: " + collision.gameObject.name);
        // Correctly getting the box collider of the skeleton's body NOT ITS HITBOX and others
        //Debug.Log("This hitbox touching: " + collision.gameObject.GetComponent<BoxCollider2D>().gameObject.name);

        // Debug.Log("My hitbox colliding with enemy's body?: " + myHitBox.IsTouching(  collision.gameObject.GetComponent<BoxCollider2D>()  ));

        if (collision.tag == "Enemy" && myHitBox.IsTouching(collision.gameObject.GetComponent<BoxCollider2D>()))
        {
            myTarget = collision.gameObject.GetComponent<Health>();
        }


        // Gets the collider2d of the target - Gobli or any enemies
/*        if (collision.tag == "Enemy")
        {
            myTarget = collision.gameObject.GetComponent<Health>();
        }*/
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Triggering exit by: " + collision.tag);
        // For some reason untagged is also being triggered as its trigger exit

        // Debug.Log("Trigger Exit of hitbox and enemy's box collider: " + myHitBox.IsTouching(collision.gameObject.GetComponent<BoxCollider2D>()));

        // Gets the collider2d of the target - Gobli or any enemies
/*        if (collision.tag == "Enemy")
        {
            myTarget = null;
        }*/

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
