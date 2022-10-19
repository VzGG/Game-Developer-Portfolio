using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to the player ship's game object
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] EnemyController currentTarget;
    [SerializeField] List<EnemyController> targets = new List<EnemyController>();
    [SerializeField] private GameObject scannerGameObj;
    [SerializeField] private PlayerShip playerShip;
    [SerializeField] Rigidbody2D rb2d;
    //[SerializeField] BoxCollider2D myBoxCollider2d;

    [SerializeField] GameObject projectile;

    [SerializeField] float evasiveSpeedBoost = 75f;
    [SerializeField] float evasiveSpeedBoostForward = 75f;

    public void SetPlayerShip(PlayerShip playerShip) { this.playerShip = playerShip; }
    public void SetRB2D(Rigidbody2D rigidbody2D) { this.rb2d = rigidbody2D; }
    public PlayerShip GetPlayerShip() { return this.playerShip; }
    public void SetProjectile(GameObject projectile) { this.projectile = projectile; }
    public void SetPlayerRegenOnStatus(bool status)
    {
        this.playerShip.GetInstalledFrame().SetIsRegenOn(status);
    }
    public void SetScanner(GameObject scanner) { this.scannerGameObj = scanner; }
    public List<EnemyController> GetTargets() { return this.targets; }
    public void ResetTargetsAndCurrentTarget()
    {
        this.targets.Clear();
        this.currentTarget = null;
    }

    private void Awake()
    {
        // SINGLETON AND DONT DESTROY ON THE NEXT LEVEL
        int playerControllers = FindObjectsOfType<PlayerController>().Length;
        if (playerControllers > 1)
        {
            GameObject parentOfThis = this.gameObject.transform.parent.gameObject;
            // Destroy parent and self
            Destroy(parentOfThis);
            // Destroy self and then the parent
            //Destroy(this.gameObject);
            
        }
        else
        {
            GameObject parentOfThis = this.gameObject.transform.parent.gameObject;
            DontDestroyOnLoad(parentOfThis);
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //FindObjectOfType<PlayerCamera>().SetPlayerController(this);
        //FindObjectOfType<PlayerUI>().SetPlayerController(this);

        // Initialize Scanner
        InitializeScanner();

        // Apply all augments on start - if there is none, the components use the original value without augments
        this.playerShip.GetInstalledWeapons()[0].ApplyAugments();
        this.playerShip.GetInstalledArmour().ApplyAugments();
        this.playerShip.GetInstalledFrame().ApplyAugments();
        this.playerShip.GetInstalledCore().ApplyAugments();
        this.playerShip.GetInstalledBooster().ApplyAugments();

    }

    [SerializeField] private float startingBoostSpeed = 1f;
    [SerializeField] private float currentBoostSpeed = 1f;

    // Dodging properties
    [SerializeField] float timeToActivate = 0.5f;
    float curTime = 0f;
    float dodgeTime = 0f;

    // Attacking properties
    float curAtkTime = 0f;

    private void Update()
    {
        if (FindObjectOfType<PlayerCamera>().GetPlayerController() == null)
        {
            Debug.Log("No camera and ui reference of the player controller, now reset and add it back again");
            // Reset only once
            FindObjectOfType<PlayerCamera>().SetPlayerController(this);
            FindObjectOfType<PlayerUI>().SetPlayerController(this);
            this.ResetTargetsAndCurrentTarget();
            // Make player go to the starting position
            FindObjectOfType<PlayerOnLoad>().OnNextLevelLoadPlacePlayerInStartingRoom(this);
        }



        // Activate Regen per second for Hull and Energy, then always check if the armour is alive
        this.playerShip.GetInstalledFrame().ComponentActive();
        this.playerShip.GetInstalledCore().ComponentActive();
        this.playerShip.GetInstalledArmour().ComponentActive();

        // Get status of frame
        bool isPlayerDead = this.playerShip.GetInstalledFrame().GetIsFrameDestroyed();

        
        if (isPlayerDead) 
        {
            Debug.Log("Player is dead...");
            SelfDestruct();
            this.enabled = false;

            // Go to main menu and reset everything
            FindObjectOfType<ProgressionManager>().ResetEverything("Defeat");

            return;
        }

        //Debug.Log("Player energy: " + this.playerShip.GetInstalledCore().GetCurrentEnergy() + " | total regen: " + this.playerShip.GetInstalledCore().GetTotalRegen());

        // Run or Move faster key
        SelectTarget();
        BoostSpeed();
        DodgeBoostKey();
        Attack();

    }

    Vector2 moveXY;
    bool canDodgeBoost = false;


    // Update is called once per frame
    void FixedUpdate()
    {
        // Get input from the player by .Move()
        moveXY = playerShip.Move();


        ShipBasicMovement(moveXY.x, moveXY.y);
        //DodgeBoost(moveXY.x, moveXY.y);


        if (canDodgeBoost)
        {
            // evade thrust forward or backward

            //Debug.Log("movexy.x: " + movexy.y);
            //Debug.Log("evasive speed boost: " + evasivespeedboost);


           // Debug.Log("pressing dodge");
            rb2d.AddForce((transform.up * evasiveSpeedBoostForward * moveXY.y), ForceMode2D.Impulse);
            dodgeTime += Time.fixedDeltaTime;

            if (dodgeTime > 0.15f)
            {
                curTime = 0f;
                dodgeTime = 0f;
                canDodgeBoost = false;
            }

            this.playerShip.GetInstalledCore().DodgeBoost();
            this.playerShip.GetInstalledCore().SetIsEnergyUsed(true);
        }

        if (sideThrustLeft)
        {
            rb2d.AddForce((this.transform.right * evasiveSpeedBoostForward * -1f), ForceMode2D.Impulse);
            // dodgeTime += Time.fixedDeltaTime;

            // The time - duration of the movement
            dodgeTime += Time.fixedDeltaTime;
            if (dodgeTime > 0.15f)
            {
                curTime = 0f;
                dodgeTime = 0f;
                sideThrustLeft = false;
            }

            this.playerShip.GetInstalledCore().DodgeBoost();
            this.playerShip.GetInstalledCore().SetIsEnergyUsed(true);
        }

        if (sideThrustRight)
        {
            rb2d.AddForce((this.transform.right * evasiveSpeedBoostForward * 1f), ForceMode2D.Impulse);

            dodgeTime += Time.fixedDeltaTime;
            if (dodgeTime > 0.15f)
            {
                curTime = 0f;
                dodgeTime = 0f;
                sideThrustRight = false;
            }

            this.playerShip.GetInstalledCore().DodgeBoost();
            this.playerShip.GetInstalledCore().SetIsEnergyUsed(true);
        }

    }

    #region Movement and Dodging

    // Only moves our ship when we press wasd or arrow keys - no energy cost
    private void ShipBasicMovement(float moveXY_X, float moveXY_Y)
    {
        //float boosterBaseSpeed = playerShip.GetInstalledBooster().GetValues()[2];
        float normalSpeed = playerShip.GetInstalledBooster().GetNormalSpeed();

        //Debug.Log("normal speed: " + normalSpeed);

        // Debug.Log("MoveXY X: " + moveXY.x);
        //float overallboostmovement = moveXY_Y * boosterBaseSpeed * currentBoostSpeed;
        // Current boostSpeed is the ship with boosted speed (not normal speed) - think of it as RUNNING AND WALKING
        float overallboostmovement = moveXY_Y * normalSpeed * currentBoostSpeed;

        //Debug.Log("Overall boost movement: " + overallboostmovement);

        // Moves forward or backward while also considering the rotation due to the transform.up
        rb2d.velocity = transform.up * overallboostmovement;

        // Rotates the ship gameobject
        rb2d.MoveRotation(rb2d.rotation + moveXY_X * normalSpeed);
    }

    private void BoostSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //Debug.Log("Attempting to boost speed");
            //if (this.playerShip.GetInstalledCore().GetCurrentEnergy() > 0f)
            float currentEnergy = this.playerShip.GetInstalledCore().GetCurrentEnergy();
            //Debug.Log("Current en: " + currentEnergy);

            //if (this.playerShip.GetInstalledCore().GetIsEnergyZero() == false && this.playerShip.GetInstalledCore().GetCurrentEnergy() > 0)
            if (currentEnergy > 0f)
            {
                //Debug.Log("Energy is not zero");
                //float addboostSpeed = playerShip.GetInstalledBooster().GetValues()[0];
                float addboostSpeed = playerShip.GetInstalledBooster().GetBoostSpeed();

                // Debug.Log("Boost base speed: " + addboostSpeed);
                currentBoostSpeed = addboostSpeed;

                // Then take energy
                this.playerShip.GetInstalledCore().EnergyBoost();
                // Stop energy from increasing
                this.playerShip.GetInstalledCore().SetIsEnergyUsed(true);
                //Debug.Log("status; " + this.playerShip.GetInstalledCore().GetIsEnergyUsed());
            }
            else
            {
                
                //// If current energy is less than or equal to 0f, set is energy to false
                // this.playerShip.GetInstalledCore().SetIsEnergyUsed(false);
                currentBoostSpeed = startingBoostSpeed;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //Debug.Log("Regen starting");

            // The moment we release the key, regen again
            currentBoostSpeed = startingBoostSpeed;
            // This causes the jagged movement - when we reach zero
            this.playerShip.GetInstalledCore().SetIsEnergyUsed(false);
        }

        //else
        //{
        //    currentBoostSpeed = boostSpeed;
        //    // This causes the jagged movement - when we reach zero
        //    this.playerShip.GetInstalledCore().SetIsEnergyUsed(false); 
        //    // REMEMBER WHEN WE ARE NOT PRESSING THEN is energy used will always be false!!! THIS IS CALLED TWICE where the seocnd one is IN DODGE BOOST AS WELL
        //}


        curTime += Time.deltaTime;
    }

    bool sideThrustLeft = false;
    bool sideThrustRight = false;
    private void DodgeBoostKey()
    {
        // Press Q and Space to left boost
        if ( (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.Space)) && curTime > timeToActivate)
        {
            if (this.playerShip.GetInstalledCore().GetCurrentEnergy() > 0f)
            {
                if (this.playerShip.GetInstalledCore().CanDodgeBoost())
                {
                    sideThrustLeft = true;
                }
            }
            
        }
        //if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Space))
        //{
        //    sideThrustLeft = false;
        //    this.playerShip.GetInstalledCore().SetIsEnergyUsed(false);
        //}



        else if ( (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Space)) && curTime > timeToActivate)
        {
            if (this.playerShip.GetInstalledCore().GetCurrentEnergy() > 0f)
            {
                if (this.playerShip.GetInstalledCore().CanDodgeBoost())
                {
                    sideThrustRight = true;
                }
            }
            
        }
        //if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Space))
        //{
        //    sideThrustRight = false;
        //    this.playerShip.GetInstalledCore().SetIsEnergyUsed(false);
        //}



        else if (Input.GetKey(KeyCode.Space) && curTime > timeToActivate)
        {
            if (this.playerShip.GetInstalledCore().GetCurrentEnergy() > 0f)
            {
                if (this.playerShip.GetInstalledCore().CanDodgeBoost())
                {
                    canDodgeBoost = true;
                }
            }
        }
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    canDodgeBoost = false;
        //    this.playerShip.GetInstalledCore().SetIsEnergyUsed(false);
        //}

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.Space))
        {
            sideThrustLeft = false;
            this.playerShip.GetInstalledCore().SetIsEnergyUsed(false);
        }
        else if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Space))
        {
            sideThrustRight = false;
            this.playerShip.GetInstalledCore().SetIsEnergyUsed(false);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            canDodgeBoost = false;
            this.playerShip.GetInstalledCore().SetIsEnergyUsed(false);
        }


    }

    #endregion

    #region Player Taking Damage

    // Player controller class should handle which component to damage, frame or armour
    public void PlayerTakeDamage(float damage)
    {
        // If armour is not zero, take armour damage instead of frame damage, otherwise take player health damage
        float armour = playerShip.GetInstalledArmour().GetCurrentArmour();
        if (armour > 0f)
        {
            playerShip.GetInstalledArmour().TakeArmourDamage(damage);
        }
        else
        {
            // Frame damage taken
            float evasion = this.playerShip.GetInstalledBooster().GetEvasion();

            float randomNumber0to1 = Random.Range(0f, 1f);
            if (randomNumber0to1 > evasion)
            {
                playerShip.GetInstalledFrame().TakeDamage(damage);
            }
            else
            {
                Debug.Log("Player is evading!!!");
            }

            

        }
            

    }

    #endregion

    #region Target Locking An Enemy - MUST HAPPEN BEFORE ATTACKING!!!

    [SerializeField] private int targetIndex = 0;

    private void SelectTarget()
    {
        // Press Right Mouse Button for Changing targets
        // By default chosen target is the first index
        // SHOULD ONLY BE ALLOWED WHEN WEAPON TYPE is 1 (you have weapon type 0 and 1)


        //Debug.Log("Weapon type: " + this.playerShip.GetInstalledWeapons()[0].GetValues()[3]);
        // If weapon type 1, proceed to select a target
        if (this.playerShip.GetInstalledWeapons()[0].GetValues()[3] == 1)
        {
            //if (this.targets.Count <= 0 && currentTarget == null)

            // Default target would be the first target in the list UNLESS WE PRESS A KEY
            if (this.targets.Count > 0 && this.targets.Count <= 1)
            {
                // Set current target
                currentTarget = this.targets[0];

                if (currentTarget == null) { return; }  // GUARD CHECK

                int lastIndex = currentTarget.transform.childCount - 1;
                currentTarget.transform.GetChild(lastIndex).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else if (this.targets.Count > 1)
            {
                // 1 = right click, 0 = left, 2 = middle click
                // If target count is more than 1, check for press key to toggle for enemies
                // If right clicked, change targets
                if (Input.GetMouseButtonDown(1))
                {
                    targetIndex += 1;

                    if (targetIndex < this.targets.Count)
                    {
                        // If the count of the index is within the list count, you can set the target - KEEP CHANGING THE INDEX TO THE LAST
                        currentTarget = this.targets[targetIndex];

                        if (currentTarget == null) { return; } // GUARD CHECK


                        int lastIndex = currentTarget.transform.childCount - 1;
                        currentTarget.transform.GetChild(lastIndex).gameObject.GetComponent<SpriteRenderer>().enabled = true;

                        // DISABLE ALL BUT THE CURRENT TARGET's sprite renderer
                        DisableLockOnInAllTargetsButCurrent(currentTarget.gameObject);
                    }
                    else if (targetIndex >= this.targets.Count)
                    {
                        // If greater than the count, start to the starting index = 0 - RESET
                        targetIndex = 0;
                        currentTarget = this.targets[targetIndex];

                        if (currentTarget == null) { return; } // GUARD CHECK 

                        int lastIndex = currentTarget.transform.childCount - 1;
                        currentTarget.transform.GetChild(lastIndex).gameObject.GetComponent<SpriteRenderer>().enabled = true;

                        // Disable sprite renderer of all but current
                        DisableLockOnInAllTargetsButCurrent(currentTarget.gameObject);
                    }
                }

            }
            else if (this.targets.Count <= 0)
            {
                currentTarget = null;
            }

        }
    }

    private void DisableLockOnInAllTargetsButCurrent(GameObject currentTarget)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (currentTarget.gameObject.GetInstanceID() == targets[i].gameObject.GetInstanceID())
            {
                // If matching ID, do not do anything
            }
            else if (currentTarget.gameObject.GetInstanceID() != targets[i].gameObject.GetInstanceID())
            {
                // DISABLE ALL SPRITE RENDERER BUT THE CURRENT TARGET

                int lastIndex = targets[i].gameObject.transform.childCount - 1;
                // If its not matching ID, disable its sprite renderer
                targets[i].gameObject.transform.GetChild(lastIndex).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

        }
    }

    // Used when a target left the player's range scanner
    public void DisableLockOnWhenEnemyLeavesScannerRange(GameObject enemyGameObj)
    {
        int lastIndex = enemyGameObj.gameObject.transform.childCount - 1;
        enemyGameObj.gameObject.transform.GetChild(lastIndex).gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    #endregion

    #region Player Attacking

    private void Attack()
    {
        string weaponName = this.playerShip.GetInstalledWeapons()[0].GetName();

        //Debug.Log("player weapon name: " + weaponName);

        if (string.IsNullOrEmpty(weaponName))
        { return; }

        //float playerDamage = this.playerShip.GetInstalledWeapons()[0].GetValues()[0];
        //float fireRate = this.playerShip.GetInstalledWeapons()[0].GetValues()[1];
        //float consumption = this.playerShip.GetInstalledWeapons()[0].GetValues()[2];
        float playerDamage = this.playerShip.GetInstalledWeapons()[0].GetPower();
        float fireRate = this.playerShip.GetInstalledWeapons()[0].GetFireRate();
        float consumption = this.playerShip.GetInstalledWeapons()[0].GetEnergyConsumption();

        int playerWeaponType = (int) this.playerShip.GetInstalledWeapons()[0].GetValues()[3];

        //Debug.Log("Consumption: " + consumption);

        curAtkTime += Time.deltaTime;

        // Left click
        if (Input.GetMouseButton(0))
        {
            if (this.playerShip.GetInstalledCore().GetCurrentEnergy() > 0f)
            {
                if (this.playerShip.GetInstalledCore().CanConsumeEnergy(consumption))
                {
                    // If we can attack
                    if (curAtkTime > fireRate)
                    {
                        //Debug.Log("Attacking");

                        curAtkTime = 0f;
                        // Create and setup a projectile
                        GameObject proj = Instantiate(projectile, transform.position, this.transform.rotation);
                        Projectile projectileComponent = proj.GetComponent<Projectile>();
                        // projectileComponent.SetLayerName("Player");
                        projectileComponent.SetDamage(playerDamage);
                        //projectileComponent.SetWeaponType(playerWeaponType);
                        projectileComponent.SetWeaponType(playerWeaponType);
                        projectileComponent.SetIsPlayerStatus(true);
                        if (playerWeaponType == 1)
                        {

                            if (currentTarget == null) { Debug.Log("Player has no target in range. Cannot fire"); return; }

                            // Set the target IF OUR WEAPON TYPE IS HOMING!!!
                            projectileComponent.SetTarget(currentTarget.gameObject);
                        }
                        // Set sorting layer
                        //proj.GetComponent<SpriteRenderer>().sortingLayerName = "Projectiles";

                        // Then take energy
                        this.playerShip.GetInstalledCore().ConsumeEnergy(consumption);
                        this.playerShip.GetInstalledCore().SetIsEnergyUsed(true);
                    }
                }
                
            }

            
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Upon releasing the click allow core to regen again
            this.playerShip.GetInstalledCore().SetIsEnergyUsed(false);
        }
        
    }
    #endregion

    #region Player Dead

    private void SelfDestruct()
    {
        Destroy(this.gameObject);
        // Play VFX
        // Play SFX
        // Do something with game status -> go to main menu, etc.
    }

    #endregion

    private void InitializeScanner()
    {
        this.scannerGameObj.layer = LayerMask.NameToLayer("Player");
    }
}
