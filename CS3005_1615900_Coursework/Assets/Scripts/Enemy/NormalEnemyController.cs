using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Oswald.UI;
using Oswald.Manager;
using Oswald.Player;


namespace Oswald.Enemy
{
    /// <summary>
    /// The normal enemy's behaviour. It makes the enemy patrol, idle, and attack the player if there is a target.
    /// </summary>
    public class NormalEnemyController : EnemyController
    {
        protected enum AttackPattern
        {
            Melee_Attacks,
            Range_Attacks,
            Mixed
        }

        [SerializeField] private AttackPattern attackPattern;
        [SerializeField] private EnemyUI enemyUI;
        [SerializeField] CapsuleCollider2D walkEdgeChecker;
        protected enum BehaviourStates
        {
            Idle,
            Move,
            Attack
        }

        [SerializeField] private BehaviourStates currentBehaviourState;

        [SerializeField] private bool _isHit = false;

        private void Awake()
        {
            target = GetComponent<Target>();
            // Randomize the distance to attack so every enemy has their own range
            distanceToAttack = UnityEngine.Random.Range(distanceToAttackMin, distanceToAttackMax);
        }

        /// <summary>
        /// Should have the physics related here. This is the loop method that can run more than once per frame.
        /// </summary>
        private void FixedUpdate()
        {
            //Debug.Log("Enemy xy scale: " + transform.localScale);
            this.enemyUI.UpdatePosition(this.transform.position);
            this.EnemyAI();
        }


        [SerializeField] float time = 0f;
        [SerializeField] bool isStartWait = false;

        /// <summary>
        /// The enemy's AI behaviour. There are 2 states; (1) the enemy patrols and idles around the platform/stage; (2) enemy moves towards the player and attack.
        /// </summary>
        protected override void EnemyAI()
        {
            if (this.health.GetHealth() <= 0) { return; }


            // When hit during break effect, the enemy cannot do any action.
            if (_isHit && _break.GetIsBreak()) 
            {
                // Set animator to idle
                this.anim.SetBool("isRunning", false);

                return; 
            }



            // When it is mid air, it should go to idle animation only - or hurt animation if hit mid air.
            if (!this.coll2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                //Debug.Log("Enemy is mid air");
                // Go idle anim
                Idle("isRunning", false);

                // Return
                return;
            }


            // If there is no target, the gobli patrols from edge to edge of the platform. The moment it reaches an edge it idles for a few seconds.
            if (this.target.GetTargets().Count <= 0)
            {
                PatrolAndIdle();
            }
            else
            {
                switch (attackPattern)
                {
                    case AttackPattern.Melee_Attacks:
                        MeleeAttackPlayer();
                        break;
                    case AttackPattern.Range_Attacks:
                        RangeAttackPlayer();
                        break;
                    case AttackPattern.Mixed:
                        MixedAttackPlayer();
                        break;
                }
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

        #region Attack Modes - Melee, Range, Mixed

        /// <summary>
        /// The enemy starts moving towards the player and at every intervals it attacks the player when at the attacking distance. 
        /// The enemy can fully commit attacks even if the player moves away the distance or the target is lost somehow.
        /// </summary>
        private void MeleeAttackPlayer()
        {
            StopCoroutine(Wait());

            // Increase time to attack.
            currentAttackTime += Time.fixedDeltaTime;


            // Go towards the player first, then attack.
            //if (Vector2.Distance(this.transform.position, this.target.transform.position) > distanceToAttack)
            if (Vector2.Distance(this.transform.position, this.target.GetTargets()[0].transform.position) > distanceToAttack)
            {
                // Don't move when cannot attack.
                if (isAttacking) { return; }

                anim.SetBool("isRunning", true);
                // If player is on the left, move towards the left direction.
                //if (this.transform.position.x > this.target.transform.position.x)
                if (this.transform.position.x > this.target.GetTargets()[0].transform.position.x)
                {
                    this.rb2D.velocity = new Vector2(-moveX, this.rb2D.velocity.y);
                    // Flip the enemy to the left - have to set to a value instead of transform.localscale.x to prevent flipping every frame.
                    //transform.localScale = new Vector3(-xScale, this.transform.localScale.y, this.transform.localScale.z);
                    transform.localScale = new Vector3(-xScale, yScale, this.transform.localScale.z);
                    isFacingRight = false;
                }
                //else if (this.transform.position.x < this.target.transform.position.x)
                else if (this.transform.position.x < this.target.GetTargets()[0].transform.position.x)
                {
                    // If player is on the right, move Gobli towards the right direction.
                    this.rb2D.velocity = new Vector2(moveX, this.rb2D.velocity.y);
                    //transform.localScale = new Vector3(xScale, this.transform.localScale.y, this.transform.localScale.z);
                    transform.localScale = new Vector3(xScale, yScale, this.transform.localScale.z);
                    isFacingRight = true;
                }
            }
            else
            {
                // Go idle state first.
                anim.SetBool("isRunning", false);

                // Then if we can now attack at the given time interval, then attack
                if (currentAttackTime >= attackRate && !isAttacking)
                {
                    isAttacking = true;
                    //LookAtPlayer(this.xScale);
                    LookAtPlayer(this.xScale, this.yScale);
                    // Then attack
                    anim.SetTrigger("isAttacking");

                    // Reset time to create an "attack rate"
                    currentAttackTime = 0f;
                }
            }
        }

        private void RangeAttackPlayer()
        {
            // Always try to attack at a range.
            // If the player gets near the enemy, attempt to go further away from player.
        }

        private void MixedAttackPlayer()
        {
            // Mixed attacks - can go melee or range.
        }

        #endregion

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
                Debug.Log("Gameobject Tag HELLO: " + arrowProjectile.gameObject.tag);
                //this.EnemyTakeDamage(arrowProjectile.GetBowDamage(), arrowProjectile.gameObject, Vector2.zero);
            }
        }

        #region Taking Damage Logic
        public override void EnemyTakeDamage(float damage, GameObject attacker, Vector2 pushback, bool criticalDamage)
        {
            if (health.GetHealth() <= 0f) { return; }
            if (damage > health.GetHealth())
            {
                damage = health.GetHealth();
            }
            health.TakeDamage(damage);
            health.PlayHurtSFX();

            // Remove all of this coroutine instances and have only one start at all times.
            StopCoroutine("ChangeIsHit");
            StartCoroutine("ChangeIsHit");


            // Displays the UI for taking damage.
            damageNumbers.IsCritical = criticalDamage;
            //damageNumbers.DamageText = Mathf.RoundToInt(damage).ToString();
            damageNumbers.DamageNumber += Mathf.RoundToInt(damage);

            if (attacker.tag.Equals("Player"))
            {
                // Take breakpoint damage
                TakeBreakDamage(damage);
            }


            // Update at every damage taken the health
            enemyUI.UpdateHealth(health);
            enemyUI.UpdateBreakPoint(_break);

            // When attacking, cannot be changing to hurt animation
            if (this.isAttacking)
            {

            }
            else
            {
                // Play Hurt Animation
                anim.SetTrigger("isHurt");
            }

            // Break/Juggle System - when broken, send them in the air and only allowing hurt/idle animations.
            if (this._break.GetIsBreak() && attacker.tag.Equals("Player"))
            {
                //Debug.Log("start juggling!!");
                StartCoroutine(Juggle(attacker, pushback));
            }

            if (health.GetHealth() <= 0f)
            {
                // Change to dead anim controller.
                //this.anim.runtimeAnimatorController = deadAnimController;
                animatorController.ChangeAnimController(AnimatorController.AnimStates.Dead);

                health.BloodEffectVFX();
                // Play Dead SFX.
                health.PlayDeadSFX();

                // Tell progress manager to delete this gameobject in the list.
                FindObjectOfType<ProgressManager>().DeleteEnemy(this.gameObject.transform.parent.gameObject);

                // Hide the health/break bar of the enemy
                this.enemyUI.GetEnemyCanvas().gameObject.SetActive(false);

                // Destroy its parent, itself and other children from the parent gameobject.
                Destroy(gameObject.transform.parent.gameObject, 5f);

            }
        }

        #endregion Taking Damage Logic

        #region Breaking Enemy Effect

        /// <summary>
        /// Should only juggle when broken. The juggle ensures that enemies are pushbacked at every damage taken.
        /// </summary>
        /// <returns></returns>
        IEnumerator Juggle(GameObject attacker, Vector2 pushback)
        {
            float airborneDuration = 0.5f;
            this.rb2D.gravityScale = 1f;

            this.rb2D.velocity = Vector2.zero;
            float pushDirection = CheckAttackerDirection(attacker);
            //this.rb2D.velocity = new Vector2(pushbackX * pushDirection, pushbackY);
            this.rb2D.velocity = new Vector2(pushback.x * pushDirection, pushback.y);

            // After the airborne duration, remove any pushback effects.
            yield return new WaitForSeconds(airborneDuration);

            this.rb2D.gravityScale = 1f;
        }

        float pushbackX = 0f;
        /// <summary>
        /// Gets whether to apply a pushback on positive x axis or negative x axis depending on who damaged the enemy.
        /// </summary>
        /// <returns></returns>
        private float CheckAttackerDirection(GameObject attacker)
        {
            //float pushbackX = 0f;

            Debug.Log("Enemy is attacked by: " + attacker);
            //if (target == null) { Debug.Log("Attacker is far away"); return pushbackX; }

            if (this.transform.position.x < attacker.transform.position.x)
            {
                pushbackX = -1f;
            }
            else if (this.transform.position.x > attacker.transform.position.x)
            {
                // The enemy is at the right of the target
                pushbackX = 1f;
            }
            return pushbackX;
        }

        /// <summary>
        /// Accumulate break point damage and when it meets the threshold, the enemy is then broken.
        /// When broken they are sent airborne upon receiving damage. 
        /// For players, this is good to juggle them while reducing damage received.
        /// </summary>
        /// <param name="breakPoint"></param>
        private void TakeBreakDamage(float breakPoint)
        {
            if (_break.GetCanBeBroken())
            {
                this._break.TakeBreakPointDamage(breakPoint);
            }

            if (this._break.GetCurrentBreakPoint() >= this._break.GetBreakPointThreshold() && this._break.GetCanBeBroken())
            {
                this._break.SetIsBreak(true);
                StartCoroutine(ApplyBrokenDurationAndCooldown());
            }
        }

        /// <summary>
        /// After an enemy is broken, they have a cooldown in which they can be broken again.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ApplyBrokenDurationAndCooldown()
        {
            this._break.SetCanBeBroken(false);
            this._break.SetIsBreak(true);

            // Wait for break duration.
            yield return new WaitForSeconds(this._break.GetBreakDuration());

            // After the break duration turn off the break.
            this._break.SetIsBreak(false);

            // Reset currentBreakPoint
            this._break.SetCurrentBreakPoint(0f);
            enemyUI.UpdateBreakPoint(_break);

            // Wait for cooldown to finish.
            yield return new WaitForSeconds(this._break.GetBreakCooldown());


            // After the cooldown can then make the enemy be broken again.
            this._break.SetCanBeBroken(true);

        }

        #endregion Breaking Enemy Effect

        #region Allow Damage Pushback Effect

        [SerializeField] float cooldown = 1f;

        Coroutine _coroutineReference;

        private IEnumerator ChangeIsHit()
        {
            Debug.Log("Changing isHit value in Coroutine");

            this._isHit = true;
            yield return new WaitForSeconds(cooldown);
            this._isHit = false;
        }

        #endregion
    }
}