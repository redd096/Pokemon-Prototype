using UnityEngine;

[System.Serializable]
public class SkillModel : IGetName
{
    public SkillData skillData;// { get; private set; }

    public int CurrentPP;

    public SkillModel(SkillData skillData)
    {
        //set data
        this.skillData = skillData;

        //set full pp
        RestorePP();
    }

    public string GetButtonName()
    {
        return skillData.SkillName + " - PP: " + CurrentPP;
    }

    public string GetObjectName()
    {
        return skillData.SkillName;
    }

    public void RestorePP()
    {
        CurrentPP = skillData.PP;
    }

    /// <summary>
    /// Get Efficiency Multiplier
    /// </summary>
    public float EfficiencyMultiplier(EType pokemonToAttackType)
    {
        //get from the matrix in fight manager
        return GameManager.instance.levelManager.FightManager.efficiencyTAB.PokemonArray[(int)pokemonToAttackType].SkillArray[(int)skillData.SkillType];
    }

    /// <summary>
    /// Same Type Attack Bonus (skill same type as pokemon)
    /// </summary>
    public float STAB(EType pokemonType)
    {
        //if same type of skill and pokemon, x1.5
        if (skillData.SkillType == pokemonType)
            return 1.5f;

        //else x1
        return 1.0f;
    }

    /// <summary>
    /// N Multiplier (Random between 0.85 and 1)
    /// </summary>
    public float NRandom()
    {
        return Random.Range(0.85f, 1.0f);
    }
}
