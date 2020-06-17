using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public FightUIManager FightUIManager { get; private set; }

    [Header("Pokemon")]
    [Min(1)] public int minPokemons = 1;
    [Min(1)] public int maxPokemons = 1;
    public List<PokemonModel> enemyPokemons;

    [Header("Skill when everything at 0 PP")]
    public SkillData baseSkill;

    [Header("Efficiency TAB")]
    public EfficiencyTAB efficiencyTAB;

    public PokemonModel currentPlayerPokemon { get; private set; }
    public PokemonModel currentEnemyPokemon { get; private set; }

    #region for states

    [HideInInspector] public SkillModel skillUsed;
    [HideInInspector] public PokemonModel pokemonSelected;
    [HideInInspector] public ItemModel itemUsed;

    #endregion

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        FightUIManager = GetComponent<FightUIManager>();
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

        //TEMP refullo i pokemon del giocatore quando esco dal combattimento
        foreach (PokemonModel pokemon in GameManager.instance.player.PlayerPokemons)
        {
            pokemon.Restore();
        }
    }

    public void BackToPlayerMenu()
    {
        //deactive other menu and active player menu
        FightUIManager.BackToPlayerMenu();
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

    public void UseItem(ItemModel item)
    {
        //remove item
        item.RemoveItem();

        //set item and change state
        itemUsed = item;
        anim.SetTrigger("Item");
    }

    #endregion

    #region others public API

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
