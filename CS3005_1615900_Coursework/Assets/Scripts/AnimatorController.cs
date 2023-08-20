using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use this class to change the animator's animation controller at run time. 
/// Use this class to also change the animation state of the current animation controller. Animator controller contains a set of animation states.
/// </summary>
public class AnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;                                                                           // An animator component, needed for changing states and animator controllers. Define it in the inspector.
    [SerializeField] private List<RuntimeAnimatorController> animControllers = new List<RuntimeAnimatorController>();     // Each anim controllers contains a state machine which contains a set of states. Also initialize/define this in the inspector.

    /// <summary>
    /// The animControllers list should match whatever order is in here. 
    /// If Main is the first item here, then in animControllers should also have Main anim controller as the first element.
    /// </summary>
    public enum AnimStates
    {
        Main,
        Dead,
        Jump,
        JumpLv2,
        JumpLv3,
        SkillBowPierceLv1,
        SkillBowNoPierceLv1,
        SkillSwordSpinLv1,
        SkillSwordSlashLv1
    }

    private AnimStates currentAnimState;

    public void ChangeAnimController(AnimStates animStates)
    {
        currentAnimState = animStates;

        int index = ((int)animStates);
        RuntimeAnimatorController chosenAnimController = animControllers[index];

        animator.runtimeAnimatorController = chosenAnimController;
    }

    public void ChangeAnimationTrigger(string animParameter)
    {
        animator.SetTrigger(animParameter);
    }

    public void ChangeAnimationBool(string animParameter, bool status)
    {
        animator.SetBool(animParameter, status);
    }

    public AnimStates GetCurrentAnimState()
    {
        return currentAnimState;
    }

    public Animator GetAnimator() { return this.animator; }
}
