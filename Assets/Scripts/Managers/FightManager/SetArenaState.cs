using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetArenaState : FightManagerState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //set arena and deactive everything
        DeactiveEverything();

        SetArena();
    }

    void DeactiveEverything()
    {
        fightManager.FightUIManager.DeactiveEverything();
    }

    void SetArena()
    {
        PokemonModel playerPokemon = GameManager.instance.player.PlayerPokemons[0];
        PokemonModel enemyPokemon = fightManager.enemyPokemons[0];

        //set currents pokemon in arena
        fightManager.SetCurrentPlayerPokemon(playerPokemon);
        fightManager.SetCurrentEnemyPokemon(enemyPokemon);

        //and set arena UI
        fightManager.FightUIManager.SetArena(playerPokemon, enemyPokemon);
    }
}
