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

        //TODO devi calcolare un botto di cose per salire di livello e item droppati        

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

    void OnEndDescription()
    {
        //deactive description
        fightManager.FightUIManager.EndDescription();

        //end fight
        fightManager.RunClick();
    }

    #endregion
}
