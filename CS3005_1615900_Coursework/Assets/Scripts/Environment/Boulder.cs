using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Concept of IEnumerator and inspired by:
 * 
 * GameDev.tv Team, Davidson, R., Pettie, G. (2019) ‘Complete C# Unity Game Developer 2D – Glitch Garden’ ***2019 Course. 2021 course provides different learning materials*** [Course] 
 * Available at: https://www.udemy.com/course/unitycourse/ 
 */

public class Boulder : MonoBehaviour
{
    [SerializeField] float respawnWait = 5f;

    [SerializeField] SpriteRenderer spriteRenderer;
    Vector2 startingPosition;

    [Space]
    [Header("Boulder damage")]
    [SerializeField] private float trapDamage = 10f;
    [SerializeField] private float trapDamageStay = 1f;
    [SerializeField] private Collider2D collider2D;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingPosition = transform.position;
    }

    private float timeDuringMoving = 0;

    private void Update()
    {
        // Strange bug from the physics of a boulder (near the gate) at longer duration of the game: doesn't move - gets stuck.
        timeDuringMoving += Time.deltaTime;

        // When it has started to move due to physics away from its starting position and 20 seconds have passed, reset this boulder
        if (timeDuringMoving > 20f && transform.position.y < startingPosition.y)
        {
            // Stops getting stuck - may cause weird interaction like boulder starting to disappear in front of the Player
            this.transform.position = startingPosition;
            timeDuringMoving = 0f;

        }
    }

    // move this boulder back to the starting position

    int counter = 0;

    // Called when something touches the collider of this object - For boulderexplode or player
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Works
        if (collision.gameObject.tag == "Player" && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //collision.gameObject.GetComponent<Health>().TakeDamage(trapDamage);
            collision.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(trapDamage);
        }

        // Explosion then reset transform

        // Make a layer that is for the boulder and boulder explode only
        // Boulder explode only interact with boulder, boulder can interact with player, enemy, projectile, ground, and dodge layers
        if (collision.gameObject.tag == "BoulderExplode")
        {
            
            counter++;
            if (counter == 1)
            {
                StartCoroutine(RespawnWait());
            }
        }
    }

    // Only collides with the player's body or feet I think
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //collision.gameObject.GetComponent<Health>().TakeDamage(trapDamageStay);
            collision.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(trapDamageStay);
        }
    }

    IEnumerator RespawnWait()
    {
        // Wait n seconds
        yield return new WaitForSeconds(respawnWait);

        // set the color alpha to 0
        while(spriteRenderer.color.a > 0)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - 0.25f);
            yield return new WaitForSeconds(0.5f);
        }
        // Respawn 
        this.gameObject.transform.position = startingPosition;
        spriteRenderer.color = Color.white;

        counter = 0;
    }
    

}
