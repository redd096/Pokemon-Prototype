using UnityEngine;

[System.Serializable]
public class SkillModel
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

    public void RestorePP()
    {
        CurrentPP = skillData.PP;
    }
}
