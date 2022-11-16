using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Animation Event concept, tutorial and inspired by:
 * 
 * GameDev.tv Team, Davidson, R., Pettie, G. (2019) ‘Complete C# Unity Game Developer 2D – Glitch Garden’ ***2019 Course. 2021 course provides different learning materials*** [Course] 
 * Available at: https://www.udemy.com/course/unitycourse/ 
 */


public class BossController : EnemyController
{
    [Header("Components")]
    private Animator animator;
    public RuntimeAnimatorController mainAnimatorController;
    public RuntimeAnimatorController deathAnimatorController;
    private Rigidbody2D bossRB2D;
    private Health bossHealth;
    private ProgressManager progressManager;
    private DialogueManager dialogueManager;
    


    [Space]
    [Header("Movement")]
    //public float moveX = 2.5f;
    //public float distanceToAttack = 0.5f;
    private float scaleX = 2f;

    [Space]
    [Header("Attack")]
    // Attacking properties
    private float time;
    public float timeToAttack = 3f;
    public bool readyToAttack = false;
    public float weaponDamage = 25f;
    public float spellDamage = 40f;
    public BoxCollider2D weaponSlashHitBox;     // Set this box collider game object's layer to enemy HITBOX to only touch the player layer

    // target components
    //private PlayerMovement target;
    public CapsuleCollider2D characterBody;
    private Health characterHealth;

    [Space]
    [Header("Teleport Points")]
    [SerializeField] private Transform[] teleportLocations;
    [SerializeField] private Transform parentTeleportLocation;

    [Space]
    [Header("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    [SerializeField] private GameObject doorUpper;
    [SerializeField] private GameObject doorLower;

    /* Boss is spawned when a certain condition is met:
     * 
     * 
     * 
     * Boss states:
     * 
     * 1. Idle (default)
     * 2. Walk
     * 3. Attack
     * 4. Cast
     * 5. Teleport (when hurt a certain threshold)
     * 
     * There is no hurt animation - so no flinch on the boss - just change the colour instead
     * 
     */

    #region Setters
    // Changes to the main controller when the boss arrives
    void ChangeToMainAnimatorController() { this.animator.runtimeAnimatorController = mainAnimatorController; }

    void ChangeToDeathAnimatorController() { this.animator.runtimeAnimatorController = deathAnimatorController; }
    #endregion

        // Spell Casting Phase
    public bool isTeleporting = false;
    [Space]
    [Header("Spell properties")]
    private float spellTime = 0f;
    private float spellTimeToAttack = 1f;
    private int numberOfSpellsToCast = 0;
    private int currentSpellsCasted = 0;
    [SerializeField] private GameObject spellProjectile;
    int counter = 0;
    // For enrage phase - spawn platforms
    public GameObject enragePhasePlatform;
    public GameObject enragePhasePlatformInvisible;

    public GameObject actualEPP;
    public GameObject actualEPPI;

    private void Awake()
    {
        //this.target = FindObjectOfType<PlayerMovement>();   // Sets boss' target
        animator = GetComponent<Animator>();                // Get the components
        bossRB2D = GetComponent<Rigidbody2D>();             // Get the rigidbody2d
        bossHealth = GetComponent<Health>();                // Get boss' health component

        // Setting up the components
        progressManager = FindObjectOfType<ProgressManager>();
        progressManager.AddEnemies(this.gameObject);

        doorUpper = GameObject.Find("Door-Tilemap");
        doorLower = GameObject.Find("Door2-Tilemap");

        dialogueManager = FindObjectOfType<DialogueManager>();


        characterHealth = target.GetComponent<Health>();                    // Get the healath component of the character
        characterBody = target.GetComponentInChildren<CapsuleCollider2D>(); // Get the body of the character/player

        // Set the playerUI's bossUI to active
        PlayerUI playerUI = FindObjectOfType<PlayerUI>();                   // Gets the PlayerUI gameobject and script
        playerUI.SetBossPanelUI(true);                                      // Activate the boss UI panel
        playerUI.SetBossHealth(bossHealth);                                 // Set the boss UI panel to track the health of the boss


        int childCount = parentTeleportLocation.childCount;                 // Get child count
        teleportLocations = new Transform[childCount];                      // Create/initialise array
        for (int i = 0; i < parentTeleportLocation.childCount; i++)
        {
            // Set all the transform location
            teleportLocations[i] = parentTeleportLocation.GetChild(i);
        }

        // Instantiate the invisible platforms and the platform in the air but they should be turned off
        actualEPP = Instantiate(enragePhasePlatform);
        actualEPP.transform.SetParent(GameObject.Find("Grid").transform);       // Set it under the Grid gameObject to see the platform
        actualEPPI = Instantiate(enragePhasePlatformInvisible);

        actualEPP.SetActive(false);
        actualEPPI.SetActive(false);

        FindObjectOfType<MusicManager>().PlayMusic(5);                         // This is the music of the boss fight


    }

    protected override void EnemyAI()
    {
        throw new System.NotImplementedException();
    }

    public override void EnemyTakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }




    private void Update()
    {
        if (bossHealth.isNormalPhase)
        {
            MoveToPlayer();
        }
        else 
        {
            SpellAttack();
        }

    }
    #region States



    private void SpellAttack()
    {
        animator.SetBool("isWalking", false);

        spellTime += Time.deltaTime;


        if (!isTeleporting)
        {
            // Play the teleport animation - ONCE
            animator.SetTrigger("isTeleporting");
            isTeleporting = true;                       // Stop teleporting - only once
            numberOfSpellsToCast = Random.Range(5, 15);  // Set our number of spells to cast for this phase
        }

        // After teleporing ONCE, increase time to spellAttack
        // If it is ready to spellAttack
        // Play spell animation

        #region Old Enrage phase
        // Works well but feels too easy to play against
        /*        if (spellTime > spellTimeToAttack)
                {   // Can only cast spell a certain amount - random number defined above
                    if (currentSpellsCasted < numberOfSpellsToCast)
                    {
                        animator.SetTrigger("isCasting");       // Cast spell, in  that animation - there should be an animation event
                        spellTime = 0f;                         // Reset time to attack
                        currentSpellsCasted += 1;               // Increase number of spell casted

                       // Debug.Log("SPELL CASTING ANIMATION");
                    }
                }
                if (currentSpellsCasted >= numberOfSpellsToCast)
                {
                    // Teleport back to the ground, 5,6,7 locations (or 6,7,8)

                    // Teleport back to ground and reset isTelporting, reset currentSpellCasted, and go back to normal phase via boss.isNormalPhase = true
                    // Reset accumulated damage = 0
                    counter++;
                    spellTime = 0f;
                    if (counter == 1)
                    {
                        animator.SetTrigger("isTeleportingToGround");
                        Debug.Log("Calling teleporint to ground animation");
                    }

                }*/
        #endregion

        // NEW VERSION of enrage phase
        if (spellTime > spellTimeToAttack)
        {   
            // Unlimited spell casting
            animator.SetTrigger("isCasting");       // Cast spell, in  that animation - there should be an animation event
            spellTime = 0f;                         // Reset time to attack

        }
        // Only stopped spell casting when HIT during enraged
        if (bossHealth.isHitDuringEnrage)
        {
            // Teleport back to the ground, 5,6,7 locations (or 6,7,8)

            // Teleport back to ground and reset isTelporting, reset currentSpellCasted, and go back to normal phase via boss.isNormalPhase = true
            // Reset accumulated damage = 0
            counter++;
            spellTime = 0f;
            if (counter == 1)
            {
                bossHealth.isHitDuringEnrage = false;
                animator.SetTrigger("isTeleportingToGround");
                Debug.Log("Calling teleporint to ground animation");
            }

        }

    }

    private void MoveToPlayer()
    {
        // Stop doing anything when the boss is dead
        if (bossHealth.GetHealth() <= 0) { return; }

        float currentDistance = Vector2.Distance(this.transform.position, target.transform.position);
        // Debug.Log("Distance: " + currentDistance);

        // Increase current time
        time += Time.deltaTime;

        // Move to the player when boss is far away 
        if (currentDistance > distanceToAttack)
        {
            if (readyToAttack) { return; }      // Only move when we are not attacking

            // Set the animation to walking
            animator.SetBool("isWalking", true);
            if (transform.position.x > target.gameObject.transform.position.x)
            {   // Move left
                bossRB2D.velocity = new Vector2(-moveX, bossRB2D.velocity.y);
                transform.localScale = new Vector2(scaleX, transform.localScale.y);     // Default scale is 2 and it looks to the left, SO don't add negative to the scaleX
            }
            else if (transform.position.x < target.gameObject.transform.position.x)
            {   // Move right
                bossRB2D.velocity = new Vector2(moveX, bossRB2D.velocity.y);
                transform.localScale = new Vector2(-scaleX, transform.localScale.y);    // Default scale is 2 and looks to the left, but we want to look right here, so add negative to scaleX
            }

        }
        else 
        {
            // If within attacking distance

            // During attack ANIMATION: do NOT change the scale x
            if (!readyToAttack)
            {
                // Check where the player is AND Look in that direction
                if (transform.position.x > target.gameObject.transform.position.x)
                {   // Look left
                    transform.localScale = new Vector2(scaleX, transform.localScale.y);
                }
                else if (transform.position.x < target.gameObject.transform.position.x)
                {   // Look right
                    transform.localScale = new Vector2(-scaleX, transform.localScale.y);
                }
            }

            // Turn off walking animation
            animator.SetBool("isWalking", false);

            // If ready to attack
            if (time > timeToAttack && !readyToAttack)
            {
                // Play attack animation
                animator.SetTrigger("isAttacking");

                // Reset Timer
                time = 0f;
                // Then set the ready to attack to true in the animation event
            }
            // Else just play the idle animation - the default state
        }

    }

    #endregion



    #region Called by Animation Event in the Animation Tab within Unity Editor

    // Called in BringerOfDeath_Attack animation
    //public void SetReadyToAttackTrue()
    //{
    //    readyToAttack = true;
    //}
    // Called in BringerOfDeath_Attack animation
    //public void SetReadyToAttackFalse()
    //{
    //    readyToAttack = false;
    //}

    // Called in BringerOfDeath_Attack animation in one of the frames
    private void AnimationEventAttack()
    {
        /*
         *  If during a certain time frame in an animation event
         *  AND
         *  if the enemy hits the player within the enemy's HITBOX
         *  THEN
         *  damage the player
         */
        if (weaponSlashHitBox.IsTouching(characterBody))
        {
            //characterHealth.TakeDamage(weaponDamage);
            characterHealth.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(weaponDamage);
        }

    }

    private void TeleportWhenHurt()
    {
        bossRB2D.bodyType = RigidbodyType2D.Dynamic; // Reset to dynamic
        GetComponent<BoxCollider2D>().enabled = true;

        // Debug.Log("Teleport locations length: " + teleportLocations.Length);
        int randNum = Random.Range(0, teleportLocations.Length-3);          // Teleport to one of the higher platforms to force the player to find the player

        // Teleport to one of the teleport locations
        transform.position = teleportLocations[randNum].position;
        // Teleport to one of the random spots in the boss area using vector3s

        // Debug.Log("ENRAGE PHASE");

        
    }

    




    // Teleports to one of the grounds
    public void TeleportBackToGroundAfterEnragePhase()
    {


        // Random between position 5-7 of teleport locations (8 is exclusive - not added)
        // int randNum = Random.Range(5, 8);
        transform.position = teleportLocations[7].position;
        // Reset them all to back to normal phase
        bossHealth.isNormalPhase = true;
        bossHealth.accumulatedDamage = 0f;      
        isTeleporting = false;
        currentSpellsCasted = 0;

        // Debug.Log("TELEPORTING BACK TO THE GROUND!!"); // For some reason this is being called twice...
        counter = 0;

        // Reset the platforms too
        actualEPP.gameObject.SetActive(false);
        actualEPPI.gameObject.SetActive(false);
    }

    private void SpellCasting()
    {
        // Spawn the shadow thunder projectile at the player's position with no rotation
        BossSpellProjectile bSP = spellProjectile.GetComponentInChildren<BossSpellProjectile>();
        bSP.SetSpellDamage(spellDamage);
        bSP.SetTargetBody(characterBody);
        bSP.SetCharacterHealth(characterHealth);

        Instantiate(spellProjectile, target.transform.position, Quaternion.identity);


    }

    private void AttackSFXAnimationEvent()
    {
        audioSource.PlayOneShot(audioClips[0]);
    }



    // Called when the boss is dead
    private void RemoveEnemiesInProgressManager()
    {
        // Set its collider to false, update method to false
        this.enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;

        this.GetComponentInChildren<CircleCollider2D>().enabled = false;

        // Remove all the gates
        doorUpper.gameObject.SetActive(false);
        doorLower.gameObject.SetActive(false);

        // Call dialogue manager to react to this: Gates have opened... I wonder what is next
        int dialogueIndex = 19;
        Instantiate(dialogueManager.InstantiateDialogue(dialogueIndex));

        // Remove all enemies from progress manager
        progressManager.RemoveAll();
        
       
    }

    #endregion


    #region Boss Taking Damage From Bow Damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile") && collision.gameObject.tag == "Player")
        {
            // Get the arrow script and use that to make the boss take damage
            ArrowProjectile arrowProjectile = collision.gameObject.GetComponent<ArrowProjectile>();
            this.bossHealth.TakeDamage(arrowProjectile.GetBowDamage());
        }
    }


    #endregion

}
