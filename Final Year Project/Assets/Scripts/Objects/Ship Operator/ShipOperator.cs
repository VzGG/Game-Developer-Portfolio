using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipOperator
{
    // Initialize these fields in the inspector
    [SerializeField] private string operatorName = "";
    [SerializeField] private int money = 0;
    [SerializeField] private int specialMoney = 0;
    [SerializeField] private List<string> dialogues = new List<string>();

    public ShipOperator(string givenOperatorName, int givenMoney, int givenSpecialMoney, List<string> givenDialogues)
    {
        this.operatorName = givenOperatorName;
        this.money = givenMoney;
        this.specialMoney = givenSpecialMoney;
        this.dialogues = givenDialogues;
    }



    // Getters and Setters
    public string GetOperatorName() { return this.operatorName; }

    public int GetMoney() { return this.money; }
    public void SetMoney(int givenMoney) { this.money = givenMoney; }

    public int GetSpecialMoney() { return this.specialMoney; }

    public List<string> GetDialogues() { return this.dialogues; }
    public void SetSpecialMoney(int givenSpecialMoney) { this.specialMoney = givenSpecialMoney; }


}
