using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    Animator anim;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //use item

        ApplyEffect();
    }

    #region enter

    protected override void GetReferences(Animator anim)
    {
        base.GetReferences(anim);

        //get animator reference
        this.anim = anim;
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
