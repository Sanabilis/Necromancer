using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    public GameObject primaryAttackGO;
    public GameObject secondaryAttackGO;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<PlayerCombat>().ResetShouldAttack();

        GameObject[] attacks = GameObject.FindGameObjectsWithTag("PlayerAttack");
        foreach (GameObject a in attacks)
        {
            Destroy(a);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PrimaryAttack"))
        {
            Instantiate(primaryAttackGO, animator.gameObject.transform.position, Quaternion.identity, animator.gameObject.transform);
        }
        else
        {
            Instantiate(secondaryAttackGO, animator.gameObject.transform.position, Quaternion.identity, animator.gameObject.transform);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetBool("IsAttacking"))
        {
            animator.gameObject.GetComponent<PlayerCombat>().ResetAttack();
        }

        animator.gameObject.GetComponent<PlayerCombat>().ResetShouldAttack();
    }
}
