using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Idea Inspired by:
 * 
 * Brackeys (2017) 'Smooth Camera Follow in Unity - Tutorial'
 * Available at: https://www.youtube.com/watch?v=MFQhpwc6cKE
 */
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] float smoothing = 0.125f;
    [SerializeField] Vector3 offset;
    public void SetPlayerController(PlayerController playerController) { this.playerController = playerController; }
    public PlayerController GetPlayerController() { return this.playerController; }


    private void FixedUpdate()
    {
        if (playerController == null) { return; }


        // Debug.Log("HELLO?");
        //this.transform.position = new Vector3(playerController.transform.position.x, playerController.transform.position.y, -10f);
        //Vector3 offset = transform.position - playerController.transform.position;

        //this.transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, -10f), 
        //    new Vector3(playerController.transform.position.x, playerController.transform.position.y, -10f) + new Vector3(offset.x, offset.y, 0f),
        //    smoothing * Time.deltaTime);

        Vector3 playerPosition = playerController.transform.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(this.transform.position, playerPosition, smoothing * Time.deltaTime);
        this.transform.position = smoothPosition;

    }
}
