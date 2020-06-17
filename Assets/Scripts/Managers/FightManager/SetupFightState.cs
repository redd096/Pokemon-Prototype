using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupFightState : FightManagerState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //set lists of skills, pokemons and items
        SetSkillsList();
        SetPokemonList();
        SetItemsList();
    }

    void SetSkillsList()
    {
        //set skills first pokemon
        fightManager.FightUIManager.SetSkillsList(fightManager.currentPlayerPokemon);
    }

    void SetPokemonList()
    {
        //set pokemons alive and not in arena
        fightManager.FightUIManager.SetPokemonList();
    }

    void SetItemsList()
    {
        fightManager.FightUIManager.SetItemsList();
    }
}
