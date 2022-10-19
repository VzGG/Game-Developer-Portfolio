using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage = 0f;
    [SerializeField] float projectileSpeed = 30f;
    [SerializeField] Rigidbody2D rb2d;

    [SerializeField] int weaponType = -1;
    // For homing movement - for player only
    [SerializeField] GameObject target;
    [SerializeField] bool isPlayer = false;
    string layerName = "";

    public void SetLayerName(string layerName) { this.layerName = layerName; }
    public void SetDamage(float damage) { this.damage = damage; }
    public void SetProjectileSpeed(float speed) { this.projectileSpeed = speed; }
    public void SetWeaponType(int weaponType) { this.weaponType = weaponType; }
    public void SetTarget(GameObject target) { this.target = target; }
    public void SetIsPlayerStatus(bool status) { this.isPlayer = status; }
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();


        // If the owner is the player it gains 50% damage increase
        if (weaponType == 0 && this.isPlayer == true)
        {
            // If weapon type is NON-HOMING -> INCREASE DAMAGE BY 100%
            this.damage = this.damage * 2f;
        }
        // Otherwise do nothing

        SelfDestruct();
    }

    private void Update()
    {
        if (weaponType == 0)
        {
            StraightMovement();
        }
        else if (weaponType == 1)
        {
            HomingMovement();
        }
    }

    private void StraightMovement()
    {
        transform.position += transform.up * projectileSpeed * Time.deltaTime;
    }

    private void HomingMovement()
    {
        if (target == null) { Debug.Log("There is no target."); return; }

        // Rotate to the given target
        Vector3 direction = target.transform.position - this.transform.position;
        float rotateSpeed = 45f;                                // How fast it rotates 30-45 is good
        float rotationModifier = 90f;                           // Points with the green axis - z axis 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - rotationModifier;
        // The quaternion needed to rotate to the given angle
        Quaternion targetQuaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        // Now rotate the projectile to the target
        // RotateTowards - rotate in constant speed
        // Slerp - rotate in a given time
        //this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetQuaternion, Time.deltaTime * rotateSpeed);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetQuaternion, 0.5f);

        // Move forward and towards green axis
        transform.position += transform.up * projectileSpeed * Time.deltaTime;
    }

    void SelfDestruct()
    {
        // Then destroy
        Destroy(gameObject, 15f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // takes damage to whoever it was
        //Debug.Log(collision.gameObject.name);

        bool isPlayer = collision.gameObject.TryGetComponent(out PlayerController player);
        bool isEnemy = collision.gameObject.TryGetComponent(out EnemyController enemy);
        bool isAsteroid = collision.gameObject.TryGetComponent(out Asteroid asteroid);


        if (isPlayer == true)
        {
            // Player takes armour damage if there is armour value otherwise take direct damage to hull
            player.PlayerTakeDamage(this.damage);  
        }
        else if (isEnemy == true)
        {
            // Enemy takes damage
            enemy.EnemyTakeDamage(this.damage);
        }
        else if (isAsteroid == true)
        {
            //Debug.Log("Projectile collided with: " + asteroid.gameObject.name);
            asteroid.TakeDamage(this.damage);
        }

        // Then destroy itself
        Destroy(this.gameObject);
    }
}
