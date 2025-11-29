using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadCrawlerStates : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Land");

        if (stateInfo.IsName("Land"))
        {
            animator.SetFloat("IdleTimer", 1.5f);
            animator.SetBool("IsAttacking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UndeadCrawler ucScript = animator.transform.parent.GetComponent<UndeadCrawler>();

        if (stateInfo.IsName("Idle"))
        {
            ucScript.Flip();
            ucScript.FacePlayer();
        }
    }
}