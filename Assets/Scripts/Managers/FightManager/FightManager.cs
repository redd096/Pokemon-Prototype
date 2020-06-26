using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public FightUIManager FightUIManager { get; private set; }

    [Header("Pokemon")]
    public List<PokemonModel> enemyPokemons;

    [Header("Skill when everything at 0 PP")]
    public SkillData baseSkill;

    #region for states

    public PokemonModel currentPlayerPokemon { get; private set; }
    public PokemonModel currentEnemyPokemon { get; private set; }

    public List<PokemonModel> pokemonsWhoFought { get; private set; }

    public SkillModel SkillUsed { get; private set; }
    public PokemonModel PokemonSelected { get; private set; }
    public ItemModel ItemUsed { get; private set; }

    public SkillModel SkillToLearn { get; private set; }

    #endregion

    Animator anim;

    private void Start()
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
        GameManager.instance.LevelManager.StartMoving();

        //TEMP refullo i pokemon del giocatore quando esco dal combattimento
        foreach (PokemonModel pokemon in GameManager.instance.Player.PlayerPokemons)
        {
            pokemon.Restore();
        }
    }

    public void BackButton()
    {
        //only if not paused
        if (Time.timeScale == 0)
            return;

        //only if player pokemon is alive (because pokemon menu is shown also when the player has to replace his dead pokemon)
        if (currentPlayerPokemon.CurrentHealth > 0)
        {
            //deactive other menu and active player menu
            FightUIManager.BackButton();
        }
    }

    #endregion

    #region other buttons

    public void UseSkill(SkillModel skill)
    {
        //remove PP
        skill.CurrentPP--;

        //set skill and change state
        SkillUsed = skill;
        anim.SetTrigger("Skill");
    }

    public void ChangePokemon(PokemonModel pokemon)
    {
        //set pokemon and change state
        PokemonSelected = pokemon;
        anim.SetTrigger("Pokemon");
    }

    public void UseItem(ItemModel item)
    {
        //remove item from player inventory
        GameManager.instance.Player.RemoveItem(item);

        //set item and change state
        ItemUsed = item;
        anim.SetTrigger("Item");
    }

    public void LearnSkill(SkillModel skill)
    {
        //set skill to learn and change state
        SkillToLearn = skill;
        anim.SetTrigger("LearnSkill");
    }

    #endregion

    #region others public API

    public void ResetPokemonsWhoFought()
    {
        pokemonsWhoFought = new List<PokemonModel>();
    }

    public void SetCurrentPlayerPokemon(PokemonModel pokemon, bool isEvolving = false)
    {
        //set player pokemon in arena
        currentPlayerPokemon = pokemon;

        //add to list only if is new pokemon and not evolution
        if (isEvolving == false)
        {
            //if not alreasy in the list, add 
            if (pokemonsWhoFought.Contains(pokemon) == false)
                pokemonsWhoFought.Add(pokemon);
        }
    }

    public void SetCurrentEnemyPokemon(PokemonModel pokemon)
    {
        //set enemy pokemon in arena
        currentEnemyPokemon = pokemon;
    }

    #endregion
}
