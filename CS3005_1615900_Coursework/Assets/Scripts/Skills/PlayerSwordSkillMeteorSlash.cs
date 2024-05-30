using Oswald.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkillMeteorSlash : Skill
{
    protected override IEnumerator BeforeEffect(PlayerController playerController)
    {
        yield return null;
    }

    protected override IEnumerator ApplyEffect(PlayerController playerController)
    {
        var animator = playerController.GetAnimatorController().GetAnimator();
        var rb2d = playerController.GetComponent<Rigidbody2D>();

        Debug.Log("Skill hello");

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("MeteorSlashRising") == true);

        // During MeteorSlashRising

        //
        rb2d.velocity = new Vector2(rb2d.velocity.x, 3.5f);

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Air_Sword_Attack_3") == true);

        // During air sword attack 3
        //

        // To-do skills stuff


        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Air_Sword_Attack_3_Landed") == true);

        // to-do

        // End of last animation. 
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f && animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Air_Sword_Attack_3_Landed") == true);
        Debug.Log("Skill finished");
    }

    protected override IEnumerator RevertEffect(PlayerController playerController)
    {
        yield return base.RevertEffect(playerController);

        // do any revert here
        var playerAttack = playerController.GetPlayerAttack();

        playerAttack.UpdateAttackAnimationSpeed();

        yield return null;
    }
}