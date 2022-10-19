using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] Vector2 randomMove;
    [SerializeField] float randomRotateZ;

    [SerializeField] Rigidbody2D rigidbody2D = null;

    [SerializeField] float moveLowerLimit = -0.5f;
    [SerializeField] float moveUpperLimit = 0.5f;

    // For Moving asteroids
    [SerializeField] bool hasHealth = false;
    [SerializeField] float health = 7500f;

    public void SetHasHealth(bool status) { this.hasHealth = status; }
    private void Start()
    {
        // No moving or anything needed when asteroid is static
        if (rigidbody2D.bodyType == RigidbodyType2D.Static) { return; }

        float randomMoveX = Random.Range(moveLowerLimit, moveUpperLimit);
        float randomMoveY = Random.Range(moveLowerLimit, moveUpperLimit);

        randomMove = new Vector2(randomMoveX, randomMoveY);

        randomRotateZ = Random.Range(-360f, 360f);

        // Add movement
        rigidbody2D.AddForce(randomMove, ForceMode2D.Impulse);

        rigidbody2D.MoveRotation(randomRotateZ);
    }

    public void SetLowerLimit(float lowerLimit) { this.moveLowerLimit = lowerLimit; }
    public void SetUpperLimit(float upperLimit) { this.moveUpperLimit = upperLimit; }
    public void TakeDamage(float damage) 
    {
        if (hasHealth == false) { Debug.Log("Asteroid has no health, cannot damage it."); return; }

        this.health -= damage;

        if (this.health <= 0f)
        {
            // Destroy itself
            Destroy(this.gameObject);
            // Provide ITEM to player
            Debug.Log("PROVIDE ITEM TO THE PLAYER!!!");
        }
    }

}
