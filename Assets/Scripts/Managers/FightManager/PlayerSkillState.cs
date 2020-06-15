using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : FightManagerState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //TODO
        //hide fight menu
        //show description what skill is used
        //make animation attack
        //damage enemy based on skill (animation health slider)
        //apply possible effects
        //start enemy turn
    }
}
