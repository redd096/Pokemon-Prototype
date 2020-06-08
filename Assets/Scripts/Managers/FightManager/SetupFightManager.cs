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

        //set arena
        SetArena();

        //set lists of pokemons and items
        SetPokemonList();
        SetItemsList();
    }

    void DeactiveEverything()
    {
        fightManager.FightUIManager.DeactiveEverything();
    }

    void SetArena()
    {
        PokemonModel playerPokemon = GameManager.instance.player.PlayerPokemon[0];
        PokemonModel enemyPokemon = fightManager.enemyPokemons[0];

        fightManager.FightUIManager.SetArena(playerPokemon, enemyPokemon);
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
