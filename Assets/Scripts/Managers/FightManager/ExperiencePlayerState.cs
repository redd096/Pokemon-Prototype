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

    float previousExp;
    SkillData skillToLearn;

    PokemonModel pokemonGettingExperience;
    List<PokemonModel> pokemonsWhoGotExperience = new List<PokemonModel>();
    bool alreadyGotExperience;
    bool alreadyCheckedEvolution;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //get experience, level up, check evolution and new skills

        CheckNewPokemon();
    }

    void CheckNewPokemon()
    {
        //get experience
        if (alreadyGotExperience == false)
        {
            //if already checked every pokemon, end fight
            if(pokemonsWhoGotExperience.Count >= fightManager.pokemonsWhoFought.Count)
            {
                EndFight();
                return;
            }

            //else check experience
            GetExperience();
            SetDescription();
        }
        //if already got experience, then we changed state for skill or evolve, so continue to check
        else
        {
            LevelUp();
        }
    }

    #region enter

    void GetExperience()
    {
        PokemonModel lastPokemon = pokemonGettingExperience;

        //get pokemon who fought and not died
        int pokemonsWhoFoughtNotDead = fightManager.pokemonsWhoFought.Where(p => p.CurrentHealth > 0).ToArray().Length;

        //every player pokemon who fought get experience
        foreach (PokemonModel pokemon in fightManager.pokemonsWhoFought)
        {
            //do only if in player list (when catch pokemon can replace pokemon who fought)
            if (GameManager.instance.Player.PlayerPokemons.Contains(pokemon) == false)
            {
                pokemonsWhoGotExperience.Add(pokemon);
                continue;
            }

            //first get experience the pokemon in arena (if in player list)
            if(pokemonGettingExperience == null)
            {
                pokemonGettingExperience = fightManager.currentPlayerPokemon;
                pokemonsWhoGotExperience.Add(pokemonGettingExperience);
                break;
            }
            //then all the others
            else if (pokemonsWhoGotExperience.Contains(pokemon) == false)
            {
                pokemonGettingExperience = pokemon;
                pokemonsWhoGotExperience.Add(pokemon);

                //update UI
                fightManager.SetCurrentPlayerPokemon(pokemon);
                fightManager.FightUIManager.SetPokemonInArena(true);

                break;
            }
        }

        //if == last pokemon, then there are only pokemon which are no more in player list
        if (pokemonGettingExperience == lastPokemon)
        {
            EndFight();
            return;
        }

        //set previous exp
        previousExp = pokemonGettingExperience.CurrentExp;

        //get experience from every enemy pokemon
        foreach (PokemonModel enemyPokemon in fightManager.enemyPokemons)
        {
            pokemonGettingExperience.GetExperience(true, enemyPokemon, pokemonsWhoFoughtNotDead);
        }
    }

    void SetDescription()
    {
        //deactive menu, to be sure to read description
        fightManager.FightUIManager.DeactiveMenu();

        //get experience got
        float experienceGot = pokemonGettingExperience.CurrentExp - previousExp;
        string _description = Utility.Parse(description, experienceGot.ToString("F0"));

        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(_description, OnEndDescription);
    }

    void OnEndDescription()
    {
        //update experience bar, then call check level up
        fightManager.FightUIManager.UpdateExperience(previousExp, CheckLevelUp);
    }

    #endregion

    #region check level up

    void CheckLevelUp(float updatedExp)
    {
        //try level up
        bool levelUp = pokemonGettingExperience.TryLevelUp();

        if (levelUp)
        {
            //update UI
            fightManager.FightUIManager.SetPokemonInArena(true);

            //try update again experience bar, then re-call check level up
            fightManager.FightUIManager.UpdateExperience(updatedExp, CheckLevelUp);
        }
        //if not level up, we don't need to update experience anymore, so check skills and evolution
        else
        {
            alreadyGotExperience = true;

            LevelUp();
        }
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
        pokemonGettingExperience.RefuseSkill(skillToLearn);

        //get skill name
        string _refuseSkill = Utility.Parse(refuseSkill, skillToLearn.SkillName);

        //description, then check other skills
        fightManager.FightUIManager.SetDescription(_refuseSkill, LevelUp);
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

        PokemonModel evolution = pokemonGettingExperience.GetEvolution();

        //replace pokemon in player list
        GameManager.instance.Player.PokemonEvolution(pokemonGettingExperience, evolution);

        //evolve pokemon (change state to do animation)
        fightManager.ChangePokemon(evolution);
    }

    void NopeEvolution()
    {
        //remove menu
        fightManager.FightUIManager.HideYesNoMenu();

        //get evolution name
        string _refuseEvolution = Utility.Parse(refuseEvolution, pokemonGettingExperience.pokemonData.PokemonEvolution.PokemonName);

        //description, then check skills
        fightManager.FightUIManager.SetDescription(_refuseEvolution, LevelUp);
    }

    #endregion

    void LevelUp()
    {
        string tempDescription = string.Empty;

        //check if can learn new skill
        skillToLearn = pokemonGettingExperience.CanLearnSkill();
        if (skillToLearn)
        {
            //get skill name
            string _questionSkill = Utility.Parse(questionSkill, skillToLearn.SkillName);

            fightManager.FightUIManager.SetDescription(_questionSkill, ShowYesNoLearnSkill);
            return;
        }

        //else check if can evolve
        if (alreadyCheckedEvolution == false && pokemonGettingExperience.CheckEvolution())
        {
            alreadyCheckedEvolution = true;

            //get evolution name
            string _questionEvolution = Utility.Parse(questionEvolution, pokemonGettingExperience.pokemonData.PokemonEvolution.PokemonName);

            fightManager.FightUIManager.SetDescription(_questionEvolution, ShowYesNoEvolution);
            return;
        }

        //if no evolution and no skills, then check new pokemon
        alreadyGotExperience = false;
        alreadyCheckedEvolution = false;
        CheckNewPokemon();
    }

    void EndFight()
    {
        //reset 
        pokemonGettingExperience = null;
        pokemonsWhoGotExperience.Clear();
        alreadyGotExperience = false;
        alreadyCheckedEvolution = false;

        //HACK
        fightManager.RunClick();
    }
}
