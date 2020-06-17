using System.Collections;
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
    public float CurrentHealth;
    public float Speed;
    public float PhysicsAttack;
    public float PhysicsDefense;
    public float SpecialAttack;
    public float SpecialDefense;
    public SkillModel[] CurrentSkills = new SkillModel[4];

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
    public void GetDamage(float damage)
    {
        CurrentHealth -= damage;

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
        }
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
