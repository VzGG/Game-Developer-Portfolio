using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Enemy;
using Oswald.Manager;
//using UnityEditor.Animations;

namespace Oswald.Player
{
    /// <summary>
    /// The player controller class should be the one doing all the logic of all the Player related class like PlayerAttack, PlayerMovement, Energy, etc.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] MyStat characterStat;

        public MyStat GetMyStat() { return characterStat; }

        MyEquipment characterEquipment;

        public IInteractableEnvironment interactableEnvironment;

        // Reference
        PlayerAttack myAttack;
        PlayerMovement myMovement;
        Health myHealth;
        Energy myEnergy;
        Armour myArmour;
        Target myTarget;
        [SerializeField] AnimatorController animatorController;             // The animator controller handler. Pass this around other components to change animation/controller.
        [SerializeField] TimeManager timeManager;                           // Set this value in the editor for each level.
        [SerializeField] Collider2D myColl2D;                               // Set in the inspector.
        Rigidbody2D rb2d;                                                   // Initialize at the start of player creation.
        private SpriteRenderer spriteRenderer;                              // The sprite is what the player looks like.
                                                                            // This bool is changed to true at the start of any animation like Attack, Jump, and Slide.
                                                                            // It is then set to false when we return to Idle.
        private bool isUsingActionAnim = false;                             // The players main action status used to only have 1 action at a time and without any action buffers.
        public bool isMidAir = false;                                              // Used to determine whether the player changes animation jump animation or idle animation.
        bool isJumpAttack3 = false;                                         // Used to determine whether the player uses the final jump attack and lands on the ground.
        bool isNextAttackBow = false;                                       // Determine whether the player, mid air, can attack with bow next.
        bool isNextAttackSword = false;                                     // Determine whether the player, mid air, can attack with sword next.
        bool disableAttack = false;

        [SerializeField] private AnimatorController.AnimStates myJumpLevelAnimState;

        int skillSlot1 = 0;
        int skillSlot2 = 1;

        #region Set by the PlayerNextAnim class and called the moment an anim state starts.

        public void SetIsNextAttackBow(bool status) { this.isNextAttackBow = status; }
        public void SetIsNextAttackSword(bool status) { this.isNextAttackSword = status; }

        #endregion

        public void SetIsUsingActionAnim(bool status) { this.isUsingActionAnim = status; }
        public void SetTimeManager(TimeManager timeManager) { this.timeManager = timeManager; }     // Set this to the timeManager we have at each level, because this gets missing at the end of each level w/o it
        public PlayerAttack GetPlayerAttack() { return this.myAttack; }

        private void Awake()
        {
            Singleton();
            SetReferences();
        }

        // Update is called once per frame - all the logic and input events here
        void Update()
        {
            PlayerBehaviour();
        }

        #region Initialization
        private void Singleton()
        {
            // Singleton - only one class should exist
            int player = FindObjectsOfType<PlayerController>().Length;
            if (player > 1)
                Destroy(this.gameObject);
            else
                DontDestroyOnLoad(this.gameObject);
        }
        private void SetReferences()
        {
            // Delete LATER!
            FindObjectOfType<EquipmentRandomizer>().LoadSprites();
            //FindObjectOfType<EquipmentRandomizer>().GenerateEquipment();
           // FindObjectOfType<EquipmentRandomizer>().AddEquipmentToPlayer();

            // Setup base stats
            // Get equipment stats
            // Apply stats

            characterStat = new MyStat();
            characterEquipment = GetComponent<MyEquipment>();

            myAttack = GetComponent<PlayerAttack>();
            myMovement = GetComponent<PlayerMovement>();
            myHealth = GetComponent<Health>();
            myEnergy = GetComponent<Energy>();
            myArmour = GetComponent<Armour>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb2d = GetComponent<Rigidbody2D>();
            myTarget = GetComponent<Target>();

            PlayerNextAnim.SetPlayerController(this);       // Set the player controller in the other class to start passing booleans needed to do mid air attacks.
        }

        #endregion Initialization

        #region Player Controls and Behaviour
        /// <summary>
        /// Contains the player frame by frame logic. i.e., movement, attacking, etc.
        /// </summary>
        private void PlayerBehaviour()
        {
            if (myHealth.GetHealth() <= 0f) { return; }         // Stops any logic below when player dies
            if (timeManager == null) { return; }                // Stop null reference error
            if (timeManager.GetIsTimeStopped()) { return; }     // Pause time should stop any action and physics movement - https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#exclude_objects_from_pause


            myEnergy.EnergyRegen();                             // Always regenerate my energy
            myAttack.MyTargets = myTarget.GetTargets();         // Update your player attack's local target.


            myHealth.HealthRegen();                             // Does not regen if you don't have the legendary helmet


            // For interactable environment
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (interactableEnvironment != null)
                {
                    interactableEnvironment.Interaction();
                }
            }


            if (myAttack.skills[0].GetActivateSkill() == true || myAttack.skills[1].GetActivateSkill() == true)
            {
                return;
            }



            // Always change to the jump anim when we are not touching the ground
            if (!myColl2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                animatorController.ChangeAnimController(myJumpLevelAnimState);
                isMidAir = true;
            }
            else
            {
                // During mid jump attack 3 animation and landed on the ground, show the landed attack animation.
                if (isJumpAttack3)
                {
                    // This is from a different animation controller - mostly still in Jump controller
                    animatorController.ChangeAnimationTrigger("isLanded");
                }
                else
                {
                    // Otherwise if we just landed, go straight to the idle animation.
                    animatorController.ChangeAnimController(AnimatorController.AnimStates.Main);
                    isMidAir = false;
                }
            }

            if (disableAttack) { return; }
            // Cannot do any action based when the player does not have any energy.
            //if (!(myEnergy.GetEnergy() > 0)) { return; }

            if (Input.GetMouseButtonDown(0) && myEnergy.GetEnergy() > 0)
            {
                
                // When we left click, proceeed to attack.
                //myAttack.SwordAttack(rb2d, animatorController, isMidAir, isNextAttackSword);
                myAttack.SwordAttack(rb2d, isMidAir, isNextAttackSword);
            }
            else if (Input.GetMouseButtonDown(1) && myEnergy.GetEnergy() > 0)
            {
                // When we right click, proceed to use special attack - bow
                //myAttack.BowAttack(rb2d, animatorController, isMidAir, isNextAttackBow);
                myAttack.BowAttack(rb2d, isMidAir, isNextAttackBow);
            }

            // If there is an animation going on, cannot do any pressing
            if (isUsingActionAnim) { /*rb2d.velocity = new Vector2(0, rb2d.velocity.y);*/ return; }

            // Cannot do any action below when we dash.
            if (myMovement.GetIsSliding()) { return; }

            // Get A/D or LEFT/RIGHT arrow keys which gives a value between -1 and 1 for moving left or right.
            float moveX = Input.GetAxis("Horizontal");
            myMovement.Move(rb2d, animatorController, moveX);

           
            // Cannot do any action based when the player does not have any energy.
            if (!(myEnergy.GetEnergy() > 0)) { return; }

            if (Input.GetKeyDown(KeyCode.LeftShift) && myMovement.GetCanSlide())
            {
                // If we press left shift and we can dash, proceed to dash.
                StartCoroutine(myMovement.Slide(rb2d, animatorController, myEnergy));
            }
            else if (Input.GetKeyDown(KeyCode.Space) && myColl2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                // Can only jump when we are touching the ground and have pressed the space key.
                //myMovement.Jump(rb2d, myEnergy, animatorController);
                myMovement.Jump(rb2d, myEnergy, animatorController, myJumpLevelAnimState);
                isMidAir = true;
            }
            //else if (Input.GetMouseButtonDown(0))
            //{
            //     When we left click, proceeed to attack.
            //    myAttack.SwordAttack(rb2d, animatorController, myEnergy, isMidAir, isNextAttackSword);
            //}
            //else if (Input.GetMouseButtonDown(1))
            //{
            //    // When we right click, proceed to use special attack - bow
            //    myAttack.BowAttack(rb2d, animatorController, isMidAir, isNextAttackBow);
            //}
            
            else if (Input.GetKeyDown(KeyCode.Alpha1) && myAttack.skills[skillSlot1].GetCanActivateSkill())
            {
                myAttack.SkillAttack(animatorController, skillSlot1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && myAttack.skills[skillSlot2].GetCanActivateSkill())
            {
                myAttack.SkillAttack(animatorController, skillSlot2);
            }
        }



        #endregion Player Controls and Behaviour

        [SerializeField] PhysicsMaterial2D material2D;                                          // MOVE THIS!!!

        #region Called in Animator's animation event on any action based animations

        // Called in Animation Event.
        public void IsUsingActionAnimTrue() { this.isUsingActionAnim = true; myColl2D.sharedMaterial = null;  }
        // Called in Animation Event.
        public void IsUsingActionAnimFalse() { this.isUsingActionAnim = false; myColl2D.sharedMaterial = material2D; }
        // Called in Animation Event.
        public void IsJumpAttack3True() { this.isJumpAttack3 = true; }
        // Called in Animation Event.
        public void IsJumpAttack3False() { this.isJumpAttack3 = false; }

        // Called in anim event - used in Player Air Sword Attack 3 and Landed to stop player from attacking while in this animation.
        public void MakeThisDisable() { disableAttack = true; }
        public void MakeThisEnable() { disableAttack = false; }

        public void IsNextAttackBowTrue() { this.isNextAttackBow = true; }
        public void IsNextAttackBowFalse() { this.isNextAttackBow = false; }

        #endregion

        #region Collision Detection Logic - Enemy Projectile Hitting The Player

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile") && collision.gameObject.tag == "EnemyProjectile")
            {
                EnemyArrowProjectile enemyArrowProjectile = collision.gameObject.GetComponent<EnemyArrowProjectile>();
                PlayerTakeDamage(enemyArrowProjectile.GetDamage());
            }
        }

        #endregion

        #region Taking Damage Logic

        /// <summary>
        /// Player takes damage, display VFX, play hurt SFX and when dead, change to game over scene.
        /// </summary>
        /// <param name="damage"></param>
        public void PlayerTakeDamage(float damage)
        {
            if (myHealth.GetHealth() <= 0f) { return; }

            // Don't take any damage when we are evading 
            myHealth.CanEvadeDamage();
            if (myHealth.isEvading)
            {
                Debug.Log("Evading!");
                myHealth.ResetEvade();
                StartCoroutine(myHealth.EvadeVFX(this.spriteRenderer));
                return;
            }

            float takeDamage = damage;

            if (myArmour.canDamageReduction)
            {
                // Reduce the damage when we have a legendary armour
                takeDamage = myArmour.ReduceDamage(damage);
            }

            if (myArmour.GetArmour() > 0)
            {
                // Take damage
                myArmour.TakeArmourDamage(takeDamage);

                myHealth.PlayHurtSFX();
                StartCoroutine(myHealth.HurtVFX(this.spriteRenderer));
                return;
            }

            // Player takes damage and play its hurt SFX and VFX.
            myHealth.TakeDamage(takeDamage);
            myHealth.PlayHurtSFX();
            StartCoroutine(myHealth.HurtVFX(this.spriteRenderer));

            // When the player dies, appropriate actions are taken.
            if (myHealth.GetHealth() <= 0f)
            {
                // Change to dead anim controller.
                animatorController.ChangeAnimController(AnimatorController.AnimStates.Dead);

                // Display blood effect on death.
                myHealth.BloodEffectVFX();

                // Destroy the player after 5 seconds.
                Destroy(gameObject, 5f);

                // Call LevelManager to the game it is over and switch to a different scene.
                StartCoroutine(myHealth.WaitingToDie());
            }
        }

        #endregion

    }
}