using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] GameObject portalVFX;

    [SerializeField] LevelsManager levelsManager;
    [SerializeField] string sceneNameOfNextLevel;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(portalVFX, transform.position, transform.rotation);
        levelsManager = FindObjectOfType<LevelsManager>();
    }

    public void SetSceneNameOfNextLevel (string sceneNameOfNextLevel) { this.sceneNameOfNextLevel = sceneNameOfNextLevel; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Portal can only interact with the player due to the COllion layer matrix in the Edit > Project Settings
        // but checks for it incase
        if (collision.tag == "Player")
        {
            levelsManager.LoadScene(sceneNameOfNextLevel);
        }
    }

}
