using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "Usi {Item} su {PlayerPokemon}...";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show description using item
        //use item

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

        //apply effect
        ApplyEffect();
    }

    #endregion

    void ApplyEffect()
    {
        //TODO
        //apply possible effect to player pokemon
        //description effect attivo?

        EndTurn();
    }

    void EndTurn()
    {
        //change state
        anim.SetTrigger("Next");
    }
}
