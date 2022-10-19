using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    void Awake()
    {
        //// Singleton and don't destroy this
        //int numberOfMainCanvas = FindObjectsOfType<MainCanvas>().Length;
        //if (numberOfMainCanvas > 1)
        //    Destroy(this.gameObject);
        //else
        //    DontDestroyOnLoad(this.gameObject);

    }

    [SerializeField] int increasePowerIndex = -1;
    [SerializeField] int increaseFireRateIndex = -1;
    [SerializeField] int reduceEnergyConsumptionIndex = -1;


}
