using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player controller class should be the one doing all the logic of all the Player related class like PlayerAttack, PlayerMovement, Energy, etc.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Reference
    PlayerAttack myAttack;
    PlayerMovement myMovement;
    Health myHealth;
    Energy myEnergy;
    [SerializeField] TimeManager timeManager;                           // Set this value in the editor for each level.
    [SerializeField] Collider2D myColl2D;                               // Set in the inspector.
    [SerializeField] Rigidbody2D rb2d;                                  // Set in the inspector.
    [SerializeField] Animator anim;                                     // Set in the inspector.
    // This bool is changed to true at the start of any animation like Attack, Jump, and Slide.
    // It is then set to false when we return to Idle.
    private bool isUsingActionAnim = false;                             // The players main action status used to only have 1 action at a time and without any action buffers.
    bool isMidAir = false;                                              // Used to determine whether the player changes animation jump animation or idle animation.
    bool isJumpAttack3 = false;                                         // Used to determine whether the player uses the final jump attack and lands on the ground.

    bool isNextAttackBow = false;                                       // Determine whether the player, mid air, can attack with bow next.
    bool isNextAttackSword = false;                                     // Determine whether the player, mid air, can attack with sword next.

    public void SetIsNextAttackBow(bool status) { this.isNextAttackBow = status; }
    public void SetIsNextAttackSword(bool status) { this.isNextAttackSword = status; }

    [SerializeField] RuntimeAnimatorController jumpAnimController;      // Jump anim controller - also holds all the mid air attack animations.
    [SerializeField] RuntimeAnimatorController mainAnimController;      // Main anim controller - includes most of the animations like running, sliding, and grounded attacks.
    [SerializeField] RuntimeAnimatorController deadAnimController;      // Dead anim controller - when the player dies, switch to the death animation forever.
    public void SetIsUsingActionAnim(bool status) { this.isUsingActionAnim = status; }
    public void SetTimeManager(TimeManager timeManager) { this.timeManager = timeManager; }     // Set this to the timeManager we have at each level, because this gets missing at the end of each level w/o it

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
        myAttack = GetComponent<PlayerAttack>();
        myMovement = GetComponent<PlayerMovement>();
        myHealth = GetComponent<Health>();
        myEnergy = GetComponent<Energy>();

        myEnergy.SetEnergy(myEnergy.GetMaxEnergy());    // Initialize my energy at the start of this player creation.
        PlayerNextAnim.SetPlayerController(this);       // Set the player controller in the other class to start passing booleans needed to do mid air attacks.
    }

    /// <summary>
    /// Contains the player frame by frame logic. i.e., movement, attacking, etc.
    /// </summary>
    private void PlayerBehaviour()
    {
        if (myHealth.GetHealth() <= 0f) { return; }         // Stops any logic below when player dies
        if (timeManager == null) { return; }                // Stop null reference error
        if (timeManager.GetIsTimeStopped()) { return; }     // Pause time should stop any action and physics movement - https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#exclude_objects_from_pause


        myEnergy.EnergyRegen();                             // Always regenerate my energy

        
        // Always change to the jump anim when we are not touching the ground
        if (!myColl2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            anim.runtimeAnimatorController = jumpAnimController;
            isMidAir = true;
        }
        else
        {
            // During mid jump attack 3 animation and landed on the ground, show the landed attack animation.
            if (isJumpAttack3)
            {
                anim.SetTrigger("isLanded");
            }
            else
            {
                // Otherwise if we just landed, go straight to the idle animation.
                anim.runtimeAnimatorController = mainAnimController;
                isMidAir = false;
            }
        }

        // If there is an animation going on, cannot do any pressing
        if (isUsingActionAnim) { return; }

        // Cannot do any action below when we dash.
        if (myMovement.GetIsSliding()) { return; }

        // Get A/D or LEFT/RIGHT arrow keys which gives a value between -1 and 1 for moving left or right.
        float moveX = Input.GetAxis("Horizontal");
        myMovement.Run(rb2d, anim, moveX);

        // Cannot do any action based when the player does not have any energy.
        if (!(myEnergy.GetEnergy() > 0)) { return; }

        if (Input.GetKeyDown(KeyCode.LeftShift) && myMovement.GetCanSlide())
        {
            // If we press left shift and we can dash, proceed to dash.
            StartCoroutine(myMovement.Slide(rb2d, anim, myEnergy));
        }
        else if (Input.GetKeyDown(KeyCode.Space) && myColl2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            // Can only jump when we are touching the ground and have pressed the space key.
            myMovement.Jump(rb2d, anim, myEnergy);
            anim.runtimeAnimatorController = jumpAnimController;
            isMidAir = true;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // When we left click, proceeed to attack.
            myAttack.SwordAttack(rb2d, anim, myEnergy, isMidAir, this, isNextAttackSword);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Attempt to bow attack");
            // When we right click, proceed to use special attack - bow
            myAttack.BowAttack(anim, myEnergy, isMidAir, rb2d, this, isNextAttackBow);

        }
    }


    #region Called in Animator's animation event on any action based animations

    // Called in Animation Event.
    public void IsUsingActionAnimTrue() { this.isUsingActionAnim = true; }
    // Called in Animation Event.
    public void IsUsingActionAnimFalse() { this.isUsingActionAnim = false; }
    // Called in Animation Event.
    public void IsJumpAttack3True() { this.isJumpAttack3 = true; }
    // Called in Animation Event.
    public void IsJumpAttack3False() { this.isJumpAttack3 = false; }




    public void IsNextAttackBowTrue() { this.isNextAttackBow = true; }
    public void IsNextAttackBowFalse() { this.isNextAttackBow = false; }


    #endregion


    #region Collision Detection Logic - Finding An Enemy In Our Character's Hitbox

    /// <summary>
    /// This method is called when the player attack class' myHitBox finds an enemy within its box collider range.
    /// Then the enemy is registered as the player's current target.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Adds 1 target for us to attack, used for when using any attack animation
        if (collision.tag == "Enemy" && myAttack.GetMyHitBox().IsTouching(collision.gameObject.GetComponent<CircleCollider2D>()))
        {
            myAttack.AddToMyTargets(collision.gameObject.GetComponent<Health>());
        }
    }

    /// <summary>
    /// Upon exiting the player attack class' myHitBox this method is called to remove the current target.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // This removes our current target when our hitbox discovers that the enemy leaves our hitbox
        if (collision.tag == "Enemy" && !myAttack.GetMyHitBox().IsTouching(collision.gameObject.GetComponent<CircleCollider2D>()))
        {
            myAttack.RemoveToMyTargets(collision.gameObject.GetComponent<Health>());
        }
    }

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
        // Player takes damage and play its hurt SFX and VFX.
        myHealth.TakeDamage(damage);
        myHealth.PlayHurtSFX();
        StartCoroutine(myHealth.HurtVFX());

        // When the player dies, appropriate actions are taken.
        if (myHealth.GetHealth() <= 0f)
        {
            // Change to dead anim controller.
            anim.runtimeAnimatorController = deadAnimController;

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
