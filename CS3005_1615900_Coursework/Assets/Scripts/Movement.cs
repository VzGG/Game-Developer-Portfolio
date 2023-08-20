using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] private float _runningThreshold = 0.01f;

    public void Move(Rigidbody2D myRB2D, AnimatorController animator, float moveX, string animParamName, float xScale = 1)
    {
        myRB2D.velocity = new Vector2(moveX * moveSpeed, myRB2D.velocity.y);

        // Flip the character depending on what we pressed - if left, flip to face right.
        if (moveX > 0)
            // We use xScale instead of transform.localScale as this will make the character flip constantly
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        else if (moveX < 0)
            transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);


        if (animParamName.Equals(string.Empty)) { return; }
        if (Mathf.Abs(moveX) >= _runningThreshold)
        {
            animator.ChangeAnimationBool(animParamName, true);
        }   
        else
        {
            animator.ChangeAnimationBool(animParamName, false);
        }
    }
}