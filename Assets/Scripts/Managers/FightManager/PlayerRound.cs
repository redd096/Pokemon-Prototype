using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRound : FightManagerState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        DeactiveDescription();

        ActivePlayerMenu();
    }

    void DeactiveDescription()
    {
        fightManager.description.gameObject.SetActive(false);
    }

    void ActivePlayerMenu()
    {
        fightManager.playerMenu.SetActive(true);
    }
}
