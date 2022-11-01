using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * IEnumerator and Animation event concept, tutorial and inspired by:
 * 
 * GameDev.tv Team, Davidson, R., Pettie, G. (2019) ‘Complete C# Unity Game Developer 2D – Glitch Garden’ ***2019 Course. 2021 course provides different learning materials*** 
 * [Course] Available at: https://www.udemy.com/course/unitycourse/
 * 
 */


public class GobliController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] Health enemyHealth;
    [SerializeField] Energy enemyEnergy;
    [SerializeField] Rigidbody2D enemyRB2D;
    [SerializeField] Collider2D enemyCollider2D;
    [Space]
    [Header("Movement properties")]
    [SerializeField] float moveX = 5f;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] float waitTime = 3f;
    [SerializeField] bool hasReachEdge = false;
    [Space]
    [Header("Current Target")]
    [SerializeField] PlayerMovement enemyTarget;
    [SerializeField] BoxCollider2D hitBox;
    [SerializeField] float distance = 2f;
    [SerializeField] float attackDamage = 10f;
    float attackRate = 1.5f;
    [SerializeField] float currentAttackTime = 0f;

    bool isAttacking = false;
    /* States of Gobli
 * 
 * Idle - default
 * Running - when it sees an character
 * Attacking - near the character           -> Uses energy
 * Hurt - attacked by character             
 * Dead - When hurt too much by character
 */

    public bool GetIsAttacking() { return this.isAttacking; }

    // Should have the physics related here
    private void FixedUpdate()
    {
        if (enemyHealth.GetHealth() <= 0) 
        {
            this.enabled = false;
            return; 
        }


        // If there's no target, the Gobli moves around the ground to find the edge and idles for a bit and finds the edge from
        // the other side and repeat
        if (enemyTarget == null)
        {
            // When the Gobli is commiting an attack, during attack animation, it should not immediately go to the patrol/idle state
            // It should wait for the animation to finish - this guard statement should do that
            if (isAttacking) { return; }

            if (!hasReachEdge)
            {
                NoTargetRun();
            }
            else
            {
                Idle();
            }
        }
        else
        {
            // If there is a target:

            /*
             * 1. If gobli is not in range, go to player
             * 2. When attack is ready, attack the player, 
             * 3. When attacking, stop moving and face that current direction
             * 4. After attacking, repeat attack if within range, if not, keep going to player
             * 5. repeat 1-4 
             * 
             */


            // If there is a target, it should stop the patrol and idle states
            StopCoroutine(wait());

            // Increase time to attack
            currentAttackTime += Time.fixedDeltaTime;


            // If we have a target, the gobli goes towards the player until a certain distance
            // And that we are not attacking
            if (Vector2.Distance(this.transform.position, enemyTarget.transform.position) > distance)
            {
                // Don't move when cannot attack
                if (isAttacking) { return; }

                animator.SetBool("isRunning", true);
                // If player is on the left, move towards the left direction
                if (this.transform.position.x > enemyTarget.transform.position.x)
                {
                    enemyRB2D.velocity = new Vector2(-moveX, enemyRB2D.velocity.y);
                    // Flip the Gobli to the left
                    transform.localScale = new Vector3(-0.6f, transform.localScale.y, transform.localScale.y);
                    isFacingRight = false;
                }
                else if (this.transform.position.x < enemyTarget.transform.position.x)
                {
                    // If player is on the right, move Gobli towards the right direction
                    enemyRB2D.velocity = new Vector2(moveX, enemyRB2D.velocity.y);
                    transform.localScale = new Vector3(0.6f, transform.localScale.y, transform.localScale.y);
                    isFacingRight = true;
                }
            }
            else
            {
                Debug.Log("Now need to wait and attack player");

                // If gobli is NOW in attacking distance

                // Go idle state first
                animator.SetBool("isRunning", false);

                // Then if we can now attack at the given time interval, then attack
                if (currentAttackTime >= attackRate && !isAttacking)
                {
                    isAttacking = true;

                    LookAtPlayer();
                    // Then attack
                    animator.SetTrigger("isAttacking");

                    // Reset time to create an "attack rate"
                    currentAttackTime = 0f;

                }
            }
        }

    }

    private void LookAtPlayer()
    {
        if (this.transform.position.x > enemyTarget.transform.position.x)
        {
            transform.localScale = new Vector3(-0.6f, transform.localScale.y, transform.localScale.y);
            isFacingRight = false;
        }
        else if (this.transform.position.x < enemyTarget.transform.position.x)
        {
            transform.localScale = new Vector3(0.6f, transform.localScale.y, transform.localScale.y);
            isFacingRight = true;
        }
    }

    private void NoTargetRun()
    {
        // When we have no target and we are touching the ground, move left and right
        if (enemyCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && enemyTarget == null && hasReachEdge == false)
        {
            animator.SetBool("isRunning", true);
            if (isFacingRight)
            {
                enemyRB2D.velocity = new Vector2(moveX, enemyRB2D.velocity.y); 
            }
            else
            {
                enemyRB2D.velocity = new Vector2(-moveX, enemyRB2D.velocity.y);
            }
        }
    }

    private void Idle()
    {
        animator.SetBool("isRunning", false);
        // Do nothing to its velocity as the methods of running stops
    }

    // This is a small capsule collider 2d that will trigger the gobli to move left or right, the trigger is placed at its feet or lower and when it touches the edge, flip the facingRight
    // This onTrigger exit should only collide with the ground
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Should add the onTrigger Exit to check if the exit trigger is the ground
        // If the layer of what we collided on exit is "Ground" -> WE WAIT, otherwise continue forward
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            // Debug.Log("Gobli found the edge!!!");
            StartCoroutine(wait());
        }

        
    }

    #region Called in the Animation tab's Animation Event in Unity Editor

    private void SetReadyToAttackTrue() { this.isAttacking = true; }
    private void SetReadyToAttackFalse() { this.isAttacking = false; }

    #endregion

    // Called from the Animation tab from the Gobli_Attack animation - Animation Event
    private void Attack()
    {
        // We know at this point: the Character is within our attacking distance, so we can just attack by deducting the player's health
        // Debug.Log("I am attacking the character");
        Health characterHealth = null;
        bool touchingCharacter = false;

        try
        {
            characterHealth = enemyTarget.GetComponent<Health>();
            touchingCharacter = hitBox.IsTouching(enemyTarget.GetComponentInChildren<CapsuleCollider2D>());
        }
        catch (NullReferenceException e)
        {
            Debug.LogError("Error: " + e.Message + "\nNo character health found - either character moved away from Gobli's hitbox or character used dodge");
        }
        

        //Debug.Log(enemyTarget.GetComponentInChildren<CapsuleCollider2D>());
        /*bool touchingCharacter = hitBox.IsTouching(enemyTarget.GetComponentInChildren<CapsuleCollider2D>());*/
        // Debug.Log("Attacking within hitBox?: " + touchingCharacter);
        if (touchingCharacter)
        {
            characterHealth.TakeDamage(attackDamage);
        }
        
    
    }

    // Concept of Coroutine inspired by... <add reference>
    // Coroutine to start timer - should only be used in a method that calls once, not on every frame - so used in OnTrigger
    IEnumerator wait()
    {
        // Stops Gobli from running over the edge
        hasReachEdge = true;

        yield return new WaitForSeconds(waitTime);
        // Waits for n seconds before code below runs
        // Debug.Log("Waiting finished... ");

        // Flips the facingRight to false or true
        isFacingRight = !isFacingRight;
        // Set scale to x = either - or +
        this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        hasReachEdge = false;
    }

    public void SetEnemyTarget(PlayerMovement target)
    {
        this.enemyTarget = target;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile") && collision.gameObject.tag == "Player")
        {
            
            // Debug.Log("Hit by: " + collision.gameObject.name);
            ArrowProjectile arrowProjectile = collision.gameObject.GetComponent<ArrowProjectile>();
            enemyHealth.TakeDamage(arrowProjectile.GetBowDamage());
        }
    }
}
