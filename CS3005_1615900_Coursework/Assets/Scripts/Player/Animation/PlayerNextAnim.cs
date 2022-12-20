using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oswald.Player
{
    /// <summary>
    /// This is attached to the animation states from the Animator tab. It is used for the player.
    /// This is used to allow which states are allowed to be transitioned into and to block certain player inputs.
    /// Blocking certain inputs ensures that the player cannot go to (2) Sword Attack Anim from (1) Bow Attack Anim when (1) can only go to 
    /// Bow Attack Anim (3).
    /// </summary>
    public class PlayerNextAnim : StateMachineBehaviour
    {
        [SerializeField] bool isNextAttackBow = false;
        [SerializeField] bool isNextAttackSword = false;
        [Tooltip("When ticked, it will allow the arrow attack projectiles to be piercing enemies.")]
        [SerializeField] bool isCurrentAttackEnder = false;
        [SerializeField] bool isBowAttack = false;

        [SerializeField] Vector2 firstHitPushback;
        [SerializeField] Vector2 stayHitPushback;
        [SerializeField] public static PlayerController playerController;

        public static void SetPlayerController(PlayerController pc) { playerController = pc; }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Activate which attacks are allowed in the next state.
            playerController.SetIsNextAttackBow(isNextAttackBow);
            playerController.SetIsNextAttackSword(isNextAttackSword);

            ChangeBowAttackOnHitEffect(playerController);
            ChangePushbackEffects(playerController);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // As comment above, this equal to the Monobehaviour's Update method.
        //}


        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            playerController.GetPlayerAttack().localFirstPushback = Vector2.zero;
            playerController.GetPlayerAttack().localStayHitPushback = Vector2.zero;
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}



        private void ChangeBowAttackOnHitEffect(PlayerController playerController)
        {
            PlayerAttack playerAttack = playerController.GetPlayerAttack();
            if (isBowAttack)
            {
                if (isCurrentAttackEnder)
                {
                    playerAttack.localOnHit = ArrowProjectile.OnHitEffect.Pierce;
                }
                else
                    playerAttack.localOnHit = ArrowProjectile.OnHitEffect.NoPierce;
            }
        }

        private void ChangePushbackEffects(PlayerController playerController)
        {
            playerController.GetPlayerAttack().localFirstPushback = this.firstHitPushback;
            playerController.GetPlayerAttack().localStayHitPushback = this.stayHitPushback;
        }
    }
}


