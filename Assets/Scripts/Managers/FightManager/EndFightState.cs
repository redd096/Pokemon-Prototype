using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFightState : FightManagerState
{
    [Header("Player Won?")]
    [SerializeField] bool isWin = true;

    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "Hai finito i pokemon...";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //set description win or lose
        //if lose, end fight
        //if win, go to next state

        SetDescription();
    }

    #region enter

    void SetDescription()
    {
        //deactive menu, to be sure to read description
        fightManager.FightUIManager.DeactiveMenu();

        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(description, OnEndDescription);
    }

    #endregion

    void OnEndDescription()
    {
        //deactive description
        fightManager.FightUIManager.EndDescription();

        //end fight
        EndFight();
    }

    void EndFight()
    {
        //if win, go to next state
        if (isWin)
        {
            anim.SetTrigger("Next");
        }
        //if lose, end fight
        else
        {
            //HACK
            fightManager.RunClick();
        }
    }
}
