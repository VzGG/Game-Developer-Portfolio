using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Enemy;
using Oswald.Player;

namespace Oswald.Environment
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private float _trapDamage = 10f;
        [SerializeField] private Vector2 _pushBackForce;
        [SerializeField] private CompositeCollider2D _compositeCollider2D;   // Reference to compositeCollider2D attached to this script

        // Called when something touches the collider of this object - For Spike Tilemap
        private void OnCollisionEnter2D(Collision2D collision)
        {
            bool touchingBody = _compositeCollider2D.IsTouching(collision.gameObject.transform.GetChild(0).GetComponent<CapsuleCollider2D>());
            bool touchingFeet = _compositeCollider2D.IsTouching(collision.gameObject.GetComponent<CapsuleCollider2D>());
            if (touchingBody || touchingFeet)
            {
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(_trapDamage);
                }
            }
        }

        // For Water-Tilemap  - Called once when something enters the trigger of this
        private void OnTriggerEnter2D(Collider2D collision)
        {
            bool touchingFeet;
            bool touchingEnemy;

            touchingFeet = _compositeCollider2D.IsTouching(collision.gameObject.GetComponent<Collider2D>());
            touchingEnemy = _compositeCollider2D.IsTouching(collision.gameObject.GetComponent<Collider2D>());

            if (touchingEnemy)
                if (collision.gameObject.CompareTag("Enemy"))
                    collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(_trapDamage, gameObject, Vector2.zero, false);

            if (touchingFeet)
                if (collision.gameObject.CompareTag("Player"))
                    collision.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(_trapDamage);
        }
    }
}

