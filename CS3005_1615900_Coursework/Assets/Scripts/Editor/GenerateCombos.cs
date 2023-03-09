using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

/// <summary>
/// Important to know when using this script - https://stackoverflow.com/questions/46106849/unity3d-assetdatabase-loadassetatpath-vs-resource-load
/// 
/// Essentially, this should not be used at anypoint during runtime as this class' UnityEditor will not be included after the build.
/// </summary>
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

        // Load all motions to be added as animator states to the state machine
        Object[] clips = AssetDatabase.LoadAllAssetRepresentationsAtPath("Assets/Animation/Player/PlayerAttacks/Player_Air_Sword_Attack_1.anim") as Object[];

        Debug.Log($"Clip list length: {clips.Length}");

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

        // Randomly add states i.e., chain it
        //int numOfChainStates = Random.Range(1, 3);
        //for(int i = 0; i < numOfChainStates; i++)
        //{

        //   // stateMachine.AddState()
        //}
        //int randomClipIndex = Random.Range(0, clips.Length);
        //AnimatorState state1 = new AnimatorState();
        //// Motion type = AnimationClip type
        //state1.motion = clips[randomClipIndex];
        //// Add a state with a random clip to it
        //stateMachine.AddState(state1, Vector3.zero);


    }
}