using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The normal enemy's behaviour. It makes the enemy patrol, idle, and attack the player if there is a target.
/// </summary>
public class NormalEnemyController : EnemyController
{
    /// <summary>
    /// Should have the physics related here. This is the loop method that can run more than once per frame.
    /// </summary>
    private void FixedUpdate()
    {
        this.EnemyAI();
    }

    /// <summary>
    /// The enemy's AI behaviour. There are 2 states; (1) the enemy patrols and idles around the platform/stage; (2) enemy moves towards the player and attack.
    /// </summary>
    protected override void EnemyAI()
    {
        if (this.health.GetHealth() <= 0)
        {
            this.enabled = false;
            this.SetRB2DMass(1f);
            return;
        }

        // If there is no target, the gobli patrols from edge to edge of the platform. The moment it reaches an edge it idles for a few seconds.
        if (this.target == null)
        {
            PatrolAndIdle();
        }
        else
        {
            AttackPlayer();
        }
    }

    /// <summary>
    /// Patrol and idle when there is no target to attack. It patrol towards each edge of the platform and idles for a few seconds.
    /// </summary>
    private void PatrolAndIdle()
    {
        if (isAttacking) { return; }

        if (!hasReachEdge)
        {
            NoTargetRun("isRunning", true);
        }
        else
        {
            Idle("isRunning", false);
        }
    }

    /// <summary>
    /// The enemy starts moving towards the player and at every intervals it attacks the player when at the attacking distance. 
    /// The enemy can fully commit attacks even if the player moves away the distance or the target is lost somehow.
    /// </summary>
    private void AttackPlayer()
    {
        StopCoroutine(Wait());

        // Increase time to attack.
        currentAttackTime += Time.fixedDeltaTime;

        // Go towards the player first, then attack.
        if (Vector2.Distance(this.transform.position, this.target.transform.position) > distanceToAttack)
        {
            // Don't move when cannot attack.
            if (isAttacking) { return; }

            animator.SetBool("isRunning", true);
            // If player is on the left, move towards the left direction.
            if (this.transform.position.x > this.target.transform.position.x)
            {
                this.rb2D.velocity = new Vector2(-moveX, this.rb2D.velocity.y);
                // Flip the enemy to the left - have to set to a value instead of transform.localscale.x to prevent flipping every frame.
                transform.localScale = new Vector3(-xScale, this.transform.localScale.y, this.transform.localScale.z);
                isFacingRight = false;
            }
            else if (this.transform.position.x < this.target.transform.position.x)
            {
                // If player is on the right, move Gobli towards the right direction.
                this.rb2D.velocity = new Vector2(moveX, this.rb2D.velocity.y);
                transform.localScale = new Vector3(xScale, this.transform.localScale.y, this.transform.localScale.z);
                isFacingRight = true;
            }
        }
        else
        {
            // Go idle state first.
            animator.SetBool("isRunning", false);

            // Then if we can now attack at the given time interval, then attack
            if (currentAttackTime >= attackRate && !isAttacking)
            {
                isAttacking = true;
                LookAtPlayer(this.xScale);
                // Then attack
                animator.SetTrigger("isAttacking");

                // Reset time to create an "attack rate"
                currentAttackTime = 0f;
            }
        }
    }

    /// <summary>
    /// This method is called the moment this script/gameobject attached to has its collider2D stops overlapping with the platform of the level. This results in this enemy reaching the edge of a platform.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StartCoroutine(Wait());
        }
    }

    /// <summary>
    /// Called by the Unity itself when this script's gameobject attached to collides with another gameobject. This method occurs when it collides with the player's arrow projectile.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile") && collision.gameObject.tag == "Player")
        {
            ArrowProjectile arrowProjectile = collision.gameObject.GetComponent<ArrowProjectile>();
            health.TakeDamage(arrowProjectile.GetBowDamage());
        }
    }


}
