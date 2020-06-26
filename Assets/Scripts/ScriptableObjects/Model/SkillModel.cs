using UnityEngine;

[System.Serializable]
public class SkillModel : IGetName
{
    public SkillData skillData;// { get; private set; }

    int currentPP;
    public int CurrentPP 
    { 
        get 
        { 
            return currentPP; 
        } 
        set 
        {
            //clamp from 0 to max PP
            currentPP = Mathf.Clamp(value, 0, skillData.PP);
        }
    }

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

    /// <summary>
    /// Set full PP
    /// </summary>
    public void RestorePP()
    {
        if (skillData == null)
            return;

        CurrentPP = skillData.PP;
    }

    /// <summary>
    /// Get Efficiency Multiplier
    /// </summary>
    public float EfficiencyMultiplier(EType pokemonToAttackType, out string efficiencyText)
    {
        //get from the matrix in fight manager
        float efficiency = GameManager.instance.LevelManager.FightManager.efficiencyTAB.PokemonArray[(int)pokemonToAttackType].SkillArray[(int)skillData.SkillType];

        efficiencyText = EfficiencyText(efficiency);

        return efficiency;
    }

    //HACK
    string EfficiencyText(float efficiency)
    {
        switch (efficiency)
        {
            case 0:
                return "Non è per niente efficace :/";                
            case 0.5f:
                return "Non è molto efficace...";
            case 1:
                return "È efficace.";
            case 2:
                return "È super efficace!";
            default:
                return string.Empty;
        }
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
