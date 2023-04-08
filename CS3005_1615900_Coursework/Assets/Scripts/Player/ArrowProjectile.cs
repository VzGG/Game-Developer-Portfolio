using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Enemy;

namespace Oswald.Player
{
    public class ArrowProjectile : MonoBehaviour
    {
        [SerializeField] CapsuleCollider2D myBody2D;
        [SerializeField] Rigidbody2D projectileRB2D;
        [SerializeField] float projectileSpeed = 5f;
        [SerializeField] float bowDamage = 0f;
        [SerializeField] float noPierceMultiplier = 3f;
        [SerializeField] float pierceMultiplier = 0.75f;
        [SerializeField] Vector3 playerScale;
        [Header("Pushback properties")]
        [SerializeField] Vector2 firstHitPushback;
        [SerializeField] Vector2 stayHitPushback;
        [Header("VFX Properties")]
        [SerializeField] GameObject arrowHitVFX;

        [SerializeField] PlayerController playerController;

        public bool isCritical { private get; set; } = false;

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
        public void SetFirstHitPushback(Vector2 vector2) { this.firstHitPushback = vector2; }
        public void SetStayHitPushback(Vector2 vector2) { this.stayHitPushback = vector2; }

        // Start is called before the first frame update
        void Start()
        {
            DetermineOnHitEffect();
            Movement();
        }

        private void DetermineOnHitEffect()
        {
            if (onHitEffect == OnHitEffect.NoPierce)
            {
                myBody2D.isTrigger = false;
                bowDamage = bowDamage * noPierceMultiplier;
            }
            else if (onHitEffect == OnHitEffect.Pierce)
            {
                bowDamage = bowDamage * pierceMultiplier;
                myBody2D.isTrigger = true;
            }
                
        }

        private void Movement()
        {
            if (playerScale.x >= 1)
            {
                // Move right
                this.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                projectileRB2D.AddForce(Vector2.right * projectileSpeed, ForceMode2D.Impulse);
            }
            else if (playerScale.x <= -1)
            {
                // Move left
                this.transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                projectileRB2D.AddForce(Vector2.left * projectileSpeed, ForceMode2D.Impulse);

            }
        }

        #region OnHit effect is Not Pierce, we must use CollisionEnter2D.

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (onHitEffect == OnHitEffect.NoPierce)
            {
                if (collision.gameObject.tag.Equals("Enemy"))
                {
                    collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(bowDamage, this.playerController.gameObject, firstHitPushback, isCritical);
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
                    collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(bowDamage, this.playerController.gameObject, firstHitPushback, isCritical);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (onHitEffect == OnHitEffect.Pierce)
            {
                if (collision.gameObject.tag.Equals("Enemy"))
                {
                    collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(bowDamage, this.playerController.gameObject, stayHitPushback, false);
                }
            }
        }

        #endregion
    }
}

