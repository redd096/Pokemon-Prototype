﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PokemonModel : IGetName
{
    #region variables

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
            //if value < 1 set to 0, so there are no problems with UI (when 0,5 hp it show 0 but don't die)
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
    public SkillModel[] CurrentSkills = new SkillModel[4];

    public List<EffectModel> ActiveEffects = new List<EffectModel>();

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
        ExpNextLevel = NecessaryExpForThisLevel(level +1);
        if (level > 1) 
            CurrentExp = NecessaryExpForThisLevel(level);

        ExpCurrentLevel = CurrentExp;

        //random skills based on level
        RandomSkills();

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

    /// <summary>
    /// get damage and check if dead
    /// </summary>
    public void GetDamage(SkillModel skill, PokemonModel pokemonWhoAttack, out string efficiencyText)
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
    }

    /// <summary>
    /// get experience and check level up
    /// </summary>
    public void GetExperience(float experience)
    {
        CurrentExp += experience;

        if(CurrentExp >= ExpNextLevel)
        {
            LevelUp();
        }
    }

    /// <summary>
    /// restore pokemon, reset full values
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

        //insert at start
        EffectModel newEffect = new EffectModel(effect);
        ActiveEffects.Insert(0, newEffect);

        //return added effect
        return newEffect;
    }

    /// <summary>
    /// Remove effect from the list
    /// </summary>
    public void RemoveEffect(EffectData effect)
    {
        //if already in the list, remove it
        foreach (EffectModel e in ActiveEffects)
        {
            if (e.effectData == effect)
            {
                ActiveEffects.Remove(e);
                break;
            }
        }
    }

    #region private API

    void LevelUp()
    {
        ExpCurrentLevel = NecessaryExpForThisLevel(CurrentLevel);
        ExpNextLevel = NecessaryExpForThisLevel(CurrentLevel +1);
        //TODO check evolution and new skills
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
        List<SkillModel> tempSkills = new List<SkillModel>();

        foreach(SPokemonSkill possibleSkill in pokemonData.PossibleSkills)
        {
            bool reachedLimit = tempSkills.Count >= CurrentSkills.Length;

            //try to add (not add always)
            if (TryAddSkill(possibleSkill.level, reachedLimit))
            {
                //if reached limit, remove one skill
                if(reachedLimit)
                {
                    int randomIndex = Random.Range(0, tempSkills.Count);
                    tempSkills.RemoveAt(randomIndex);
                }

                //add this one
                tempSkills.Add(new SkillModel(possibleSkill.skill));
            }
        }

        //set current skills
        CurrentSkills = tempSkills.ToArray();
    }

    bool TryAddSkill(int levelSkill, bool reachedLimit)
    {
        //check level
        if (levelSkill <= CurrentLevel)
        {
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

    #endregion
}
