using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaughtState : FightManagerState
{
    [Header("Description")]
    [TextArea()] [SerializeField] string description = "Scegli uno slot per {EnemyPokemon}.";
    [TextArea()] [SerializeField] string confirmDescription = "{EnemyPokemon} ora è nel tuo team.";
    [TextArea()] [SerializeField] string replacePokemonDescription = "{0} non fa più parte del tuo team...";
    [TextArea()] [SerializeField] string refuseDescription = "{EnemyPokemon} non farà parte del tuo team...";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show description and set list of pokemons to replace
        //then go to victory or enemy change pokemon

        SetDescription();
        fightManager.FightUIManager.SetCatchPokemonMenu(AddPokemon, RefusePokemon);
    }

    #region enter

    void SetDescription()
    {
        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(description, OnEndDescription);
    }

    void OnEndDescription()
    {
        //show pokemons to replace
        fightManager.FightUIManager.ShowCatchPokemonMenu();
    }

    #endregion

    void AddPokemon(int index)
    {
        //remove menu
        fightManager.FightUIManager.HideCatchPokemonMenu();

        //get pokemon to replace
        PokemonModel pokemonToReplace = index < GameManager.instance.Player.PlayerPokemons.Count ? GameManager.instance.Player.PlayerPokemons[index] : null;

        //add or replace pokemon
        GameManager.instance.Player.AddPokemon(fightManager.currentEnemyPokemon, index);

        //if need to replace a pokemon, show replacePokemonDescription before confirm
        if (pokemonToReplace != null)
        {
            string s = Utility.Parse(replacePokemonDescription, pokemonToReplace.GetObjectName());

            fightManager.FightUIManager.SetDescription(s, ConfirmPokemonDescription);
        }
        //else show immediatly the confirm
        else
        {
            ConfirmPokemonDescription();
        }
    }

    void RefusePokemon()
    {
        //remove menu
        fightManager.FightUIManager.HideCatchPokemonMenu();

        //show refuse description
        fightManager.FightUIManager.SetDescription(refuseDescription, CheckPokemons);
    }

    void ConfirmPokemonDescription()
    {
        //show confirm description
        fightManager.FightUIManager.SetDescription(confirmDescription, CheckPokemons);
    }

    void CheckPokemons()
    {
        //remove pokemon from enemy list
        fightManager.enemyPokemons.Remove(fightManager.currentEnemyPokemon);

        //check if there are pokemon alive in the list
        foreach (PokemonModel pokemon in fightManager.enemyPokemons)
        {
            //if there is at least one
            if (pokemon.CurrentHealth > 0)
            {
                //change enemy pokemon
                fightManager.ChangePokemon(pokemon);

                return;
            }
        }

        //if there are not pokemons alive, call NoMorePokemons
        anim.SetTrigger("NoMorePokemons");
    }
}
