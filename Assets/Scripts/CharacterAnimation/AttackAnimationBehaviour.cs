using UnityEngine;

public class AttackAnimationBehaviour : StateMachineBehaviour
{
    // Questo flag serve per evitare di chiamare AllowAttackQueueing() ripetutamente.
    private bool hasEnabledQueue = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Quando l'animazione ha raggiunto il 75% e non è già stato abilitato il queue, abilita la coda.
        if (!hasEnabledQueue && stateInfo.normalizedTime > 0.75f)
        {
            var playerCombat = animator.gameObject.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                playerCombat.AllowAttackQueueing();
            }
            hasEnabledQueue = true;
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        var playerCombat = animator.gameObject.GetComponent<PlayerCombat>();
        if (playerCombat != null){
            playerCombat.OnAttackAnimationComplete();
        }
         // Reset del flag per la prossima attivazione dell'animazione
        hasEnabledQueue = false;
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
}
