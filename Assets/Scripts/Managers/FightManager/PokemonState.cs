using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    [Header("Is Evolving")]
    [SerializeField] bool isEvolving = false;

    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "Torna {PlayerPokemon}.\nVai {Pokemon}! Scelgo te!";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show description changing pokemon
        //change pokemon and make animation

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

        //start animation
        fightManager.FightUIManager.PokemonSpawnAnimation(false, isPlayer, NextPokemon);
    }

    #endregion

    void NextPokemon()
    {
        //set new pokemon
        if (isPlayer)
            fightManager.SetCurrentPlayerPokemon(fightManager.PokemonSelected, isEvolving);
        else
            fightManager.SetCurrentEnemyPokemon(fightManager.PokemonSelected);

        //set UI new pokemon
        fightManager.FightUIManager.SetPokemonInArena(isPlayer);

        //if is player turn, set new skill
        if(isPlayer)
            fightManager.FightUIManager.SetSkillsList(fightManager.currentPlayerPokemon);

        //start animation
        fightManager.FightUIManager.PokemonSpawnAnimation(true, isPlayer, EndTurn);
    }

    void EndTurn()
    {
        //change state
        anim.SetTrigger("Next");
    }
}
