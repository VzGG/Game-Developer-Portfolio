using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Animation Event concept, tutorial and inspired by:
 * 
 * GameDev.tv Team, Davidson, R., Pettie, G. (2019) ‘Complete C# Unity Game Developer 2D – Glitch Garden’ ***2019 Course. 2021 course provides different learning materials*** [Course] 
 * Available at: https://www.udemy.com/course/unitycourse/ 
 */

public class EvilWizardController : MonoBehaviour
{
    private float timeToAttack = 5f;
    private float time = 0f; 
    public Animator animator;                                   // To change animation
    public PlayerMovement target = null;                        // Target
    public bool isFacingRight = true;                                  // Change x scale depending on where the enemy is
    public float scaleX = 0.35f;
    public EnemyArrowProjectile enemyArrowProjectileGameObject;
    public Health enemyHealth;
    /*
     * States of Wizard:
     * 
     * 1. Idle
     * 2. Attack
     * 3. Death
     * 
     * When hit just set the color to something and revert back after
     */





    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.GetHealth() <= 0) { return; }

        Attack();

        // If no target
        // Look at right all the time
        // If there is target look either left or right depending on target position
        if (target == null)
        {
            return;
        }
        else
        {
            if (this.transform.position.x > target.transform.position.x)
            {
                isFacingRight = false;
            }
            else if (this.transform.position.x < target.transform.position.x)
            {
                isFacingRight = true;

            }
        }

        if (isFacingRight)
        {// Look right
            this.transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.y);
        }
        else if (!isFacingRight)
        {// Look left
            this.transform.localScale = new Vector3(-scaleX, transform.localScale.y, transform.localScale.y);
        }


    }

    void Attack()
    {
        time += Time.deltaTime;

        if (time > timeToAttack)
        {
            // Set animator
            animator.SetTrigger("isAttacking");
            // Reset time
            time = 0f;
        }
    }

    void AnimationEventAttack()
    {
        enemyArrowProjectileGameObject.SetEvilWizardController(this);
        enemyArrowProjectileGameObject.SetScale(this.transform.localScale);
        GameObject proj = Instantiate(enemyArrowProjectileGameObject.gameObject, transform.position, Quaternion.identity);
        
        // Spawn an arrow projectile
    }

    // This taking damage from projectile
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectile") && collision.gameObject.tag == "Player")
        {

            // Debug.Log("Hit by: " + collision.gameObject.name);
            ArrowProjectile arrowProjectile = collision.gameObject.GetComponent<ArrowProjectile>();
            enemyHealth.TakeDamage(arrowProjectile.GetBowDamage());
        }
    }


}
