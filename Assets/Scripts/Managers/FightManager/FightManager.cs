using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public FightUIManager FightUIManager { get; private set; }

    [Header("TODO")]
    public PokemonData[] enemyPokemonsData;
    public PokemonModel[] enemyPokemons;

    [Header("Skill when everything at 0 PP")]
    public SkillData baseSkill;

    public PokemonModel currentPlayerPokemon { get; private set; }
    public PokemonModel currentEnemyPokemon { get; private set; }

    #region for states
    
    public SkillModel skillUsed { get; private set; }
    public PokemonModel pokemonSelected { get; private set; }
    public ItemData itemUsed { get; private set; }

    #endregion

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        FightUIManager = GetComponent<FightUIManager>();

        //foreach data, create a pokemon model
        enemyPokemons = new PokemonModel[enemyPokemonsData.Length];

        for (int i = 0; i < enemyPokemonsData.Length; i++)
            enemyPokemons[i] = new PokemonModel(enemyPokemonsData[i], 5);
    }

    #region state machine

    public void SetupState()
    {
        anim.SetTrigger("Setup");
    }

    public void StartFightPhase()
    {
        anim.SetTrigger("StartFightPhase");
    }

    #endregion

    #region player menu

    public void FightClick()
    {
        //check if there is at least one skill with PP > 0
        foreach(SkillModel skill in currentPlayerPokemon.CurrentSkills)
        {
            if (skill.CurrentPP > 0)
            {
                //if can fight (there is at least one skill with PP > 0) go from player menu to fight menu
                FightUIManager.FightClick();
                return;
            }
        }

        //if every skill has PP 0 then use baseSkill
        UseSkill(new SkillModel(baseSkill));
    }

    public void PokemonClick()
    {
        //from player menu to pokemon menu
        FightUIManager.PokemonClick();
    }

    public void BagClick()
    {
        //from player menu to bag menu
        FightUIManager.BagClick();
    }

    public void RunClick()
    {
        //remove player menu, cause the player will try to run away
        FightUIManager.RunClick();
        GameManager.instance.levelManager.StartMoving();
    }

    #endregion

    #region other buttons

    public void UseSkill(SkillModel skill)
    {
        //remove PP
        skill.CurrentPP--;

        //set skill and change state
        skillUsed = skill;
        anim.SetTrigger("Skill");
    }

    public void ChangePokemon(PokemonModel pokemon)
    {
        //set pokemon and change state
        pokemonSelected = pokemon;
        anim.SetTrigger("Pokemon");
    }

    public void UseItem(ItemData item)
    {
        //set item and change state
        itemUsed = item;
        anim.SetTrigger("Item");
    }

    #endregion

    #region others public API

    public void BackToPlayerMenu()
    {
        //deactive other menu and active player menu
        FightUIManager.BackToPlayerMenu();
    }

    public void SetCurrentPlayerPokemon(PokemonModel pokemon)
    {
        //set player pokemon in arena
        currentPlayerPokemon = pokemon;
    } 

    public void SetCurrentEnemyPokemon(PokemonModel pokemon)
    {
        //set enemy pokemon in arena
        currentEnemyPokemon = pokemon;
    }

    #endregion
}
