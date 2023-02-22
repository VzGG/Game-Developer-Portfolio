using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class GenerateCombos : MonoBehaviour
{
    // We want to generate all the possible combos - this is to save us time?
    // Only do this once!

    [MenuItem("Oswald/GenerateCombo")]
    static void GenerateCombo()
    {
        Debug.Log("Generating all possible combos!");

        // Load the animator controller https://docs.unity3d.com/ScriptReference/AssetDatabase.LoadAssetAtPath.html
        RuntimeAnimatorController animatorController =  (RuntimeAnimatorController)AssetDatabase.LoadAssetAtPath("Assets/Animation/Player/PremadeCombos/Test_SwordAirCombo.controller", typeof(RuntimeAnimatorController));

        // Cast it to a different type https://forum.unity.com/threads/create-mecanim-states-and-transitions-by-script.188544/
        UnityEditor.Animations.AnimatorController castAnimatorController = (UnityEditor.Animations.AnimatorController)animatorController;
        UnityEditor.Animations.AnimatorControllerLayer layer = castAnimatorController.layers[0];
        AnimatorStateMachine stateMachine = layer.stateMachine;

        // Clear all current states in the state machine
        ChildAnimatorState[] allStates = stateMachine.states;
        // Only clear it when there are animator states in it
        if (allStates.Length > 0)
        {
            Debug.Log($"Removing any states in the {animatorController.name}");
            foreach (ChildAnimatorState childState in allStates)
            {
                stateMachine.RemoveState(childState.state);
            }
        }

    }
}