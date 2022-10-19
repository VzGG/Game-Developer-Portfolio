using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformElevator : MonoBehaviour
{
    [SerializeField] Vector2 yLimitUp;
    [SerializeField] Vector2 yLimitDown;
    [SerializeField] float moveY = 5f;
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] Rigidbody2D rb2d;

    [SerializeField] bool movingUp = true;
    [SerializeField] float time = 0f;

    private void FixedUpdate()
    {
        if (movingUp)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, moveY * moveSpeed);
        }
        else if (!movingUp)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -moveY * moveSpeed);
        }

        if (rb2d.position.y > yLimitUp.y)
        {
            movingUp = false;
        }
        else if (rb2d.position.y < yLimitDown.y)
        {

            movingUp = true;
        }


    }
}
