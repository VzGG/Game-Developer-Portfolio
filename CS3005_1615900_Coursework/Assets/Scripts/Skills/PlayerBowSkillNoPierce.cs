using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowSkillNoPierce : Skill
{
    [SerializeField] private Vector2 _skillPushbackFirst;   // Define it in the inspector.

    public override IEnumerator Effect(AnimatorController animatorController)
    {
        // Show animation of the skill.
        Debug.LogError("Running skill");
        this.ActivateSkill = true;
        PlayerController playerController = animatorController.GetComponent<PlayerController>();
        PlayerAttack playerAttack = playerController.GetPlayerAttack();
        Rigidbody2D rb2d = playerController.GetComponent<Rigidbody2D>();
        //PlayerAttack playerAttack = animatorController.GetComponent<PlayerController>().GetPlayerAttack();

        Animator animator = animatorController.GetComponent<Animator>();
        animator.runtimeAnimatorController = this.runtimeAnimatorController;
        animator.SetTrigger(this.AnimParameter);
        this.CanActivateSkill = false;

        // The yield returns below are used like async and await functions.
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Bow_Attack") == true);
        // After entering the first bow attack animation state, change all projectile attacks to non piercing attacks.
        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3.5f);

        playerAttack.localOnHit = ArrowProjectile.OnHitEffect.NoPierce;
        playerAttack.skillDamage = this.Damage[0];
        playerAttack.SkillEnergy = this.EnergyUsage;
        playerAttack.localFirstPushback = _skillPushbackFirst;

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Bow_Attack 1") == true);

        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3.5f);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Bow_Attack 0") == true);

        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3.5f);

        // End of last animation. 
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Bow_Attack 0") == true);
        Debug.Log("Skill finished");

        // Deactivate the skill, meaning the player is not locked out of any animations anymore.

        playerAttack.skillDamage = 0;
        playerAttack.SkillEnergy = 0;
        playerAttack.localFirstPushback = Vector2.zero;
        playerAttack.localStayHitPushback = Vector2.zero;

        this.ActivateSkill = false;

        //yield return new WaitForSeconds(skillCooldown); // Easier way but need to track the timer.
        // Ensure that this skill can only be activated when there is no more cooldown.
        bool onCooldown = true;
        float interval = 0.1f;
        while (onCooldown)
        {
            yield return new WaitForSeconds(interval);
            Timer += interval;
            if (Timer >= Cooldown)
            {
                Timer = 0f;
                onCooldown = false;
            }
        }
        Debug.Log("Can now activate skill again");
        this.CanActivateSkill = true;
    }
}
