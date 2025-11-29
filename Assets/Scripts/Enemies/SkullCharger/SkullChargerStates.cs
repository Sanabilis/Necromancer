using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullChargerStates : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SkullCharger scScript = animator.transform.parent.GetComponent<SkullCharger>();

        if (stateInfo.IsName("Charge"))
        {
            scScript.SetAttack();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SkullCharger scScript = animator.transform.parent.GetComponent<SkullCharger>();

        if (stateInfo.IsName("Charge"))
        {
            scScript.ResetAttack();
        }
        else
        {
            scScript.Flip();
            scScript.FacePlayer();
        }
    }
}
