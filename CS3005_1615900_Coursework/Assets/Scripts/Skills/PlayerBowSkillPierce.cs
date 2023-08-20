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
        yield return StartCoroutine(ChangeSkillAnimation(animatorController));

        PlayerController playerController = animatorController.GetComponent<PlayerController>();

        yield return StartCoroutine(BeforeEffect(playerController));

        yield return StartCoroutine(ApplyEffect(playerController));

        yield return StartCoroutine(RevertEffect(playerController));

        yield return StartCoroutine(ActivateCooldown());

        Debug.Log("Can now activate skill again");
        this.CanActivateSkill = true;
    }

    protected override IEnumerator BeforeEffect(PlayerController playerController)
    {
        yield return null;
    }

    protected override IEnumerator ApplyEffect(PlayerController playerController)
    {
        var animator = playerController.GetAnimatorController().GetAnimator();
        var rb2d = playerController.GetComponent<Rigidbody2D>();
        var playerAttack = playerController.GetPlayerAttack();

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
    }

    protected override IEnumerator RevertEffect(PlayerController playerController)
    {
        var playerAttack = playerController.GetPlayerAttack();
        var animatorController = playerController.GetAnimatorController();

        playerAttack.skillDamage = 0;
        playerAttack.SkillEnergy = 0;
        playerAttack.localFirstPushback = Vector2.zero;
        playerAttack.localStayHitPushback = Vector2.zero;

        this.ActivateSkill = false;

        // Change the animation back
        if (playerController._isMidAir)
            animatorController.ChangeAnimController(playerController.GetJumpLevelAnimState());
        else
            animatorController.ChangeAnimController(AnimatorController.AnimStates.Main);

        yield return null;
    }
}