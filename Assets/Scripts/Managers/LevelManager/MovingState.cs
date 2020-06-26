using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        SetPlayer();
    }

    void SetPlayer()
    {
        //set player
        GameManager.instance.Player.StartMovingPhase();
    }
}
