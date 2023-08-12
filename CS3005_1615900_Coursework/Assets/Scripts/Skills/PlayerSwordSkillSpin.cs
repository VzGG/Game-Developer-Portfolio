using Oswald.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkillSpin : Skill
{
    [SerializeField] private Vector2 _sizeIncrease;     // Define in the inspector.
    [SerializeField] private Vector2 _offset;           // Define in the inspector.
    [SerializeField] private Vector2 _skillPushback;    // Define in the inspector.

    public override IEnumerator Effect(AnimatorController animatorController)
    {
        // Show animation for the skill.
        this.ActivateSkill = true;
        animatorController.ChangeAnimController(AnimStates);
        animatorController.ChangeAnimationTrigger(AnimParameter);
        this.CanActivateSkill = false;

        PlayerController playerController = animatorController.GetComponent<PlayerController>();
        PlayerAttack playerAttack = playerController.GetPlayerAttack();
        //PlayerAttack playerAttack = animatorController.GetComponent<PlayerController>().GetPlayerAttack();

        Rigidbody2D rb2d = playerController.GetComponent<Rigidbody2D>();
        // Update skill energy that is used to reduce the current energy.
        playerAttack.SkillEnergy = this.EnergyUsage;

        // Make search target box bigger - Search Target GameObject.
        BoxCollider2D targetBoxCollider2D = animatorController.GetComponent<Target>().GetBoxCollider2D();
        targetBoxCollider2D.size = new Vector2(_sizeIncrease.x, targetBoxCollider2D.size.y);
        targetBoxCollider2D.offset = _offset;

        // Make the skill attack hitbox bigger - Hitbox GameObject.
        BoxCollider2D boxCollider2D = playerAttack.GetMyHitBox();
        boxCollider2D.size = new Vector2(_sizeIncrease.x, boxCollider2D.size.y);
        boxCollider2D.offset = _offset;

        // Change the push back to the reverse pushback.
        playerAttack.localFirstPushback = _skillPushback;

        // Wait for anim 1 to enter state. See animator tab.
        yield return new WaitUntil(() => animatorController.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Player_Sword_Attack_3") == true);

        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3f);
        playerAttack.skillDamage = this.Damage[0];

        // Wait for anim 2 to enter state. See animator tab.
        yield return new WaitUntil(() => animatorController.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Player_Sword_Attack_3 0") == true);

        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3f);
        playerAttack.skillDamage = this.Damage[1];

        // Wait for anim 3 to enter state. See animator tab.
        yield return new WaitUntil(() => animatorController.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Player_Sword_Attack_3 1") == true);

        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3f);
        playerAttack.skillDamage = this.Damage[2];

        // Wait until the activate skill is of this skills is false, then run everything below.
        //yield return new WaitWhile(() => this.ActivateSkill == true);

        // Wait until it is the final animation and the final animation finished.
        yield return new WaitWhile(() =>
        animatorController.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f &&
        animatorController.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Player_Sword_Attack_3 1") == true);
        Debug.Log("Skill animation just finished. Wait for cooldown to finish before activating skill.".ToUpper());

        playerAttack.SkillEnergy = 0;
        playerAttack.skillDamage = 0f;
        // Make the skill attack hitbox smaller, back to original hitbox size.
        boxCollider2D.size = new Vector2(PlayerAttack.SmallHitboxSizeX, boxCollider2D.size.y);
        boxCollider2D.offset = new Vector2(PlayerAttack.SmallHitboxOffsetX, boxCollider2D.offset.y);
        // Make search target box smaller.
        targetBoxCollider2D.size = new Vector2(PlayerAttack.SmallHitboxSizeX, targetBoxCollider2D.size.y);
        targetBoxCollider2D.offset = new Vector2(PlayerAttack.SmallHitboxOffsetX, targetBoxCollider2D.offset.y);
        // Change the pushback back to 0.
        playerAttack.localFirstPushback = Vector2.zero;
        playerAttack.localStayHitPushback = Vector2.zero;

        // Deactivate skill.
        this.ActivateSkill = false;

        //yield return new WaitForSeconds(skillCooldown); // Easier way but need to track the timer.
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
