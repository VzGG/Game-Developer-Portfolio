using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Heavily inspired and tutorial by:
 * 
 * CasanisPlays (2015) ‘2D Prototyping in Unity – Tutorial – Platformer – Game Camera’. [YouTube Tutorial] 21 September. 
 * Available at: https://www.youtube.com/watch?v=xSZRYGZ6TqA&list=PL2cNFQAw_ndyKRiobQ2WqVBBBSbAYBobf&index=13 
 */
public class PlayerCamera : MonoBehaviour
{

    [SerializeField] private Transform target;          // Camera following
    [SerializeField] private float smoothing;           // Dampening effect - Defined in the editor
    [SerializeField] private Vector3 offset;            // Difference between the character and camera
    [SerializeField] float cameraLowYThreshold;         // Lowest point of Y the camera can go - Defined in the editor

    // Used by the PlayerArriveLocation script to set the camera to the character again when changing levels
    public void SetMyTarget(Transform target) { this.target = target; }

    // Start is called before the first frame update
    void Start()
    {
        // Current camera position - player's position is the offset
        offset = transform.position - target.position;

        /*cameraLowYThreshold = transform.position.y;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) { return; }
        Vector3 targetCameraPostion = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, targetCameraPostion, smoothing * Time.deltaTime);

        // Camera cannot go lower than threshold 
        if (transform.position.y < cameraLowYThreshold)
        {
            transform.position = new Vector3(transform.position.x, cameraLowYThreshold, transform.position.z);
        }
    }
}
