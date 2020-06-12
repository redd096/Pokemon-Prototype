using UnityEngine;

[System.Serializable]
public class SkillModel : IGetButtonName
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

    public void RestorePP()
    {
        CurrentPP = skillData.PP;
    }
}
