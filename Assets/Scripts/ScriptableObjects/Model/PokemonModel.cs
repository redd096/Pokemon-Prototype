﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonModel : IGetName
{
    #region public variables

    public PokemonData pokemonData;// { get; private set; }

    public int CurrentLevel;// { get; private set; }

    public float CurrentExp;// { get; private set; }
    public float ExpCurrentLevel;
    public float ExpNextLevel;// { get; private set; }

    //fight
    float currentHealth;
    public float CurrentHealth
    { 
        get
        { 
            return currentHealth; 
        } 
        set
        { 
            //if value < 1 set to 0, so there are no problems with UI (like when is 0,5 hp and it shows 0 but don't die)
            if (value < 1) 
                currentHealth = 0; 
            //clamp 0 (dead) or data.health (full hp)
            else 
                currentHealth = Mathf.Clamp(value, 0, pokemonData.Health); 
        } 
    }
    public float Speed;
    public float PhysicsAttack;
    public float PhysicsDefense;
    public float SpecialAttack;
    public float SpecialDefense;
    public List<SkillModel> CurrentSkills = new List<SkillModel>();

    public List<EffectModel> ActiveEffects = new List<EffectModel>();
    public List<EffectModel> RemovedEffects = new List<EffectModel>();
    [Range(0, 100)] public int CurrentMaxAccuracy;
    #endregion

    #region private variables

    List<SPokemonSkill> skillsLearnedOrRefused = new List<SPokemonSkill>();

    #endregion

    /// <summary>
    /// set default values
    /// </summary>
    public PokemonModel(PokemonData pokemonData, int level)
    {
        //set data
        this.pokemonData = pokemonData;

        //set level and exp
        CurrentLevel = level;
        ExpNextLevel = NecessaryExpForThisLevel(level + 1);
        if (level > 1)
            ExpCurrentLevel = NecessaryExpForThisLevel(level);

        //reset to start of this level
        CurrentExp = ExpCurrentLevel;

        //random skills based on level
        RandomSkills();

        Restore();
    }

    /// <summary>
    /// set default values from another pokemon (used from GetEvolution())
    /// </summary>
    PokemonModel(PokemonModel previousPokemon)
    {
        //set data
        pokemonData = previousPokemon.pokemonData.PokemonEvolution;

        //set level and exp
        int level = previousPokemon.CurrentLevel;
        CurrentLevel = level;
        ExpNextLevel = NecessaryExpForThisLevel(level + 1);
        if(level > 1)
            ExpCurrentLevel = NecessaryExpForThisLevel(level);

        //get other pokemon exp
        CurrentExp = previousPokemon.CurrentExp;

        //get a copy of the skills 
        CurrentSkills = previousPokemon.CurrentSkills.CreateCopy();
        skillsLearnedOrRefused = previousPokemon.skillsLearnedOrRefused.CreateCopy();

        Restore();
    }

    public string GetButtonName()
    {
        return pokemonData.PokemonName + " - " + CurrentHealth.ToString("F0") + "/" + pokemonData.Health.ToString("F0");
    }

    public string GetObjectName()
    {
        return pokemonData.PokemonName;
    }

    #region fight and restore

    /// <summary>
    /// get damage and add effect (if skill has one)
    /// </summary>
    public void GetDamage(SkillModel skill, PokemonModel pokemonWhoAttack, out string efficiencyText)
    {
        int random = Random.Range(0, 100);
        int accuracy = Mathf.Min(CurrentMaxAccuracy, skill.skillData.Accuracy);

        //try to hit enemy
        if (random < accuracy)
        {
            //get attack and defense, based to the skill if special or physics
            float attack = skill.skillData.IsSpecial ? pokemonWhoAttack.SpecialAttack : pokemonWhoAttack.PhysicsAttack;
            float defense = skill.skillData.IsSpecial ? SpecialDefense : PhysicsDefense;

            //get multipliers -> out efficiencyText
            float efficiencyMultiplier = skill.EfficiencyMultiplier(pokemonData.PokemonType, out efficiencyText);
            float stab = skill.STAB(pokemonWhoAttack.pokemonData.PokemonType);
            float nRandom = skill.NRandom();

            //((( (2 * Livello Pokemon + 10) * Attacco Pokemon * Potenza Mossa ) / (250 * Difesa Fisica o Difesa Speciale del Nemico)) +2 ) * Efficacia * STAB * N
            float damage = ((((2 * pokemonWhoAttack.CurrentLevel + 10) * attack * skill.skillData.Power) / (250 * defense)) + 2) * efficiencyMultiplier * stab * nRandom;

            //if the skill has one, add effect to the list
            AddEffect(skill.skillData.Effect);

            //apply effective damage
            CurrentHealth -= damage;
            return;
        }

        //else miss
        efficiencyText = "Ups, mancato!";
    }

    /// <summary>
    /// restore pokemon, reset full values (restore also skills and remove effects)
    /// </summary>
    public void Restore()
    {
        CurrentHealth = pokemonData.Health;
        Speed = pokemonData.Speed;
        PhysicsAttack = pokemonData.PhysicsAttack;
        PhysicsDefense = pokemonData.PhysicsDefense;
        SpecialAttack = pokemonData.SpecialAttack;
        SpecialDefense = pokemonData.SpecialDefense;

        //restore skills
        foreach(SkillModel skill in CurrentSkills)
        {
            skill.RestorePP();
        }

        //remove effects
        ActiveEffects.Clear();
        RemovedEffects.Clear();
        CurrentMaxAccuracy = 100;
    }

    /// <summary>
    /// Add effect to the list
    /// </summary>
    public EffectModel AddEffect(EffectData effect)
    {
        //if no effect, return null
        if (effect == null)
            return null;

        //if already in the list, remove it
        foreach(EffectModel e in ActiveEffects)
        {
            if(e.effectData == effect)
            {
                ActiveEffects.Remove(e);
                break;
            }
        }

        //remove from removed effects too
        foreach(EffectModel e in RemovedEffects)
        {
            if(e.effectData == effect)
            {
                RemovedEffects.Remove(e);
                break;
            }
        }

        //insert at start
        EffectModel newEffect = new EffectModel(effect);
        ActiveEffects.Insert(0, newEffect);

        //return added effect
        return newEffect;
    }

    /// <summary>
    /// Remove effect from the list and add to removed effects
    /// </summary>
    public void RemoveEffect(EffectData effect)
    {
        //if already in the list, remove it and add to RemovedEffects
        foreach (EffectModel e in ActiveEffects)
        {
            if (e.effectData == effect)
            {
                RemovedEffects.Add(e);
                ActiveEffects.Remove(e);
                break;
            }
        }
    }

    #endregion

    #region experience and level up

    /// <summary>
    /// get experience
    /// </summary>
    /// <param name="isWildPokemon">is the enemy a wild pokemon or a trainer?</param>
    /// <param name="experience">experienceOnDeath from enemy pokemon data</param>
    /// <param name="levelEnemyPokemon">enemy pokemon current level</param>
    /// <param name="numberPokemonsNotDeadInFight">every pokemon who fought but didn't die</param>
    public void GetExperience(bool isWildPokemon, float experience, int levelEnemyPokemon, int numberPokemonsNotDeadInFight)
    {
        //is wild pokemon from bool to float multiplier
        float multiplierPokemonTrainer = isWildPokemon ? 1 : 1.5f;

        //calculate experience and add to CurrentExp
        float experienceToGet = (multiplierPokemonTrainer * experience * levelEnemyPokemon) / (7 * numberPokemonsNotDeadInFight);
        CurrentExp += experienceToGet;
    }

    /// <summary>
    /// check if level up. If true level up and update ExpCurrentLevel and ExpNextLevel
    /// </summary>
    public bool TryLevelUp()
    {
        //check level up
        if (CurrentExp >= ExpNextLevel)
        {
            LevelUp();
            return true;
        }

        return false;
    }

    #endregion

    #region evolution

    /// <summary>
    /// check if pokemon can evolve
    /// </summary>
    public bool CheckEvolution()
    {
        //if has an evolution and current level is >= return true
        if(pokemonData.PokemonEvolution != null && CurrentLevel >= pokemonData.EvolutionLevel)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// get model of evolution pokemon
    /// </summary>
    public PokemonModel GetEvolution()
    {
        //create evolution pokemon model, same level
        return new PokemonModel(this);
    }

    #endregion

    #region learn skill

    /// <summary>
    /// check if can learn new skill
    /// </summary>
    public SkillData CanLearnSkill()
    {
        //check every possible skill for this pokemon
        foreach(SPokemonSkill skill in pokemonData.PossibleSkills)
        {
            //if there is a skill for this level and is not in "learned or refused", then return the skill data
            if (skill.level <= CurrentLevel && skillsLearnedOrRefused.Contains(skill) == false)
            {
                //if no skill setted, then add to "learned or refused" and skip
                if (skill.skill == null)
                {
                    skillsLearnedOrRefused.Add(skill);
                    continue;
                }

                return skill.skill;
            }
        }

        return null;
    }

    /// <summary>
    /// learn skill
    /// </summary>
    public void LearnSkill(SkillModel skillToLearn, int skillToReplace)
    {
        //add to learned or refused list
        SetSkillLearnedOrRefused(skillToLearn.skillData);

        //replace skill with new one
        if (skillToReplace < CurrentSkills.Count)
        {
            CurrentSkills[skillToReplace] = skillToLearn;
        }
        //or add to the list
        else
        {
            CurrentSkills.Add(skillToLearn);
        }
    }

    /// <summary>
    /// refuse skill
    /// </summary>
    public void RefuseSkill(SkillData skillToRefuse)
    {
        SetSkillLearnedOrRefused(skillToRefuse);
    }

    #endregion

    #region private API

    void LevelUp()
    {
        //update level
        CurrentLevel++;

        //update necessary exp
        ExpCurrentLevel = NecessaryExpForThisLevel(CurrentLevel);
        ExpNextLevel = NecessaryExpForThisLevel(CurrentLevel + 1);
    }

    float NecessaryExpForThisLevel(int level)
    {
        //expNextLevel = switch between type of speed experience
        switch (pokemonData.SpeedExperience)
        {
            case ESpeedExperience.fast:
                return (4f / 5f) * Mathf.Pow(level, 3);
            case ESpeedExperience.mid_fast:
                return Mathf.Pow(level, 3);
            case ESpeedExperience.mid_slow:
                return (6f / 5f) * Mathf.Pow(level, 3) - (15 * Mathf.Pow(level, 2)) + (100 * level) - 140;
            case ESpeedExperience.slow:
                return (5f / 4f) * Mathf.Pow(level, 3);
            default:
                return 0;
        }
    }

    void RandomSkills()
    {
        //check every possible skill
        foreach(SPokemonSkill possibleSkill in pokemonData.PossibleSkills)
        {
            bool reachedLimit = CurrentSkills.Count >= GameManager.instance.MaxSkillForPokemon;

            //if no skill setted, then skip it
            if (possibleSkill.skill == null)
                continue;

            //try to add (add until reach limit, then 50% to add)
            if (TryAddSkill(possibleSkill, reachedLimit))
            {
                //if reached limit, remove one skill
                if(reachedLimit)
                {
                    int randomIndex = Random.Range(0, CurrentSkills.Count);
                    CurrentSkills.RemoveAt(randomIndex);
                }

                //add this one
                CurrentSkills.Add(new SkillModel(possibleSkill.skill));
            }
        }
    }

    bool TryAddSkill(SPokemonSkill skill, bool reachedLimit)
    {
        //check level
        if (skill.level <= CurrentLevel)
        {
            //add to the list learned or refused
            skillsLearnedOrRefused.Add(skill);

            int random = Random.Range(0, 100);

            //normally 100%, if reached limit 50% (will replace one skill)
            int percentage = reachedLimit ? 50 : 100;

            //add only if lower then percentage
            if (random < percentage)
            {
                return true;
            }
        }

        return false;
    }

    void SetSkillLearnedOrRefused(SkillData skillLearnedOrRefused)
    {
        if (skillLearnedOrRefused == null)
            return;

        //check every possible skill
        foreach (SPokemonSkill skill in pokemonData.PossibleSkills)
        {
            //if is not already in the list and is the skill to refuse
            if (skillsLearnedOrRefused.Contains(skill) == false && skill.skill == skillLearnedOrRefused)
            {
                //add to refused list
                skillsLearnedOrRefused.Add(skill);
                return;
            }
        }
    }

    #endregion
}
