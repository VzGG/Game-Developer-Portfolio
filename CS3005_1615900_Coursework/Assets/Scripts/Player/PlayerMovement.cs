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
    // Another way to get a reference to the animator is to drag the animator component in this field in the editor
    // SerializeField exposes this variable in the editor regardless of being private or public
    [Header("Animation Properties")]
    [SerializeField] private Animator animator;
    [SerializeField] private bool isFacingRight = true;
    // [SerializeField] private bool isJumping = false;
    [Space]
    [Header("Physics Properties")]
    [SerializeField] Rigidbody2D myRB2D;
    [SerializeField] float runningThreshold = 0.01f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] Collider2D myCollider2D;
    [SerializeField] Vector2 slideForce;

    [Space]
    [Header("Energy Properties")]
    [SerializeField] Energy myEnergy;
    [SerializeField] float jumpEnergy = 5f;
    [SerializeField] float slideEnergy = 10f;
    [SerializeField] GameObject body;
    [Space]
    [Header("Health Properties")]
    [SerializeField] Health myHealth;
    [Space]
    [Header("Time Manager")]
    [SerializeField] TimeManager timeManager;

    [Header("Sliding Properties")]
    [SerializeField] SpriteRenderer spriteRenderer;

    private bool isSliding = false;

    private bool slidePressed = false;
    // Get the playerAttack scripts reference


    // Use this in next level change 
    public void SetMyTimeManager(TimeManager timeManager) { this.timeManager = timeManager; }
    // The methods are made like this as the editor (animation event) cannot take in boolean parameters, but can for others like int and float, etc.

    #region Player Movements



    // Inspired by... Casanis <add more reference>
    public void Run()
    {
        // If we press any of the left, right arrow key or the A or D keys then we get a value here between -1 to 1
        float moveX = Input.GetAxis("Horizontal");
        // Move our player either positively or negatively
        myRB2D.velocity = new Vector2(moveX * moveSpeed, myRB2D.velocity.y);
        // We will get a negative 1 if we press left, so we must use its absolute value
        // If we start moving, we should also set the animation to running
        if (Mathf.Abs(moveX) >= runningThreshold)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);

        // Flip the character depending on what we pressed - if left, flip to face right
        if (moveX > 0)
            isFacingRight = true;
        else if (moveX < 0)
            isFacingRight = false;

        if (isFacingRight)
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
    }

    // Inspired by... <add reference>
    public void Jump(Energy myEnergy)
    {
        // If we press jump and is touching the ground - jump
        // Uses energy - We can only jump when we have energy
        float jumpCounter = 0;
        if (Input.GetKey(KeyCode.Space) && myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && myEnergy.GetEnergy() > 0)
        {
            jumpCounter++;
            // Jump
            myRB2D.velocity = new Vector2(myRB2D.velocity.x, jumpSpeed);
            // Only here that we take energy away * Time.deltaTime for smoothening
            // myEnergy.UseEnergy(jumpEnergy * Time.deltaTime);
            animator.SetTrigger("isJumping 0");
            if (jumpCounter == 1)
            {
                // Prevents use energy more than once when we jump
                // Get Key is registered more than once
                myEnergy.UseEnergy(jumpEnergy);
                jumpCounter = 0;
                
            }
            
        }
    }

    public bool SlidePressed(Energy myEnergy)
    {
        // Get left shift input status
        slidePressed = Input.GetKey(KeyCode.LeftShift);
        // If we have energy, we should be able to slide
        if (myEnergy.GetEnergy() > 0f)
        {
            if (slidePressed)
            {
                // Sliding shows the sliding animation, also takes energy, and allows to "Phase" through enemies using the layering and collision matrix
                animator.SetTrigger("isSliding2");
                myEnergy.UseEnergy(slideEnergy * Time.deltaTime * 30f);
                SetLayer("Dodge");

                
            }
            else
            {
                // Set the player back to player to not "Phase" through enemies
                SetLayer("Player");
                // Allows the energy to start recovering
                myEnergy.SetIsEnergyBeingUsed(false);
            }
        }
        else
        {
            SetLayer("Player");
            // Stops us from sliding when we have 0 energy
            slidePressed = false;
        }
        return slidePressed;
    }

    // Dodge ability - to implement iframe
    public void SlideNew()
    {
        if (slidePressed)
        {
            if (isFacingRight)
            {
                myRB2D.AddForce(myRB2D.velocity + new Vector2(50f, 0f));
            }
            else
            {
                myRB2D.AddForce(myRB2D.velocity + new Vector2(-50f, 0f));
            }
                
        }

    }

    #endregion

    private void SetLayer(string layerName)
    {
        this.gameObject.layer = LayerMask.NameToLayer(layerName); // Gets the layer number of the layer mask Dodge and sets the Character to that or to default - essentially the iFrame but not complete yet
        body.gameObject.layer = LayerMask.NameToLayer(layerName); // Fixes the layer collision matrix not working - idea by: https://forum.unity.com/threads/collision-matrix-not-working.904460/
    }

}
