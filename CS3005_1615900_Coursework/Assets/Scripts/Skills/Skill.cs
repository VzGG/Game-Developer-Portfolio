using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The skill class changes the animator controller and animation state to a specific one that represents that specific skill.
/// Along with that is that within the new animator controller, each entry state of an animation state can have their own unique effect.
/// </summary>
public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected RuntimeAnimatorController runtimeAnimatorController;             // Change the current animator controller to this one when the skill is activated (effect is called).
    [SerializeField] protected AnimatorController.AnimStates AnimStates;                        
    [SerializeField] protected string AnimParameter = "";                                       // The parameter needed to trigger the animation to change states.
    [SerializeField] protected float[] Damage;                                                  // Gives the holder of skill bonus damage.
    [SerializeField] protected float EnergyUsage = 0f;                                          // Used to reduce the holder of skill their energy.
    [SerializeField] protected float Cooldown = 0f;                                             // All skills cannot be activated right after it finished, it must have a timer before it can be used again.
    [SerializeField] protected float Timer = 0f;                                                // The timer that determines whether the cooldown has finished or not.
    [SerializeField] protected bool CanActivateSkill = true;                                    // Determines whether the skill can be used the holder.
    [SerializeField] protected bool ActivateSkill = false;                                      // Determines that the holder must fully commit to this skill and must not be able to do other actions while doing so. 
    [SerializeField] Sprite skillIcon;
    protected bool onCooldown = true;
    protected float cooldownInterval = 0.1f;

    public float[] GetDamage() { return this.Damage; }
    public float GetTimer() { return this.Timer; }
    public float GetCooldown() { return this.Cooldown; }
    public bool GetCanActivateSkill() { return this.CanActivateSkill; }
    public bool GetActivateSkill() { return this.ActivateSkill; }
    public Sprite GetSkillIcon() { return this.skillIcon; }
    public void SetActivateSkill(bool status) { this.ActivateSkill = status; }

    /// <summary>
    /// The skill's effect, define in each child class. Each have their own effect, but the caller of this method does not care about what it does,
    /// it should only care about calling it.
    /// </summary>
    /// <param name="animatorController"></param>
    /// <returns></returns>
    public abstract IEnumerator Effect(AnimatorController animatorController);
    protected abstract IEnumerator BeforeEffect(PlayerController playerController);
    protected abstract IEnumerator ApplyEffect(PlayerController playerController);
    protected abstract IEnumerator RevertEffect(PlayerController playerController);
    protected IEnumerator ChangeSkillAnimation(AnimatorController animatorController)
    {
        this.ActivateSkill = true;
        animatorController.ChangeAnimController(AnimStates);
        animatorController.ChangeAnimationTrigger(AnimParameter);
        this.CanActivateSkill = false;

        yield return null;
    }

    protected IEnumerator ActivateCooldown()
    {
        Debug.Log("Activating cooldown timer");
        onCooldown = true;
        while (onCooldown)
        {
            yield return new WaitForSeconds(cooldownInterval);
            Timer += cooldownInterval;
            if (Timer >= Cooldown)
            {
                Timer = 0f;
                onCooldown = false;
            }
        }
    }
}