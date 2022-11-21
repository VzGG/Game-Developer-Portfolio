using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    // [SerializeField] bool 

    [SerializeField] CapsuleCollider2D myBody2D;
    [SerializeField] Rigidbody2D projectileRB2D;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float bowDamage = 0f;
    [SerializeField] Vector3 playerScale;
    [Header("Pushback properties")]
    [SerializeField] Vector2 firstHitPushback;
    [SerializeField] Vector2 stayHitPushback;
    [Header("VFX Properties")]
    [SerializeField] GameObject arrowHitVFX;

    [SerializeField] PlayerController playerController;

    public enum OnHitEffect
    {
        Pierce,
        NoPierce
    }

    [SerializeField] private OnHitEffect onHitEffect;



    public void SetBowDamage(float bowDamage) { this.bowDamage = bowDamage; }
    public void SetPlayerScale(Vector3 vector3) { playerScale = vector3; }
    public void SetPlayerController(PlayerController playerController) { this.playerController = playerController; }
    public float GetBowDamage() { return bowDamage; }
    public void SetOnHitEffect(OnHitEffect onHit) { this.onHitEffect = onHit; }

    // Start is called before the first frame update
    void Start()
    {
        DetermineOnHitEffect();
        Movement();
    }

    private void DetermineOnHitEffect()
    {
        if (onHitEffect == OnHitEffect.NoPierce)
            myBody2D.isTrigger = false;
        else if (onHitEffect == OnHitEffect.Pierce)
            myBody2D.isTrigger = true;
    }
    
    private void Movement()
    {
        if (playerScale.x >= 1)
        {
            // Move right
            this.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            //projectileRB2D.AddForce(new Vector2(1, 0) * projectileSpeed, ForceMode2D.Impulse);
            projectileRB2D.AddForce(Vector2.right * projectileSpeed, ForceMode2D.Impulse);
        }
        else if (playerScale.x <= -1)
        {
            // Move left
            this.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            //projectileRB2D.AddForce(new Vector2(-1, 0) * projectileSpeed, ForceMode2D.Impulse);
            projectileRB2D.AddForce(Vector2.left * projectileSpeed, ForceMode2D.Impulse);

        }
    }

    #region OnHit effect is Not Pierce, we must use CollisionEnter2D.

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onHitEffect == OnHitEffect.NoPierce)
        {
            //Debug.Log("Collided enter with: " + collision.gameObject);
            if (collision.gameObject.tag.Equals("Enemy"))
            {
                //collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(bowDamage, this.playerController.gameObject, new Vector2(2f, 0.25f));
                collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(bowDamage, this.playerController.gameObject, firstHitPushback);
            }
        }

        // Instantiate arrow hit VFX with its sound FX inside
        Instantiate(arrowHitVFX, transform.position, Quaternion.identity);
        // Then destory this projectile
        Destroy(gameObject);
    }

    #endregion

    #region OnHit effect is Pierce, we must use OnTriggerEnter2D and Exit2D.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (onHitEffect == OnHitEffect.Pierce)
        {
            if (collision.gameObject.tag.Equals("Enemy"))
            {
                //collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(bowDamage, this.playerController.gameObject, new Vector2(2f, 0.25f));
                collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(bowDamage, this.playerController.gameObject, firstHitPushback);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (onHitEffect == OnHitEffect.Pierce)
        {
            //Debug.Log("Collided stay with: " + collision.gameObject);
            if (collision.gameObject.tag.Equals("Enemy"))
            {
                //collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(bowDamage, this.playerController.gameObject, new Vector2(3.5f, 0.25f));
                collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(bowDamage, this.playerController.gameObject, stayHitPushback);
            }
        }
    }

    #endregion



}
