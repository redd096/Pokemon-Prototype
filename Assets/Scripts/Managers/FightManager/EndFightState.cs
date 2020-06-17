using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFightState : FightManagerState
{
    [Header("Player Won?")]
    [SerializeField] bool isWin = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //TODO devi calcolare un botto di cose per salire di livello e item droppati        

        fightManager.RunClick();
    }
}
