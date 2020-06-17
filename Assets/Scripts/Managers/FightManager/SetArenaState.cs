using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetArenaState : FightManagerState
{
    [SerializeField] string pathPokemon = "ScriptableObjects/Pokemons";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //deactive every menu, enemy select his pokemons, then set arena
        DeactiveEverything();

        EnemySelectPokemons();

        SetArena();
    }

    void DeactiveEverything()
    {
        fightManager.FightUIManager.DeactiveEverything();
    }

    void EnemySelectPokemons()
    {
        //get list of pokemons and random team quantity
        PokemonData[] pokemonDatas = Resources.LoadAll<PokemonData>(pathPokemon);
        int quantity = Random.Range(fightManager.minPokemons, fightManager.maxPokemons);

        fightManager.enemyPokemons = new List<PokemonModel>();

        //fill the quantity with random pokemon
        for (int i = 0; i < quantity; i++)
        {
            int random = Random.Range(0, pokemonDatas.Length);

            fightManager.enemyPokemons.Add( new PokemonModel(pokemonDatas[random], 5) );
        }
    }

    void SetArena()
    {
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
}
