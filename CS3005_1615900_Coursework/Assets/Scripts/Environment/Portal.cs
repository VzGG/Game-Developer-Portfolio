using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oswald.Manager;

namespace Oswald.Environment
{
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

        public void SetSceneNameOfNextLevel(string sceneNameOfNextLevel) { this.sceneNameOfNextLevel = sceneNameOfNextLevel; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Portal can only interact with the player due to the COllion layer matrix in the Edit > Project Settings
            // but checks for it incase
            // Can also go to the portal when sliding/dodging
            if (collision.tag == "Player" || collision.tag == "Dodge")
            {
                levelsManager.LoadScene(sceneNameOfNextLevel);
            }
        }
    }
}


