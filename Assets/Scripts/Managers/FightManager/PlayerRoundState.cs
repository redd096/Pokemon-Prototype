using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoundState : FightManagerState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        ActivePlayerMenu();
    }

    void ActivePlayerMenu()
    {
        fightManager.FightUIManager.ActivePlayerMenu();
    }
}
