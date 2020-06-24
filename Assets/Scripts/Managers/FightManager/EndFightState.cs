using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EndFightState : FightManagerState
{
    [Header("Player Won?")]
    [SerializeField] bool isWin = true;

    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "Hai finito i pokemon...";

    [Header("Update Experience")]
    [SerializeField] float durationUpdateExperience = 0.7f;

    [Header("Animation Despawn")]
    [SerializeField] float durationDespawn = 0.5f;

    [Header("Animation Spawn")]
    [SerializeField] float durationSpawn = 1.5f;

    float previousExp;
    PokemonModel pokemonPlayer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //set description win or lose
        //if lose, end fight
        //if win, get experience, level up, check new skills and get items

        pokemonPlayer = fightManager.currentPlayerPokemon;

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

        int pokemonsWhoFoughtNotDead = fightManager.pokemonsWhoFought.Where(p => p.CurrentHealth > 0).ToArray().Length;

        //every player pokemon who fought get experience
        foreach(PokemonModel pokemon in fightManager.pokemonsWhoFought)
        {
            //set previous exp for pokemon in arena
            if (pokemon == pokemonPlayer)
                previousExp = pokemon.CurrentExp;

            //get experience from every enemy pokemon
            foreach(PokemonModel enemyPokemon in fightManager.enemyPokemons)
            {
                pokemon.GetExperience(true, enemyPokemon.pokemonData.ExperienceOnDeath, enemyPokemon.CurrentLevel, pokemonsWhoFoughtNotDead);
            }
        }
    }

    void OnEndDescription()
    {
        //deactive description
        fightManager.FightUIManager.EndDescription();

        //if win, show experience got
        if (isWin)
        {
            //set Description letter by letter, then call UpdateExperience
            float experienceGot = pokemonPlayer.CurrentExp - previousExp;
            fightManager.FightUIManager.SetDescription(pokemonPlayer + " ottiene " + experienceGot + " punti esperienza.", UpdateExperience);

            return;
        }

        //else end fight
        EndFight();
    }

    #endregion

    #region update experience

    void UpdateExperience()
    {
        //update experience bar, then call check level up
        fightManager.FightUIManager.UpdateExperience(previousExp, durationUpdateExperience, CheckLevelUp);
    }

    void CheckLevelUp(float updatedExp)
    {
        //update previous exp
        previousExp = updatedExp;

        //try level up
        bool levelUp = pokemonPlayer.CheckLevelUp();

        if (levelUp)
        {
            //update level UI
            fightManager.FightUIManager.UpdateLevel(pokemonPlayer.CurrentLevel);

            //check evolution, learn skill, and if still need to update experience
            LevelUp(true);
        }
        //if not level up, we don't need to update experience anymore, so end fight
        else
        {
            EndFight();
        }
    }

    void LevelUp(bool checkEvolution)
    {
        //check if can evolve
        if (checkEvolution && pokemonPlayer.CheckEvolution())
        {
            fightManager.FightUIManager.ShowYesNoMenu(YesEvolution, NopeEvolution);
        }
        //else check if can learn new skill
        else if (pokemonPlayer.CanLearnSkill())
        {
            fightManager.FightUIManager.ShowYesNoMenu(YesLearnSkill, NopeLearnSkill);
        }
        //else check if still need to update experience
        else
        {
            UpdateExperience();
        }
    }

    #endregion

    #region evolution

    void YesEvolution()
    {
        //create evolution pokemon model, same level
        PokemonModel evolution = new PokemonModel(pokemonPlayer.pokemonData.PokemonEvolution, pokemonPlayer.CurrentLevel);

        //copy current skills to new evolution
        SkillModel[] playerSkills = pokemonPlayer.CurrentSkills.CreateCopy();
        evolution.CurrentSkills = playerSkills;

        //replace pokemon in player list
        GameManager.instance.player.ReplacePokemon(pokemonPlayer, evolution);

        SetEvolutionDescription();

        //come back to level up, but not check evolution 
        LevelUp(false);
    }

    void NopeEvolution()
    {
        //come back to level up, but not check evolution 
        LevelUp(false);
    }

    #region evolution description

    void SetEvolutionDescription()
    {
        //deactive menu, to be sure to read description
        fightManager.FightUIManager.DeactiveMenu();

        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(description, OnEndEvolutionDescription);
    }

    void OnEndEvolutionDescription()
    {
        //deactive description
        fightManager.FightUIManager.EndDescription();

        //start animation
        fightManager.FightUIManager.StartAnimation(RemovePokemon());
    }

    #endregion

    #region evolution animations

    IEnumerator RemovePokemon()
    {
        float delta = 1;

        //reduce animation
        while (delta > 0)
        {
            delta -= Time.deltaTime / durationDespawn;
            fightManager.FightUIManager.PokemonSpawnAnimation(true, delta);
            yield return null;
        }

        //end animation and change pokemon
        fightManager.FightUIManager.EndAnimation();
        NextPokemon();
    }

    IEnumerator SpawnNextPokemon()
    {
        float delta = 0;

        //increase animation
        while (delta < 1)
        {
            delta += Time.deltaTime / durationSpawn;
            fightManager.FightUIManager.PokemonSpawnAnimation(isPlayer, delta);
            yield return null;
        }

        //be sure to end animation
        fightManager.FightUIManager.PokemonSpawnAnimation(isPlayer, 1);

        //end animation and change pokemon
        fightManager.FightUIManager.EndAnimation();
        EndTurn();
    }

    #endregion

    void NextPokemon()
    {
        //set new pokemon
        fightManager.SetCurrentPlayerPokemon(fightManager.PokemonSelected);

        //set UI new pokemon
        fightManager.FightUIManager.SetPokemonInArena(isPlayer);

        //if is player turn, set new skill
        if (isPlayer)
            fightManager.FightUIManager.SetSkillsList(fightManager.currentPlayerPokemon);

        //start animation
        fightManager.FightUIManager.StartAnimation(SpawnNextPokemon());
    }

    #endregion

    #region learn skill

    void YesLearnSkill()
    {

    }

    void NopeLearnSkill()
    {
        //check if still need to update experience
        UpdateExperience();
    }

    #endregion

    void EndFight()
    {
        //TODO
        //DOVREBBE RACCOGLIERE ANCHE OGGETTI

        //HACK
        fightManager.RunClick();
    }
}
