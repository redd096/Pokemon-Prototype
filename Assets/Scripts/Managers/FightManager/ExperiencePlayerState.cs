using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ExperiencePlayerState : FightManagerState
{
    [Header("Description")]
    [TextArea()] [SerializeField] string description = "{PlayerPokemon} ottiene {0} punti esperienza.";
    [TextArea()] [SerializeField] string questionEvolution = "{PlayerPokemon} vuole evolversi in {0}";
    [TextArea()] [SerializeField] string refuseEvolution = "{PlayerPokemon} non si evolve in {0}...";
    [TextArea()] [SerializeField] string questionSkill = "{PlayerPokemon} vuole imparare {0}";
    [TextArea()] [SerializeField] string refuseSkill = "{PlayerPokemon} non apprende {0}...";

    [Header("Update Experience")]
    [SerializeField] float durationUpdateExperience = 0.7f;

    float previousExp;
    PokemonModel pokemonPlayer;
    bool alreadyGotExperience;
    SkillData skillToLearn;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //get experience, level up, check evolution and new skills

        pokemonPlayer = fightManager.currentPlayerPokemon;

        //get experience and start descrption, then check level up
        if (alreadyGotExperience == false)
        {
            GetExperience();
            SetDescription();
        }
        //if already got experience, then we changed state to evolve pokemon -> check skills now
        else
        {
            CheckSkillsToLearn();
        }
    }

    string Parse(string text, string value)
    {
        if (text.Contains("{0}"))
            return text.Replace("{0}", value);

        return text;
    }

    #region enter

    void GetExperience()
    {
       int pokemonsWhoFoughtNotDead = fightManager.pokemonsWhoFought.Where(p => p.CurrentHealth > 0).ToArray().Length;

        //every player pokemon who fought get experience
        foreach (PokemonModel pokemon in fightManager.pokemonsWhoFought)
        {
            //set previous exp for pokemon in arena
            if (pokemon == pokemonPlayer)
            {
                previousExp = pokemon.CurrentExp;
            }

            //get experience from every enemy pokemon
            foreach (PokemonModel enemyPokemon in fightManager.enemyPokemons)
            {
                pokemon.GetExperience(true, enemyPokemon.pokemonData.ExperienceOnDeath, enemyPokemon.CurrentLevel, pokemonsWhoFoughtNotDead);
            }
        }
    }

    void SetDescription()
    {
        //deactive menu, to be sure to read description
        fightManager.FightUIManager.DeactiveMenu();

        //get experience got
        float experienceGot = pokemonPlayer.CurrentExp - previousExp;
        string _description = Parse(description, experienceGot.ToString("F0"));

        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(_description, OnEndDescription);
    }

    void OnEndDescription()
    {
        fightManager.FightUIManager.EndDescription();

        //update experience bar, then call check level up
        fightManager.FightUIManager.UpdateExperience(previousExp, durationUpdateExperience, CheckLevelUp);
    }

    #endregion

    #region level up

    void CheckLevelUp(float updatedExp)
    {
        //try level up
        bool levelUp = pokemonPlayer.TryLevelUp();

        if (levelUp)
        {
            //update level UI
            fightManager.FightUIManager.UpdateLevel(pokemonPlayer.CurrentLevel);

            //try update again experience bar, then re-call check level up
            fightManager.FightUIManager.UpdateExperience(updatedExp, durationUpdateExperience, CheckLevelUp);
        }
        //if not level up, we don't need to update experience anymore, so check evolution and skills
        else
        {
            alreadyGotExperience = true;

            LevelUp(true);
        }
    }

    void CheckSkillsToLearn()
    {
        LevelUp(false);
    }

    #endregion

    #region evolution

    void ShowYesNoEvolution()
    {
        fightManager.FightUIManager.ShowYesNoMenu(YesEvolution, NopeEvolution);
    }

    void YesEvolution()
    {
        //remove menu
        fightManager.FightUIManager.HideYesNoMenu();

        PokemonModel evolution = pokemonPlayer.GetEvolution();

        //replace pokemon in player list
        GameManager.instance.player.ReplacePokemon(pokemonPlayer, evolution);

        //change pokemon (change state to do animation)
        fightManager.ChangePokemon(evolution);
    }

    void NopeEvolution()
    {
        //remove menu
        fightManager.FightUIManager.HideYesNoMenu();

        //get evolution name
        string _refuseEvolution = Parse(refuseEvolution, pokemonPlayer.pokemonData.PokemonEvolution.PokemonName);

        //description, then check skills
        fightManager.FightUIManager.SetDescription(_refuseEvolution, CheckSkillsToLearn);
    }

    #endregion

    #region learn skill

    void ShowYesNoLearnSkill()
    {
        fightManager.FightUIManager.ShowYesNoMenu(YesSkill, NopeSkill);
    }

    void YesSkill()
    {
        //remove menu
        fightManager.FightUIManager.HideYesNoMenu();

        SkillModel skill = new SkillModel(skillToLearn);

        //learn skill (change state to do it)
        fightManager.LearnSkill(skill);
    }

    void NopeSkill()
    {
        //remove menu
        fightManager.FightUIManager.HideYesNoMenu();

        //refuse skill
        pokemonPlayer.RefuseSkill(skillToLearn);

        //get skill name
        string _refuseSkill = Parse(refuseSkill, skillToLearn.SkillName);

        //description, then check other skills
        fightManager.FightUIManager.SetDescription(_refuseSkill, CheckSkillsToLearn);
    }

    #endregion

    void LevelUp(bool checkEvolution)
    {
        string tempDescription = string.Empty;

        //check if can evolve
        if (checkEvolution && pokemonPlayer.CheckEvolution())
        {
            //get evolution name
            string _questionEvolution = Parse(questionEvolution, pokemonPlayer.pokemonData.PokemonEvolution.PokemonName);

            fightManager.FightUIManager.SetDescription(_questionEvolution, ShowYesNoEvolution);
            return;
        }

        //else check if can learn new skill
        skillToLearn = pokemonPlayer.CanLearnSkill();
        if (skillToLearn)
        {
            //get skill name
            string _questionSkill = Parse(questionSkill, skillToLearn.SkillName);

            fightManager.FightUIManager.SetDescription(_questionSkill, ShowYesNoLearnSkill);
            return;
        }

        //if no evolution and no skills, then end fight
        EndFight();
    }

    void EndFight()
    {
        //reset 
        alreadyGotExperience = false;

        //HACK
        fightManager.RunClick();
    }
}
