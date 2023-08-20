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

    protected override IEnumerator BeforeEffect(PlayerController playerController)
    {
        var animatorController = playerController.GetAnimatorController();
        var playerAttack = playerController.GetPlayerAttack();

        BoxCollider2D targetBoxCollider2D = animatorController.GetComponent<Target>().GetBoxCollider2D();
        targetBoxCollider2D.size = new Vector2(_sizeIncrease.x, targetBoxCollider2D.size.y);
        targetBoxCollider2D.offset = _offset;

        // Make the skill attack hitbox bigger - Hitbox GameObject.
        BoxCollider2D boxCollider2D = playerAttack.GetMyHitBox();
        boxCollider2D.size = new Vector2(_sizeIncrease.x, boxCollider2D.size.y);
        boxCollider2D.offset = _offset;

        // Change the push back to the reverse pushback.
        playerAttack.localFirstPushback = _skillPushback;

        yield return null;
    }

    protected override IEnumerator ApplyEffect(PlayerController playerController)
    {
        var rb2d = playerController.GetComponent<Rigidbody2D>();
        var playerAttack = playerController.GetPlayerAttack();
        var animatorController = playerController.GetAnimatorController();

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

        // Wait until it is the final animation and the final animation finished.
        yield return new WaitWhile(() =>
        animatorController.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f &&
        animatorController.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Player_Sword_Attack_3 1") == true);
        Debug.Log("Skill animation just finished. Wait for cooldown to finish before activating skill.".ToUpper());
    }

    protected override IEnumerator RevertEffect(PlayerController playerController)
    {
        yield return base.RevertEffect(playerController);

        var playerAttack = playerController.GetPlayerAttack();
        var boxCollider2D = playerAttack.GetMyHitBox();
        var targetBoxCollider2D = playerController.GetAnimatorController().GetComponent<Target>().GetBoxCollider2D();

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

        yield return null;
    }
}
