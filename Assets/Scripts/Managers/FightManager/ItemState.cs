using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //TODO
        //apply possible effect to player pokemon
        //description effect attivo?
        //start enemy turn
    }
}
