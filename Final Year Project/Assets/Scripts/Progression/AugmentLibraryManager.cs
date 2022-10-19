using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentLibraryManager : MonoBehaviour
{
    [SerializeField] ProgressionManager progressionManager;
    // Start is called before the first frame update
    void Start()
    {
        progressionManager = FindObjectOfType<ProgressionManager>();
        Debug.Log("My progression manager: " + progressionManager);
    }


    /// <summary>
    /// Used in the button in MainCanvas
    /// </summary>
    /// <param name="index"></param>
    public void AddAugmentToList(int index)
    {
        // this.progressionManager.AddAugmentToList(index);
        Augment augment = AugmentLibrary.GetAugment(index);
        //Debug.Log("Augment: " + augment + "\nAugment name: " + augment.GetName() + "Augment values: " + augment.GetValues()[0]);
        
        this.progressionManager.AddAugmentToChosenAugments(augment);
    }
}
