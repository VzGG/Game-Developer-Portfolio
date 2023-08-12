using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowSkillPierce : Skill
{
    [SerializeField] private Vector2 _skillPushbackFirst;       // Define in the inspector.
    [SerializeField] private Vector2 _skillPushbackStay;        // Define in the inspector.

    public override IEnumerator Effect(AnimatorController animatorController)
    {
        // Show animation of the skill.
        Debug.Log("Activate pierce skill");
        this.ActivateSkill = true;
        PlayerController playerController = animatorController.GetComponent<PlayerController>();
        PlayerAttack playerAttack = playerController.GetPlayerAttack();
        Rigidbody2D rb2d = playerController.GetComponent<Rigidbody2D>();
        //PlayerAttack playerAttack = animatorController.GetComponent<PlayerController>().GetPlayerAttack();
        Animator animator = animatorController.GetComponent<Animator>();
        animator.runtimeAnimatorController = this.runtimeAnimatorController;
        animator.SetTrigger(this.AnimParameter);
        this.CanActivateSkill = false;

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Bow_Attack") == true);

        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3.5f);

        playerAttack.localOnHit = ArrowProjectile.OnHitEffect.Pierce;
        playerAttack.skillDamage = this.Damage[0];
        playerAttack.SkillEnergy = this.EnergyUsage;
        // Add push back effect for this particular attack.
        playerAttack.localFirstPushback = _skillPushbackFirst;
        playerAttack.localStayHitPushback = _skillPushbackStay;

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Bow_Attack 0") == true);

        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3.5f);
        playerAttack.skillDamage = this.Damage[1];

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Bow_Attack 1") == true);

        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3.5f);
        playerAttack.skillDamage = this.Damage[2];


        // End of last animation.
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f &&
        animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Bow_Attack 1") == true);
        Debug.Log("Skill pierece finished");

        playerAttack.skillDamage = 0;
        playerAttack.SkillEnergy = 0;
        playerAttack.localFirstPushback = Vector2.zero;
        playerAttack.localStayHitPushback = Vector2.zero;

        this.ActivateSkill = false;


        // Ensure that this skill can only be activated when there is no more cooldown.
        bool onCooldown = true;
        float interval = 0.01f;
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
