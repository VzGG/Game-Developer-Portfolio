using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected AnimatorController.AnimStates animState;
    [SerializeField] protected string animParameter = "";
    [SerializeField] protected float damage = 0f;


}
