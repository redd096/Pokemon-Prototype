using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonDeadState : FightManagerState
{
    [Header("Is Player Pokemon Who is Dead")]
    [SerializeField] bool isPlayerPokemon = true;

    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "{PlayerPokemon} è esausto";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show description enemy dead
        //check if there are pokemon alives, else call NoMorePokemons
        //if there are pokemon alives, show player menu or select new enemy pokemon

        SetDescription();
    }

    #region enter

    void SetDescription()
    {
        //deactive menu, to be sure to read description
        fightManager.FightUIManager.DeactiveMenu();

        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(description, OnEndDescription);
    }

    void OnEndDescription()
    {
        //deactive description
        fightManager.FightUIManager.EndDescription();

        //check pokemons
        CheckPokemons();
    }

    #endregion

    void CheckPokemons()
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
