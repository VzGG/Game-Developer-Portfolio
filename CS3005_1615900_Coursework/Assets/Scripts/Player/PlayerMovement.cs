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
    private PlayerAttack playerAttack;


    // Use this in next level change 
    public void SetMyTimeManager(TimeManager timeManager) { this.timeManager = timeManager; }
    // The methods are made like this as the editor (animation event) cannot take in boolean parameters, but can for others like int and float, etc.
    public void SetIsSlidingTrue() { this.isSliding = true; }
    public void SetIsSlidingFalse() { this.isSliding = false; }
    public bool GetIsSliding() { return this.isSliding; }

    // Singleton - only one is alive
    // And don't destroy this on next level
    private void Awake()
    {

        int numberOfCharacters = FindObjectsOfType<PlayerMovement>().Length;
        if (numberOfCharacters > 1)
        {
            Destroy(this.gameObject);
        }
        else
            DontDestroyOnLoad(this.gameObject);

        // Set the reference
        playerAttack = GetComponent<PlayerAttack>();
        // Pass this class as the reference for the PlayerAttack to use
        playerAttack.SetPlayerMovement(this);

    }

    // Should put any input events here
    private void Update()
    {
        // Cannot slide when we are in attacking animation
        if (playerAttack.GetIsAttacking()) { return; }

        SlidePressed();
    }



    // Should put any physics related events here like RigidBody component BUT not transform component movements
    void FixedUpdate()
    {
        if (myHealth.GetHealth() <= 0) { return; }          // Stops movement

        if (timeManager == null) { return; }

        // French, J. (2020). ‘The right way to pause a game in Unity’. [Blog]. 13 February. Available at:
        // https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/#exclude_objects_from_pause
        if (timeManager.GetIsTimeStopped() == true) {
            // Debug.Log("Attempting to move in a pause state");
            return; } // If time is stopped (true), stop any movement


        // When the player uses any attacking animation, stop any movement below
        if (playerAttack.GetIsAttacking()) { return; }


        
        Run();
        // To Do - when we slide and jump, but not run, we should disable attacking/left click or right clicks from executing to stop having attack "buffers"
        Jump();
        SlideNew();
        //SlideOld();
    }
    #region Player Movements



    // Inspired by... Casanis <add more reference>
    private void Run()
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
    private void Jump()
    {
        // If we press jump and is touching the ground - jump
        // Uses energy - We can only jump when we have energy
        // if (Input.GetKey(KeyCode.Space) && myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && myEnergy.CanUseEnergy(jumpEnergy))

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
 
        // Jump animation when not touching the ground
/*        if (!myCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            animator.SetTrigger("isJumping 0");
        }  */ 
    }

    private void SlidePressed()
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
    }

    private void SlideNew()
    {
        if (slidePressed)
        {
            if (isFacingRight)
                myRB2D.AddForce(myRB2D.velocity + new Vector2(50f, 0f));
            else
                myRB2D.AddForce(myRB2D.velocity + new Vector2(-50f, 0f));
        }

    }

    // Dodge ability - to implement iframe
    private void SlideOld()
    {
        if (myEnergy.GetEnergy() > 0)
        {
            // Cannot use GetKeyUp or GetKeyDown in FixedUpdate as this gets called multiple times per frame if needed
                        // NEED TO MAKE SURE THIS IS ONLY CALLED ONCE!!!
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // When sliding, in the animation tab itself, one of its frame should set the isSliding to true
                // This is to stop attack animation buffers
                animator.SetBool("isSliding", true);
                if (isFacingRight)
                {
                    // myRB2D.AddForce(slideForce, ForceMode2D.Impulse);
                    float slideSpeed = 5;
                    // Do it like the jump movement
                    myRB2D.velocity = new Vector2(slideSpeed, myRB2D.velocity.y);
                }
                else
                {
                    // myRB2D.AddForce(-slideForce, ForceMode2D.Impulse);
                    float slideSpeed = 5;
                    // Do it like the jump movement
                    myRB2D.velocity = new Vector2(-slideSpeed, myRB2D.velocity.y);
                }
                // myEnergy.UseEnergy(slideEnergy * Time.deltaTime);
                myEnergy.UseEnergy(slideEnergy);
                SetLayer("Dodge");

                // Make mental image of dodging provides I-frames
                //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);         
            }
            else
            {
                animator.SetBool("isSliding", false);
                SetLayer("Player");                     // Instead of default
                myEnergy.SetIsEnergyBeingUsed(false);
                /* this.spriteRenderer.color = new Color(1f, 1f, 1f, 1f);*/

                // Undo dodging/sliding visuals
                //spriteRenderer.color = Color.white;
            }
        }
        else
        {
            // Debug.Log("Attempting to slide!!!");
            animator.SetBool("isSliding", false);
            SetLayer("Player"); // Instead of default 

            // Undo dodging/sliding visuals WHEN we run out of energy DURING sliding
            //spriteRenderer.color = Color.white;


        }
        
    }

    

    #endregion

    private void SetLayer(string layerName)
    {
        this.gameObject.layer = LayerMask.NameToLayer(layerName); // Gets the layer number of the layer mask Dodge and sets the Character to that or to default - essentially the iFrame but not complete yet
        body.gameObject.layer = LayerMask.NameToLayer(layerName); // Fixes the layer collision matrix not working - idea by: https://forum.unity.com/threads/collision-matrix-not-working.904460/
    }

}
