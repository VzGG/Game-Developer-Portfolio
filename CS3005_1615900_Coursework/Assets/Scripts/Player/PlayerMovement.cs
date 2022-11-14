using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Movement concept inspired by: 
 * 
 * CasanisPlays (2015) '2D Prototyping in Unity - Tutorial Platformer - Character Movement Code'. [YouTube Tutorial] 4 September.
 * Available at: https://www.youtube.com/watch?v=MvRqEDcJoJQ&list=PL2cNFQAw_ndyKRiobQ2WqVBBBSbAYBobf&index=7
 */
public class PlayerMovement : MonoBehaviour
{
    [Header("Animation Properties")]
    [SerializeField] private Animator animator;                             // SerializeField exposes the variable in the inspector/Unity Editor - set this in the inspector.
    [Space]
    [Header("Physics Properties")]
    [SerializeField] float runningThreshold = 0.01f;                        // The running threshold needed to change the animation from idle to running.
    [SerializeField] float moveSpeed = 5f;                                  // The moveSpeed is used as constant speed increaser when moving the player.
    [SerializeField] float jumpSpeed = 10f;                                 // How high should the player jump.
    [Space]
    [Header("Energy Properties")]
    [SerializeField] Energy myEnergy;
    [SerializeField] float jumpEnergy = 5f;                                 // How much energy is used when jumping.
    [SerializeField] float slideEnergy = 10f;                               // How much energy is used when sliding.
    [Space]
    [Header("Slide Properties")]
    [SerializeField] private float slideSpeed = 5f;                         // How fast is the slide action.
    [SerializeField] private float slideDuration = 0.2f;                    // How long is the player locked in the slide animation.
    [SerializeField] private float slideCooldown = 1f;                      // How long can the player wait for the slide action to be active again.
    [SerializeField] private bool canSlide = true;
    [SerializeField] private bool isSliding = false;

    public bool GetCanSlide() { return this.canSlide; }
    public bool GetIsSliding() { return this.isSliding; }

    #region Player Movements

    // Inspired by... Casanis <add more reference>
    public void Run(Rigidbody2D myRB2D, Animator animator, float moveX)
    {
        // Move the player according to the given movement value.
        myRB2D.velocity = new Vector2(moveX * moveSpeed, myRB2D.velocity.y);

        // Flip the character depending on what we pressed - if left, flip to face right.
        if (moveX > 0)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else if (moveX < 0)
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
     
        // If we move above above the running threshold, change to the running animation.
        if (Mathf.Abs(moveX) >= runningThreshold)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);
    }

    public void Jump(Rigidbody2D myRB2D, Animator animator, Energy myEnergy)
    {
        myRB2D.velocity = new Vector2(myRB2D.velocity.x, jumpSpeed);
        animator.SetTrigger("isJumping 0");
        myEnergy.UseEnergy(jumpEnergy);
    }

    public IEnumerator Slide(Rigidbody2D myRB2D, Animator animator, Energy myEnergy)
    {
        // Move the player forward in its facing direction.
        myRB2D.velocity = new Vector2(slideSpeed * transform.localScale.x, myRB2D.velocity.y);
        canSlide = false;
        isSliding = true;

        // Remove the player's gravity.
        myRB2D.gravityScale = 0;

        // Show slide animation and reduce energy.
        animator.SetTrigger("isSliding2");
        myEnergy.UseEnergy(slideEnergy);

        // Wait for the given duration and continue the code below.
        yield return new WaitForSeconds(slideDuration);

        // Turn off sliding.
        isSliding = false;

        // Add gravity on the player back.
        myRB2D.gravityScale = 1;

        // Player has to wait for the given time before being able to slide again.
        yield return new WaitForSeconds(slideCooldown);

        // At this point the player can slide again.
        canSlide = true;

    }

    #endregion player movement


}
