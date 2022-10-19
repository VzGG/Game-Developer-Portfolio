using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Concept of IEnumerator and inspired by:
 * 
 * GameDev.tv Team, Davidson, R., Pettie, G. (2019) ‘Complete C# Unity Game Developer 2D – Glitch Garden’ ***2019 Course. 2021 course provides different learning materials*** [Course] 
 * Available at: https://www.udemy.com/course/unitycourse/ 
 */

public class TrapFloor : MonoBehaviour
{
    [SerializeField] float timeToShowVFX = 3f;
    [SerializeField] Vector2 spawnPosition;
    [SerializeField] GameObject explosionVFX;


    [Space]
    [Header("Dialogue Properties")]
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] int dialogueIndex;                         // Specified in the editor for more customisation


    [SerializeField] float destroyFloorTimer = 2f;
    private int counter = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            counter++;
            if (counter == 1)
            {
                StartCoroutine(DestroyFloor());
            }
        }
    }

    IEnumerator DestroyFloor()
    {
        yield return new WaitForSeconds(destroyFloorTimer);

        // Spawn explosion VFX at the given position
        Instantiate(explosionVFX, spawnPosition, Quaternion.identity);

        Instantiate(dialogueManager.InstantiateDialogue(dialogueIndex));

        gameObject.SetActive(false);
    }
}
