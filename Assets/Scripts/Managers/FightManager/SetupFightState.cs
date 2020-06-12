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
        SetItemsList();
        SetPokemonList();
    }

    void SetSkillsList()
    {
        //set skills first pokemon
        fightManager.FightUIManager.SetSkillsList(GameManager.instance.player.PlayerPokemons[0]);
    }

    void SetPokemonList()
    {
        fightManager.FightUIManager.SetPokemonList();
    }

    void SetItemsList()
    {
        fightManager.FightUIManager.SetItemsList();
    }
}
