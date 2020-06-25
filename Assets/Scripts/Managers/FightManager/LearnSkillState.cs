using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnSkillState : FightManagerState
{
    [Header("Description")]
    [TextArea()] [SerializeField] string description = "Scegli uno slot per {SkillToLearn}";
    [TextArea()] [SerializeField] string confirmDescription = "{PlayerPokemon} apprende {SkillToLearn}.";
    [TextArea()] [SerializeField] string forgetSkillDescription = "{PlayerPokemon} dimentica {0}...";
    [TextArea()] [SerializeField] string refuseDescription = "{PlayerPokemon} non apprende {SkillToLearn}...";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show description and set list of skill to replace
        //then go to next state

        SetDescription();
        fightManager.FightUIManager.SetLearnSkillsMenu(LearnSkill, RefuseSkill);
    }

    #region enter

    void SetDescription()
    {
        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(description, OnEndDescription);
    }

    void OnEndDescription()
    {
        //deactive description
        fightManager.FightUIManager.EndDescription();

        //show skills to replace
        fightManager.FightUIManager.ShowLearnSkillsMenu();
    }

    #endregion

    string Replace(string text, string value)
    {
        if (text.Contains("{0}"))
            return text.Replace("{0}", value);

        return text;
    }

    void LearnSkill(int index)
    {
        //get skill to forget
        SkillModel skillToForget = index < fightManager.currentPlayerPokemon.CurrentSkills.Length ? fightManager.currentPlayerPokemon.CurrentSkills[index] : null;

        //learn skill
        fightManager.currentPlayerPokemon.LearnSkill(fightManager.SkillToLearn, index);

        //if need to forget a skill, show forgetSkillDescription before confirm
        if (skillToForget != null)
        {
            string s = Replace(forgetSkillDescription, skillToForget.GetObjectName());

            fightManager.FightUIManager.SetDescription(forgetSkillDescription, ConfirmSkillDescription);
        }
        //else show immediatly the confirm
        else
        {
            ConfirmSkillDescription();
        }
    }

    void RefuseSkill()
    {
        //refuse skill
        fightManager.currentPlayerPokemon.RefuseSkill(fightManager.SkillToLearn.skillData);

        //description, then come back to previous state
        fightManager.FightUIManager.SetDescription(refuseDescription, ChangeState);
    }

    void ConfirmSkillDescription()
    {
        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(confirmDescription, ChangeState);
    }

    void ChangeState()
    {
        //deactive description
        fightManager.FightUIManager.EndDescription();

        //remove menu
        fightManager.FightUIManager.HideLearnSkillsMenu();

        //change state
        anim.SetTrigger("Next");
    }
}
