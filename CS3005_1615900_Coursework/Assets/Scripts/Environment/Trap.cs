using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float trapDamage = 10f;
    [SerializeField] Vector2 pushBackForce;

    [SerializeField] private CompositeCollider2D compositeCollider2D;   // Reference to compositeCollider2D attached to this script


    // Called when something touches the collider of this object - For Spike Tilemap
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("caller: " + this.gameObject.name);

        // Touching the body capsule collider of the Character
        bool touchingBody = compositeCollider2D.IsTouching(collision.gameObject.transform.GetChild(0).GetComponent<CapsuleCollider2D>());
        bool touchingFeet = compositeCollider2D.IsTouching(collision.gameObject.GetComponent<CapsuleCollider2D>());
        if (touchingBody || touchingFeet)
        {
            if (collision.gameObject.tag == "Player")
            {
                //collision.gameObject.GetComponent<Health>().TakeDamage(trapDamage);
                collision.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(trapDamage);
            }
        }
    }

    // For Water-Tilemap  - Called once when something enters the trigger of this
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // bool touchingBody = compositeCollider2D.IsTouching(collision.gameObject.transform.GetChild(0).GetComponent<CapsuleCollider2D>());
        // Debug.Log("Touching body: " + touchingBody);
        bool touchingFeet = false;
        bool touchingEnemy = false;
        try
        {
            touchingFeet = compositeCollider2D.IsTouching(collision.gameObject.GetComponent<CapsuleCollider2D>());
            touchingEnemy = compositeCollider2D.IsTouching(collision.gameObject.GetComponent<CircleCollider2D>());
        }
        catch (ArgumentNullException e)
        {
            Debug.LogError("Error found!!!: "+ e);
        }
        catch (NullReferenceException e)
        {
            Debug.LogError("Error found!!!: " + e);
        }

        // If this is the player
        if (touchingFeet)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(trapDamage);
            }
        }
        
        if (touchingEnemy)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyController>().EnemyTakeDamage(trapDamage, gameObject.tag);
            }
        }

    }
}
