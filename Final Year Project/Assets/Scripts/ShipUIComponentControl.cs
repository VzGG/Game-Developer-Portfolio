using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// NOT USED
/// </summary>
public class ShipUIComponentControl : MonoBehaviour
{
    [SerializeField] GameObject target;

    private Vector3 screenPoint;
    private Vector3 offset;

    public void SetTarget(GameObject target) { this.target = target; }



    // http://coffeebreakcodes.com/drag-object-with-mouse-unity3d/
    private void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

/*        Debug.Log("Mouse pos: " + Input.mousePosition);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("mouse pos in world space: " + worldPos);*/

    }

    private void OnMouseDrag()
    {
        //Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        //Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        //transform.position = cursorPosition;

        //Debug.Log("mouse pos: " + Input.mousePosition);
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("mouse pos in world pos: " + worldPos);

        //// Moves object to the mouse' position
        //transform.position = Camera.main.ScreenToWorldPoint(
        //    new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(0f)));

        Vector2 clickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(clickPos);

        //Debug.Log("Click pos: " + clickPos + "\n" + "World pos: " + worldPos);

        this.transform.position = worldPos;
    }



    private void OnMouseUp()
    {
        bool isTouching = this.gameObject.GetComponent<BoxCollider2D>().IsTouching(target.GetComponent<BoxCollider2D>());
        Debug.Log("istouching?: " + isTouching);
        // If after releasing the mouse button and we are touching the target's collider, we can then can set the position to be
        // where the mouse was last placed position
        if (isTouching)
        {
            // Do nothing
            Debug.Log("It is allowed.");
            Vector2 clickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(clickPos);
            transform.position = worldPos;
        }
        else
        {
            // Return it to somewhere else
            this.transform.position = new Vector2(0f, -4f);
        }

        //if (!this.gameObject.GetComponent<BoxCollider2D>().IsTouching(target.GetComponent<BoxCollider2D>()))
        //{
        //    // if not touching, return to target's position
        //    this.transform.position = target.transform.position;
        //}

        //Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("pos: " + pos);
    }
}
