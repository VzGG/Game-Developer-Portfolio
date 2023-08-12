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
        #region Fields

        [SerializeField] MyStat characterStat;

        // Reference
        private PlayerAttack _myAttack;
        private PlayerMovement _myMovement;
        private Health _myHealth;
        private Energy _myEnergy;
        private Armour _myArmour;
        private Target _myTarget;
        [SerializeField] private AnimatorController _animatorController;             // The animator controller handler. Pass this around other components to change animation/controller.
        [SerializeField] private TimeManager _timeManager;                           // Set this value in the editor for each level.
        [SerializeField] private Collider2D _myColl2D;                               // Set in the inspector.
        private Rigidbody2D _rb2d;                                                   // Initialize at the start of player creation.
        private SpriteRenderer _spriteRenderer;                              // The sprite is what the player looks like.
                                                                            
        private bool _isUsingActionAnim = false;                             // The players main action status used to only have 1 action at a time and without any action buffers.
        public bool _isMidAir = false;                                              // Used to determine whether the player changes animation jump animation or idle animation.
        private bool _isJumpAttack3 = false;                                         // Used to determine whether the player uses the final jump attack and lands on the ground.
        private bool _isNextAttackBow = false;                                       // Determine whether the player, mid air, can attack with bow next.
        private bool _isNextAttackSword = false;                                     // Determine whether the player, mid air, can attack with sword next.
        private bool _disableAttack = false;

        //int skillSlot1 = 0;
        //int skillSlot2 = 1;
        private MyEquipment _characterEquipment;
        public IInteractableEnvironment InteractableEnvironment;
        [SerializeField] private AnimatorController.AnimStates _myJumpLevelAnimState;
        [SerializeField] private PhysicsMaterial2D _material2D;

        #endregion

        #region Control Input Fields

        private bool _primaryAttack => Input.GetMouseButtonDown(0);
        private bool _secondaryAttack => Input.GetMouseButtonDown(1);
        private bool _interact => Input.GetKeyDown(KeyCode.E);
        private bool _dodge => Input.GetKeyDown(KeyCode.LeftShift);
        private bool _jump => Input.GetKeyDown(KeyCode.Space);
        private bool _skill1 => Input.GetKeyDown(KeyCode.Alpha1);
        private bool _skill2 => Input.GetKeyDown(KeyCode.Alpha2);
        private float _moveX => Input.GetAxis("Horizontal");

        #endregion

        #region Animator Methods -  Set by the PlayerNextAnim class and called the moment an anim state starts.

        public void SetIsNextAttackBow(bool status) { this._isNextAttackBow = status; }
        public void SetIsNextAttackSword(bool status) { this._isNextAttackSword = status; }
        // Called in the Animator??? [have to double check]
        public void SetIsUsingActionAnim(bool status) { this._isUsingActionAnim = status; }

        #endregion

        #region Character Stat methods

        public MyStat GetMyStat() { return characterStat; }
        public MyEquipment GetMyEquipment() { return _characterEquipment; }
        public Health GetHealth() { return this._myHealth; }
        public Energy GetEnergy() { return this._myEnergy; }
        public Armour GetArmour() { return this._myArmour; }

        #endregion

        #region Methods

        public void SetTimeManager(TimeManager timeManager) { this._timeManager = timeManager; }     // Set this to the timeManager we have at each level, because this gets missing at the end of each level w/o it
        public PlayerAttack GetPlayerAttack() { return this._myAttack; }
        public void SetIsMidAir(bool isMidAir)
        {
            this._isMidAir = isMidAir;
        }

        #endregion

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
            // Setup base stats
            // Get equipment stats
            // Apply stats

            characterStat = new MyStat();
            _characterEquipment = GetComponent<MyEquipment>();

            _myAttack = GetComponent<PlayerAttack>();
            _myMovement = GetComponent<PlayerMovement>();
            _myHealth = GetComponent<Health>();
            _myEnergy = GetComponent<Energy>();
            _myArmour = GetComponent<Armour>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb2d = GetComponent<Rigidbody2D>();
            _myTarget = GetComponent<Target>();

            PlayerNextAnim.SetPlayerController(this);       // Set the player controller in the other class to start passing booleans needed to do mid air attacks.
            FindObjectOfType<EquipmentManager>().PlayerController = this;
        }

        #endregion Initialization

        #region Player Controls and Behaviour
        /// <summary>
        /// Contains the player frame by frame logic. i.e., movement, attacking, etc.
        /// </summary>
        private void PlayerBehaviour()
        {
            if (_myHealth.GetHealth() <= 0f) { return; }         // Stops any logic below when player dies
            if (_timeManager == null) { return; }                // Stop null reference error
            if (_timeManager.GetIsTimeStopped()) { return; }     // Pause time should stop any action and physics movement - https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#exclude_objects_from_pause


            _myEnergy.EnergyRegen();                             // Always regenerate my energy
            _myAttack.MyTargets = _myTarget.GetTargets();         // Update your player attack's local target.


            _myHealth.HealthRegeneration();                             // Does not regen if you don't have the legendary helmet

            // For interactable environment
            if (_interact)
            {
                Debug.Log("interactable environment?: " + InteractableEnvironment);
                if (InteractableEnvironment != null)
                {
                    InteractableEnvironment.Interaction(this);
                }
            }


            //if (myAttack.skills[0].GetActivateSkill() == true || myAttack.skills[1].GetActivateSkill() == true)
            if (_myAttack.GetFirstSkill().GetActivateSkill() == true || _myAttack.GetSecondSkill().GetActivateSkill() == true)
            {
                return;
            }

            if (_disableAttack) { return; }


            if (_primaryAttack && _myEnergy.HasEnergy())
            {
                // When we left click, proceeed to attack.
                _myAttack.SwordAttack(_rb2d, _isMidAir, _isNextAttackSword);
            }
            else if (_secondaryAttack && _myEnergy.HasEnergy())
            {
                // When we right click, proceed to use special attack - bow
                _myAttack.BowAttack(_rb2d, _isMidAir, _isNextAttackBow);
            }


            // If there is an animation going on, cannot do any pressing
            if (_isUsingActionAnim) { return; }


            // Cannot do any action below when we dash.
            if (_myMovement.GetIsSliding()) { return; }


            // Get A/D or LEFT/RIGHT arrow keys which gives a value between -1 and 1 for moving left or right.
            _myMovement.Move(_rb2d, _animatorController, _moveX);


            // Cannot do any action based when the player does not have any energy.
            if (!(_myEnergy.GetEnergy() > 0)) { return; }


            if (_dodge && _myMovement.GetCanSlide())
            {
                // If we press left shift and we can dash, proceed to dash.
                StartCoroutine(_myMovement.Slide(_rb2d, _animatorController, _myEnergy));
            }
            else if (_jump && _myColl2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                _myMovement.Jump(_rb2d, _myEnergy, _animatorController, _myJumpLevelAnimState);
                SetIsMidAir(true);
            }
            //else if (_skill1 && myAttack.skills[skillSlot1].GetCanActivateSkill())
            else if (_skill1 && _myAttack.GetFirstSkill().GetCanActivateSkill())
            {
                //myAttack.SkillAttack(animatorController, skillSlot1);
                _myAttack.SkillAttack(_animatorController, _myAttack.GetFirstSkill());
            }
            //else if (_skill2 && myAttack.skills[skillSlot2].GetCanActivateSkill())
            else if (_skill2 && _myAttack.GetSecondSkill().GetCanActivateSkill())
            {
                //myAttack.SkillAttack(animatorController, skillSlot2);
                _myAttack.SkillAttack(_animatorController, _myAttack.GetSecondSkill());
            }
        }

        #endregion Player Controls and Behaviour

        #region Called in Animator's animation event on any action based animations

        // Called in Animation Event.
        public void IsUsingActionAnimTrue() { this._isUsingActionAnim = true; _myColl2D.sharedMaterial = null;  }
        // Called in Animation Event.
        public void IsUsingActionAnimFalse() { this._isUsingActionAnim = false; _myColl2D.sharedMaterial = _material2D; }
        // Called in Animation Event.
        public void IsJumpAttack3True() { this._isJumpAttack3 = true; }
        // Called in Animation Event.
        public void IsJumpAttack3False() { this._isJumpAttack3 = false; }

        // Called in anim event - used in Player Air Sword Attack 3 and Landed to stop player from attacking while in this animation.
        public void MakeThisDisable() { _disableAttack = true; }
        public void MakeThisEnable() { _disableAttack = false; }

        public void IsNextAttackBowTrue() { this._isNextAttackBow = true; }
        public void IsNextAttackBowFalse() { this._isNextAttackBow = false; }

        #endregion

        #region Collision Detection Logic - Enemy Projectile Hitting The Player

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Debug.Log("Touched ground");
                // During mid jump attack 3 animation and landed on the ground, show the landed attack animation.
                if (_isJumpAttack3)
                {
                    // This is from a different animation controller - mostly still in Jump controller
                    _animatorController.ChangeAnimationTrigger("isLanded");
                }
                else
                {
                    // Otherwise if we just landed, go straight to the idle animation.
                    _animatorController.ChangeAnimController(AnimatorController.AnimStates.Main);
                    _myAttack.UpdateAttackAnimationSpeed();

                    SetIsMidAir(false);
                }
            }

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
            if (_myHealth.GetHealth() <= 0f) { return; }

            // Don't take any damage when we are evading 
            _myHealth.CanEvadeDamage();
            if (_myHealth.isEvading)
            {
                Debug.Log("Evading!");
                _myHealth.ResetEvade();
                StartCoroutine(_myHealth.EvadeVFX(this._spriteRenderer));
                return;
            }

            float takeDamage = damage;

            if (_myArmour.canDamageReduction)
            {
                // Reduce the damage when we have a legendary armour
                takeDamage = _myArmour.ReduceDamage(damage);
            }

            if (_myArmour.GetArmour() > 0)
            {
                // Take damage
                _myArmour.TakeArmourDamage(takeDamage);

                _myHealth.PlayHurtSFX();
                StartCoroutine(_myHealth.HurtVFX(this._spriteRenderer));
                return;
            }

            // Player takes damage and play its hurt SFX and VFX.
            _myHealth.TakeDamage(takeDamage);
            _myHealth.PlayHurtSFX();
            StartCoroutine(_myHealth.HurtVFX(this._spriteRenderer));

            // When the player dies, appropriate actions are taken.
            if (_myHealth.GetHealth() <= 0f)
            {
                // Change to dead anim controller.
                _animatorController.ChangeAnimController(AnimatorController.AnimStates.Dead);

                // Display blood effect on death.
                _myHealth.BloodEffectVFX();

                // Destroy the player after 5 seconds.
                Destroy(gameObject, 5f);

                // Call LevelManager to the game it is over and switch to a different scene.
                StartCoroutine(_myHealth.WaitingToDie());
            }
        }

        #endregion

    }
}