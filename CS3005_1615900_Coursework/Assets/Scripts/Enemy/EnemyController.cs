using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oswald.Player;

namespace Oswald.Enemy
{
    /// <summary>
    /// The base abstract class for all enemies. It should have all the base function of an enemy, and only in the child classes, if needed, should have their own functions/methods that is unique to them.
    /// Otherwise, have them all in here.
    /// </summary>
    public abstract class EnemyController : MonoBehaviour
    {
        //[SerializeField] protected Canvas damageNumberCanvas;
        [SerializeField] protected Animator anim;
        //[SerializeField] protected RuntimeAnimatorController deadAnimController;
        [SerializeField] protected AnimatorController animatorController;
        [SerializeField] protected Health health;
        [SerializeField] protected Break _break;
        [SerializeField] protected Rigidbody2D rb2D;
        [SerializeField] protected Collider2D coll2D;                   // This is the box collider2d of the enemy which is the enemy's actual body collider.
        [Space]
        [Header("Movement Properties")]
        [SerializeField] protected float moveX = 5f;
        [SerializeField] protected bool isFacingRight = true;
        [SerializeField] protected float waitTime = 3f;
        [SerializeField] protected bool hasReachEdge = false;
        [Space]
        [Header("Current Target")]
        /*[SerializeField] protected PlayerController target;  */           // The target that is needed to be attacked.
        [SerializeField] protected Target target;
        [SerializeField] protected BoxCollider2D hitBox;                // The hitbox collider used to check when there is a target during an attack animation.
        [SerializeField] protected float distanceToAttack = 2f;         // It is the distance needed before it starts to attack.
        [SerializeField] protected float distanceToAttackMin = 1f;
        [SerializeField] protected float distanceToAttackMax = 2f;
        [SerializeField] protected float attackDamage = 10f;            // How strong they damage the player.
        [SerializeField] protected float attackRate = 1.5f;             // How fast the enemy attacks.
        [SerializeField] protected float currentAttackTime = 0f;        // Attack time counter used to as a timer for attacking at every intervals.
        [SerializeField] protected bool isAttacking = false;            // The attack status of the enemy, used to fully commit attacks even when it is hurt (hurt animation triggering).
        [SerializeField] protected float xScale = 0f;                   // Define this in the Editor, define differently for Gobli (0.6f), Skeleton (1f) and Boss.
        [SerializeField] protected float yScale = 0f;                   // Define this in the Editor

        [SerializeField] protected EnemyMovement EnemyMovement;

        [SerializeField] protected DamageNumbers damageNumbers;

        //public bool GetIsAttacking() { return this.isAttacking; }
        //public void SetTarget(PlayerController target) { this.target = target; }

        #region Called in Animation tab's Animation Event in the Unity Editor

        /// <summary>
        /// Cannot pass bool parameter in the animation tab's animation event in Unity Editor. It can pass other parameters i.e., int, float, string, etc.
        /// </summary>
        protected void SetReadyToAttackTrue() { this.isAttacking = true; }
        protected void SetReadyToAttackFalse() { this.isAttacking = false; }

        #endregion

        /// <summary>
        /// The enemy AI/behaviour which should be defined in each child classes.
        /// It is "Protected" so that it is public only in itself and its children.
        /// </summary>
        protected abstract void EnemyAI();
        public abstract void EnemyTakeDamage(float damage, GameObject attacker, Vector2 pushback, bool criticalDamage);

        /// <summary>
        /// Looks at the player when attacking
        /// </summary>
        /// <param name="xScale"></param>
        protected void LookAtPlayer(float xScale, float yScale)
        {
            if (this.transform.position.x > target.GetTargets()[0].transform.position.x)
            {
                transform.localScale = new Vector3(-xScale, yScale, transform.localScale.y);
                isFacingRight = false;
            }
            else if (this.transform.position.x < target.GetTargets()[0].transform.position.x)
            {
                transform.localScale = new Vector3(xScale, yScale, transform.localScale.y);
                isFacingRight = true;
            }
        }

        /// <summary>
        /// When there is no target, it patrols from edge to edge of the platform it is standing on.
        /// </summary>
        /// <param name="animatorParameterName"></param>
        /// <param name="status"></param>
        protected void NoTargetRun(string animatorParameterName, bool status)
        {
            //// When we have no target and we are touching the ground, move left and right
            //if (coll2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && target.GetTargets().Count <= 0 && hasReachEdge == false)
            //{
            //    anim.SetBool(animatorParameterName, status);
            //    if (isFacingRight)
            //    {
            //        rb2D.velocity = new Vector2(moveX, rb2D.velocity.y);
            //    }
            //    else
            //    {
            //        rb2D.velocity = new Vector2(-moveX, rb2D.velocity.y);
            //    }
            //}

            // When we have no target and we are touching the ground, move left and right
            if (coll2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && target.GetTargets().Count <= 0 && hasReachEdge == false)
            {
                if (isFacingRight)
                {
                    EnemyMovement.Move(rb2D, animatorController, 0.75f, "isRunning", xScale);
                }
                else
                {
                    EnemyMovement.Move(rb2D, animatorController, -0.75f, "isRunning",  xScale);
                }

            }
        }

        /// <summary>
        /// Set its animation to idle.
        /// </summary>
        /// <param name="animatorParameterName"></param>
        /// <param name="status"></param>
        protected void Idle(string animatorParameterName, bool status)
        {
            anim.SetBool(animatorParameterName, status);
        }

        /// <summary>
        /// Makes the gobli wait a few seconds, then flips itself from the current facing direction.
        /// Coroutine should only be called once, it should be done in Start method or OnTriggerExit methods.
        /// </summary>
        /// <returns></returns>
        protected IEnumerator Wait()
        {
            //Debug.Log("Starting to idle");
            // Stops Gobli from running over the edge.
            hasReachEdge = true;

            // Waits for n seconds before code below runs.
            yield return new WaitForSeconds(waitTime);

            isFacingRight = !isFacingRight;

            // Flip this gobli's facing direction by changing its x scale; multiply by -1 does this.
            this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            hasReachEdge = false;
        }


        /// <summary>
        /// Called from the Animation tab from the Gobli/Skeleton/Boss animation - Animation Event. At a particular frame of the attack animation, this is called.
        /// </summary>
        protected void Attack()
        {
            // We know at this point: the Character is within our attacking distance, so we can just attack by deducting the player's health
            Health characterHealth = null;
            bool touchingCharacter = false;

            try
            {
                characterHealth = target.GetTargets()[0].GetComponent<Health>();
                touchingCharacter = hitBox.IsTouching(target.GetTargets()[0].GetComponentInChildren<Collider2D>());
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("INTERCEPTED Error: " + e.Message + "\nNo character health found - either character moved away from Gobli's hitbox or character used dodge");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.LogError("INTERCEPTED Error: " + e.Message);
            }

            // At the frame which the enemy does the attack animation AND it is touching the character, the character takes damage
            if (touchingCharacter)
            {
                //characterHealth.TakeDamage(attackDamage);
                characterHealth.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(attackDamage);
            }
        }

    }
}


