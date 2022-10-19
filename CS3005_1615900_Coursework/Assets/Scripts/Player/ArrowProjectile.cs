using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    // [SerializeField] bool 

    [SerializeField] Rigidbody2D projectileRB2D;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float bowDamage = 0f;
    [SerializeField] Vector3 playerScale;
    [Space]
    [Header("VFX Properties")]
    [SerializeField] GameObject arrowHitVFX;
    public void SetBowDamage(float bowDamage) { this.bowDamage = bowDamage; }
    public void SetPlayerScale(Vector3 vector3) { playerScale = vector3; }

    public float GetBowDamage() { return bowDamage; }

    // Start is called before the first frame update
    void Start()
    {
        if (playerScale.x >= 1)
        {
            // Move right
            this.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            projectileRB2D.AddForce(new Vector2(1, 0) * projectileSpeed, ForceMode2D.Impulse);
        }
        else if (playerScale.x <= -1)
        {
            // Move left
            this.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            projectileRB2D.AddForce(new Vector2(-1, 0) * projectileSpeed, ForceMode2D.Impulse);
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Instantiate arrow hit VFX with its sound FX inside
        Instantiate(arrowHitVFX, transform.position, Quaternion.identity);
        // Then destory this projectile
        Destroy(gameObject);
    }

}
