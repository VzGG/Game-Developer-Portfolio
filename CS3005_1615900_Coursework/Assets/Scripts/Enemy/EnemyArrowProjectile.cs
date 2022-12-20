using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oswald.Enemy
{
    public class EnemyArrowProjectile : MonoBehaviour
    {
        [SerializeField] Rigidbody2D projectileRB2D;
        [SerializeField] float projectileSpeed = 5f;
        [SerializeField] float damage = 15f;
        //[SerializeField] EvilWizardController evilWizardController;
        [SerializeField] Vector3 scale;
        [Header("VFX Properties")]
        [SerializeField] GameObject arrowHitVFX;

        public float GetDamage() { return damage; }

        public void SetScale(Vector3 scale) { this.scale = scale; }
        //public void SetEvilWizardController(EvilWizardController evilWizardController) { this.evilWizardController = evilWizardController; }

        // Start is called before the first frame update
        void Start()
        {


            if (scale.x >= 0.35f)
            {
                // Move right
                this.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                projectileRB2D.AddForce(new Vector2(1, 0) * projectileSpeed, ForceMode2D.Impulse);
            }
            else if (scale.x <= -0.35f)
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
}


