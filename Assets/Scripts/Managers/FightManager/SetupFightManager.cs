using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupFightManager : FightManagerState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //setup everything, than wait end of the transition, it will call next state

        DeactiveEverything();

        //TODO set every menu (skill, lista pokemon, lista oggetti)
    }

    void DeactiveEverything()
    {
        //deactive everything
        fightManager.playerPokemon.gameObject.SetActive(false);
        fightManager.description.gameObject.SetActive(false);
        fightManager.playerMenu.SetActive(false);
        fightManager.fightMenu.SetActive(false);
        fightManager.pokemonMenu?.SetActive(false);
        fightManager.bagMenu?.SetActive(false);
    }
}
