using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class that holds and activates the enemyship (Data) class. Like MVC design pattern in a way.
/// </summary>
public class EnemyController : MonoBehaviour
{
    [SerializeField] float armourDefense = 0f;  // DELETE LATER
    [SerializeField] float hullHP = 0f;         // DELETE LATER
    [SerializeField] float energyMP = 0f;       // DELETE LATER
    [SerializeField] float normalSpeed = 0f;    // DELETE LATER
    [SerializeField] float weight = 0f;         // DELETE LATER
    [SerializeField] float evasion = 0f;        // DELETE LATER

    [SerializeField] private EnemyShip enemyShip;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] GameObject projectile;

    public void SetEnemyShip(EnemyShip enemyShip) { this.enemyShip = enemyShip; }
    public void SetRB2D(Rigidbody2D rigidbody2D) { this.rb2d = rigidbody2D; }
    public void SetProjectile(GameObject projectile) { this.projectile = projectile; }
    public EnemyShip GetEnemyShip() { return this.enemyShip; }
    public void SetScanner(GameObject scanner) { this.scannerGameObj = scanner; }
    public void SetPlayerController(PlayerController playerController) { this.playerController = playerController; }
    
    // Disable or Enable enemy's hp regen - called by the ship spawner
    public void SetEnemyRegenOnStatus(bool status)
    {
        this.enemyShip.GetInstalledFrame().SetIsRegenOn(status);
    }
   
    // The enemy's target
    [SerializeField] private PlayerController playerController;

    [SerializeField] private GameObject scannerGameObj;

    float currentTime = 0f;

    private void Start()
    {

        //this.projectile.GetComponent<Projectile>().SetProjectileSpeed(30f);

        InitializeScanner();

        // Initialize modified values and apply to enemy controller
        this.enemyShip.GetInstalledWeapons()[0].ApplyValueStatModifier();
        this.enemyShip.GetInstalledArmour().ApplyValueStatModifier();
        this.enemyShip.GetInstalledFrame().ApplyValueStatModifier();
        this.enemyShip.GetInstalledCore().ApplyValueStatModifier();
        this.enemyShip.GetInstalledBooster().ApplyValueStatModifier();

        // Add the weight of weapons AND weights from other components
        for (int i = 0; i < this.enemyShip.GetInstalledWeapons().Count; i++)
        {
            weight += enemyShip.GetInstalledWeapons()[i].GetWeight();
        }
        weight += enemyShip.GetInstalledArmour().GetWeight();
        weight += enemyShip.GetInstalledBooster().GetWeight();
        weight += enemyShip.GetInstalledFrame().GetWeight();
        weight += enemyShip.GetInstalledCore().GetWeight();

       


        // Initialize the maxAttackCounter randomly
        maxAttackCounter = Random.Range(5, 15+1);

    }

    // Update is called once per frame
    void Update()
    {
        this.armourDefense = this.enemyShip.GetInstalledArmour().GetCurrentArmour(); // DELETE LATER
        this.hullHP = this.enemyShip.GetInstalledFrame().GetCurrentHull();           // DELETE LATER
        this.energyMP = this.enemyShip.GetInstalledCore().GetCurrentEnergy();        // DELETE LATER
        this.normalSpeed = this.enemyShip.GetInstalledBooster().GetModifiedValues()[0]; // DELETE LATER
        
        this.evasion = this.enemyShip.GetInstalledBooster().GetModifiedValues()[1];  // DELETE LATER

        // Initialise its vitality
        this.enemyShip.GetInstalledFrame().ComponentActive();
        this.enemyShip.GetInstalledCore().ComponentActive();
        this.enemyShip.GetInstalledArmour().ComponentActive();

        // Don't attack and rotate to target when there is no target
        if (playerController == null) { return; }

        // Deciding factor to destroy enemy or not
        bool isEnemyDead = this.enemyShip.GetInstalledFrame().GetIsFrameDestroyed();

        if (isEnemyDead)
        {
            Debug.Log("Enemy dead");
            SelfDestruct();
            this.enabled = false;
            FindObjectOfType<ProgressionManager>().IncreaseCurrentDestroyed();
            return;
        }



        // Enemy AI
        /*
         * Follow player, rotate while following
         * Attack while player is in RANGE
         * Attack using energy and attacks in intervals
         */
        FollowTarget();
        RotateToTarget();
        //Attack();


        // If energy is depleted, wait until energy percentage is at 35% or greater
        if (energyDepleted == true)
        {
            if (this.enemyShip.GetInstalledCore().GetCurrentEnergyPerecentage() >= 0.35f)
            {
                energyDepleted = false;
                maxAttackCounter = Random.Range(5, 15 + 1);
            }

            return;
        }// If <bool> is true, do not attack and use a timer and reset
        else if (attackCounterExceeded == true)
        {
            // Do no attack by RETURNING and increase timer of wait time, only then we change the attack counter to false
            currentWaitTime += Time.deltaTime;

            // When cooldown is finished THEN enemy can start attacking again AND DECLARE NEW attack pattern
            if (currentWaitTime > waitTimeForNextAttackCombo)
            {
                attackCounterExceeded = false;
                maxAttackCounter = Random.Range(5, 15 + 1);
                //Debug.Log("New max attack interval: " + maxAttackCounter);
                currentWaitTime = 0f;
                return;
            }

            return;
        }
         

        EnemyAttackPattern();

    }

    private void RotateToTarget()
    {
        // Rotate to player - BUT STILL SNAPS to player when we move fast
        Vector3 direction = playerController.transform.position - this.transform.position;
        float rotateSpeed = 45f;                   // How fast it rotates 30-45 is good
        float rotationModifier = 90f;        // Points with the green axis - z axis 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - rotationModifier;
        Quaternion targetQuaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetQuaternion,Time.deltaTime * speed);
        // Quaternion Slerp - rotates in a time limit
        // Quaternion Rotate Towarwds - rotates in a fixed constant speed (OUR DESIRED ROTATION)!!!
        // Read: https://www.youtube.com/watch?v=Esz2MqxhNig - RotateToward vs Slerp Chapter
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetQuaternion, Time.deltaTime * rotateSpeed);
    }


    // Attack pattern properties
    int currentAttackCounter = 0;
    int maxAttackCounter = 0;
    bool attackCounterExceeded = false;
    float currentWaitTime = 0f;
    float waitTimeForNextAttackCombo = 5f;
    bool energyDepleted = false;
    // 


    private void EnemyAttackPattern()
    {
        // At every 30%? (or 15-30%) cumulative energy consumption, wait for a bit (3-5 seconds)
        // Or at every 5-15 cumulative attacks, wait for a few seconds, then restart attacking, then declare new attack combo counter
        // Then proceed attacking
        // Only attacks when there is energy
        // When depleted, wait until energy is at 50%


        // Cannot attack when there is no weapon equipped/no name
        if (string.IsNullOrEmpty(this.enemyShip.GetInstalledWeapons()[0].GetName())) { return; }

        //float weaponConsumption = this.enemyShip.GetInstalledWeapons()[0].GetValues()[2];

        // Attack properties
        //float weaponDamage = this.enemyShip.GetInstalledWeapons()[0].GetModifiedValues()[0];
        //float weaponFireRate = this.enemyShip.GetInstalledWeapons()[0].GetModifiedValues()[1];
        //float weaponConsumption = this.enemyShip.GetInstalledWeapons()[0].GetModifiedValues()[2];

        float weaponDamage = this.enemyShip.GetInstalledWeapons()[0].GetPower();
        float weaponFireRate = this.enemyShip.GetInstalledWeapons()[0].GetFireRate();
        float weaponConsumption = this.enemyShip.GetInstalledWeapons()[0].GetEnergyConsumption();

        // If it runs out of energy in calculation cost of attack, then wait until energy is at 30-50%
        if (this.enemyShip.GetInstalledCore().CanConsumeEnergy(weaponConsumption) == false)
        {
            // reset attack counter and attack again after a few seconds
            
            currentAttackCounter = 0;
            energyDepleted = true;
            this.enemyShip.GetInstalledCore().SetIsEnergyUsed(false);
            return;
        }

        // ATTACKING
        if (this.enemyShip.GetInstalledCore().GetCurrentEnergy() > 0f)
        {
            // Method requires passing from weapon energy consumption
            // Can only use energy when energy given is subtracted to the current energy is greater than ZERO
            if (this.enemyShip.GetInstalledCore().CanConsumeEnergy(weaponConsumption))
            {
                // The attack pattern

                // Cannot ATTACK when attack counter is > than max attack counter
                if (currentAttackCounter > maxAttackCounter)
                {
                    // Do no attack, and wait for a few seconds
                    // USE BOOL to wait for few seconds
                    // THEN change the maxAttackCounter to something different
                    attackCounterExceeded = true;
                    currentAttackCounter = 0;
                    this.enemyShip.GetInstalledCore().SetIsEnergyUsed(false);
                    return;
                }

                Attack(weaponDamage, weaponFireRate, weaponConsumption);

                
            }
        }
        

    }

    private void Attack(float weaponDamage, float weaponFireRate, float weaponConsumption)
    {
        string weaponName = this.enemyShip.GetInstalledWeapons()[0].GetName();

        if (string.IsNullOrEmpty(weaponName)) { 
            //Debug.Log("Enemy has melee, cannot attack at the moment... ADD VALUES TO WEAPONS... and animation next time...." + 
            //"\nEnemy name: " + this.gameObject.name);  
            return; }

        //float enemyDamage = this.enemyShip.GetInstalledWeapons()[0].GetValues()[0];
        //float fireRate = this.enemyShip.GetInstalledWeapons()[0].GetValues()[1];

        // Fire projectiles
        //this.enemyShip.GetInstalledWeapons()[0].ComponentActive();

        int weaponType = (int) this.enemyShip.GetInstalledWeapons()[0].GetValues()[3];

        currentTime += Time.deltaTime;

        if (currentTime > weaponFireRate)
        {
            currentTime = 0f;

            // Create projectile gameobject and move towards the player
            GameObject proj = Instantiate(projectile, transform.position, this.transform.rotation);
            Projectile projectileComponent = proj.GetComponent<Projectile>();
            // projectileComponent.SetLayerName("Enemy");
            //projectileComponent.SetDamage(enemyDamage);
            projectileComponent.SetDamage(weaponDamage);

            // The value multiplier FOR EACH ship component is DIFFERENT
            //Debug.Log("Enemy weapon damage: " + weaponDamage 
            //    + " | this wep multiplier: " + this.enemyShip.GetInstalledWeapons()[0].GetValueMultiplier()  
            //    + " | armour multiplier: " + this.enemyShip.GetInstalledArmour().GetValueMultiplier() 
            //    + " | core multiplier" + this.enemyShip.GetInstalledCore().GetValueMultiplier() 
            //    + " | ooster multiplier: " + this.enemyShip.GetInstalledBooster().GetValueMultiplier()
            //    + " | frame multiplier: " + this.enemyShip.GetInstalledFrame().GetValueMultiplier());
            // Make the enemy projectile weapon type a NON-HOMING MOVEMENT
            projectileComponent.SetWeaponType(0);


            // TESTING FOR NOW _ DELETE LATER AFTER FINISHED WITH EXPERIMENT
            //projectileComponent.SetWeaponType(weaponType);
            //// Set target
            //projectileComponent.SetTarget(playerController.gameObject);
            // WORKS



            // Then take energy 
            this.enemyShip.GetInstalledCore().ConsumeEnergy(weaponConsumption);
            this.enemyShip.GetInstalledCore().SetIsEnergyUsed(true);

            // Add attack counter for attack intervals
            currentAttackCounter += 1;
        }



    }

    private void InitializeScanner()
    {
        scannerGameObj.layer = LayerMask.NameToLayer("Enemy");
    }

    private void FollowTarget()
    {
        float distance = Vector2.Distance(this.transform.position, playerController.transform.position);

        // Debug.Log("target distance: " + distance);

        // If target hit by a dodge boost

        //float boosterSpeed = enemyShip.GetInstalledBooster().GetValues()[0];
        //float boosterSpeed = enemyShip.GetInstalledBooster().GetModifiedValues()[0];
        float boosterSpeed = enemyShip.GetInstalledBooster().GetBoostSpeed();

        if (distance > 10f)
        {
            //Debug.Log("Velocity: " + rb2d.velocity);
            // Move to target
            // rb2d.velocity = transform.up * boosterSpeed;
            transform.position += transform.up * boosterSpeed * Time.deltaTime;
            //Debug.Log("Velocity after: " + rb2d.velocity);
        }

        //Debug.Log("Velocity after if statement exec: " + rb2d.velocity);


        // If enemy is within 20f distance and greater, go boost speed, if distance is less than 20f 
    }

    // Called by projectile class
    public void EnemyTakeDamage(float damage)
    {
        float armour = enemyShip.GetInstalledArmour().GetCurrentArmour();
        if (armour > 0f)
            enemyShip.GetInstalledArmour().TakeArmourDamage(damage);
        else
        {
            // CHECK THE EVASION FIRST!!! - the values is around 0.00-0.45 -> 0% to 45% evasion
            //float evasion = this.enemyShip.GetInstalledBooster().GetModifiedValues()[1];
            //float evasion = this.enemyShip.GetInstalledBooster().GetEvasion();

            //float randomNumber0to1 = Random.Range(0f, 1f);
            ////Debug.Log("enemy Evasion: " + evasion + "\n random number...: " + randomNumber0to1);
            //// if (canEvade)
            //if (randomNumber0to1 > evasion)
            //{
            //    // Take damage in the frame (HP)
            //    enemyShip.GetInstalledFrame().TakeDamage(damage);
            //}
            //else
            //{
            //    Debug.Log("EVADING");
            //}

            // Remove evasion for now - enemies do not have any evasion - just the player
            enemyShip.GetInstalledFrame().TakeDamage(damage);
        }
            
    }


    private void SelfDestruct()
    {
        Destroy(this.gameObject);

        // Play sfx
        // play vfx
    }

}
