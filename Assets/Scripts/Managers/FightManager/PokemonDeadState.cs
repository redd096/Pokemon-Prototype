using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonDeadState : FightManagerState
{
    [Header("Is Player Pokemon Who is Dead")]
    [SerializeField] bool isPlayerPokemon = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //check if there are pokemon alives, else call NoMorePokemons

        //if there are pokemon alives, show player menu or select new enemy pokemon

        CheckPokemons(animator);
    }

    void CheckPokemons(Animator anim)
    {
        List<PokemonModel> pokemonList = isPlayerPokemon ? GameManager.instance.player.PlayerPokemons : fightManager.enemyPokemons;
        
        //check if there are pokemon alive in the list
        foreach (PokemonModel pokemon in pokemonList)
        {
            //if there is at least one
            if (pokemon.CurrentHealth > 0)
            {
                if (isPlayerPokemon)
                {
                    //show player pokemons
                    fightManager.PokemonClick();
                }
                else
                {
                    //or change pokemon to the enemy
                    fightManager.ChangePokemon(pokemon);
                }

                return;
            }
        }

        //if there are not pokemons alive, call NoMorePokemons
        anim.SetTrigger("NoMorePokemons");
    }
}
