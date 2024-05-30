using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkillSlash : Skill
{
    [SerializeField] private float _slashMoveSpeed = 3.25f;
    [SerializeField] private Vector2 _sizeIncrease;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Vector2[] _skillPushbacks;

    protected override IEnumerator BeforeEffect(PlayerController playerController)
    {
        yield return null;
    }

    protected override IEnumerator ApplyEffect(PlayerController playerController)
    {
        var animator = playerController.GetAnimatorController().GetAnimator();
        var rb2d = playerController.GetComponent<Rigidbody2D>();
        var playerAttack = playerController.GetPlayerAttack();
        var targetBoxCollider2D = playerController.GetAnimatorController().GetComponent<Target>().GetBoxCollider2D();

        // Don't run below until the animation state is at the first one
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Sword_Attack_2") == true);

        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3f);

        playerAttack.skillDamage = this.Damage[0];
        playerAttack.SkillEnergy = this.EnergyUsage;
        playerAttack.localFirstPushback = _skillPushbacks[0];
        targetBoxCollider2D.size = new Vector2(_sizeIncrease.x, targetBoxCollider2D.size.y);
        targetBoxCollider2D.offset = _offset;

        // Wait for second animation state to enter.
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Sword_Attack_1 0") == true);

        if (playerController._isMidAir)
            rb2d.velocity = new Vector2(rb2d.velocity.x, 3f);

        playerAttack.skillDamage = this.Damage[1];
        playerAttack.localFirstPushback = _skillPushbacks[1];

        // Wait for third animation state to enter then run the code below.
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Sword_Attack_3") == true);

        playerAttack.skillDamage = this.Damage[2];
        playerAttack.localFirstPushback = _skillPushbacks[2];
        rb2d.velocity = new Vector2(_slashMoveSpeed * playerController.transform.localScale.x, rb2d.velocity.y);
        playerController.transform.localScale = new Vector3(playerController.transform.localScale.x, playerController.transform.localScale.y,
            playerController.transform.localScale.z);


        // When the third animation state is done, then run the code below.
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Sword_Attack_3") == true);

        Debug.Log("Sword slash finished");
    }

    protected override IEnumerator RevertEffect(PlayerController playerController)
    {
        yield return base.RevertEffect(playerController);

        var playerAttack = playerController.GetPlayerAttack();
        var targetBoxCollider2D = playerController.GetAnimatorController().GetComponent<Target>().GetBoxCollider2D();

        playerAttack.UpdateAttackAnimationSpeed();
        playerAttack.skillDamage = 0;
        playerAttack.SkillEnergy = 0;
        playerAttack.localFirstPushback = Vector2.zero;
        playerAttack.localStayHitPushback = Vector2.zero;
        targetBoxCollider2D.size = new Vector2(PlayerAttack.SmallHitboxSizeX, targetBoxCollider2D.size.y);
        targetBoxCollider2D.offset = new Vector2(PlayerAttack.SmallHitboxOffsetX, targetBoxCollider2D.offset.y);

        yield return null;
    }
}