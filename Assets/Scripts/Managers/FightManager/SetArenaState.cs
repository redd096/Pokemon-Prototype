using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetArenaState : FightManagerState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //deactive every menu, then set arena and player menu
        DeactiveEverything();

        SetArena();

        SetPlayerMenu();
    }

    void DeactiveEverything()
    {
        fightManager.FightUIManager.DeactiveEverything();
    }

    void SetArena()
    {
        //set only if there are pokemons (because this is called at start game before deactive fightManager, but the enemy has no pokemons until player find one in the grass)
        if (GameManager.instance.player.PlayerPokemons.Count <= 0 || fightManager.enemyPokemons.Count <= 0)
            return;

        PokemonModel playerPokemon = null;

        //find first pokemon alive
        foreach (PokemonModel pokemon in GameManager.instance.player.PlayerPokemons)
        {
            if (pokemon.CurrentHealth > 0)
            {
                playerPokemon = pokemon;
                break;
            }
        }

        //if no pokemon alive, show the first
        if(playerPokemon == null)
        {
            playerPokemon = GameManager.instance.player.PlayerPokemons[0];
        }

        PokemonModel enemyPokemon = fightManager.enemyPokemons[0];

        //set currents pokemon in arena
        fightManager.SetCurrentPlayerPokemon(playerPokemon);
        fightManager.SetCurrentEnemyPokemon(enemyPokemon);

        //and set arena UI
        fightManager.FightUIManager.SetArena();
    }

    void SetPlayerMenu()
    {
        //set only if there is a pokemon in arena
        if (fightManager.currentPlayerPokemon == null)
            return;

        //set skills first pokemon
        fightManager.FightUIManager.SetSkillsList(fightManager.currentPlayerPokemon);

        //set pokemons alive and not in arena
        fightManager.FightUIManager.SetPokemonList();

        //set list of items
        fightManager.FightUIManager.SetItemsList();
    }
}
