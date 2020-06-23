using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFightState : FightManagerState
{
    [Header("Player Won?")]
    [SerializeField] bool isWin = true;

    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "Hai finito i pokemon...";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //set description win or lose
        //if lose, end fight
        //if win, get experience, level up, check new skills and get items    

        SetDescription();
        GetExperience();
    }

    #region enter

    void SetDescription()
    {
        //deactive menu, to be sure to read description
        fightManager.FightUIManager.DeactiveMenu();

        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(description, OnEndDescription);
    }

    void GetExperience()
    {
        //only if win
        if (isWin == false)
            return;

        //every player pokemon who fought get experience
        foreach(PokemonModel pokemon in fightManager.pokemonsWhoFought)
        {
            //get experience from every enemy pokemon
            foreach(PokemonModel enemyPokemon in fightManager.enemyPokemons)
            {
                pokemon.GetExperience(true, enemyPokemon.pokemonData.ExperienceOnDeath, enemyPokemon.CurrentLevel, fightManager.pokemonsWhoFought.Count);
            }
        }
    }

    #endregion

    void OnEndDescription()
    {
        //deactive description
        fightManager.FightUIManager.EndDescription();

        //TODO
        //INVECE DI FARE RUNCLICK, DOVREBBE MOSTRARE LA BARRA DELL'EXP CHE SI AGGIORNA GRADUALMENTE (COME UPDATE HEALTH)
        //IN CASO DI LEVEL UP DEVE AGGIORNARSI E VEDERE ANCHE SE IMPARA NUOVE SKILL 
        //DOVREBBE RACCOGLIERE ANCHE OGGETTI

        //end fight
        fightManager.RunClick();
    }
}
