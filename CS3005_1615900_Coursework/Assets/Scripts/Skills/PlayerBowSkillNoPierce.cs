using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowSkillNoPierce : Skill
{
    [SerializeField] private Vector2 _skillPushbackFirst;   // Define it in the inspector.

    protected override IEnumerator BeforeEffect(PlayerController playerController)
    {
        yield return null;
    }

    protected override IEnumerator ApplyEffect(PlayerController playerController)
    {
        var animator = playerController.GetAnimatorController().GetAnimator();
        var rb2d = playerController.GetComponent<Rigidbody2D>();
        var playerAttack = playerController.GetPlayerAttack();

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
    }

    protected override IEnumerator RevertEffect(PlayerController playerController)
    {
        yield return base.RevertEffect(playerController);

        var playerAttack = playerController.GetPlayerAttack();

        playerAttack.skillDamage = 0;
        playerAttack.SkillEnergy = 0;
        playerAttack.localFirstPushback = Vector2.zero;
        playerAttack.localStayHitPushback = Vector2.zero;

        yield return null;
    }
}