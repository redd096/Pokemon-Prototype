using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateDescriptionState : FightManagerState
{
    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "{PlayerPokemon} usa {Skill}...";
    [SerializeField] float timeBetweenChar = 0.05f;
    [SerializeField] float skipSpeed = 0.01f;

    Animator anim;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show a description then go to next state

        DeactiveMenu();

        SetDescription();
    }

    protected override void GetReferences(Animator anim)
    {
        base.GetReferences(anim);

        //get animator reference
        this.anim = anim;
    }

    #region enter

    void DeactiveMenu()
    {
        fightManager.FightUIManager.DeactiveMenu();
    }

    void SetDescription()
    {
        //select description args and Set Description letter by letter, then call OnEndDescription
        string[] args = new string[] { };
        string text = ReplaceString(description);
        fightManager.FightUIManager.SetDescription(text, args, timeBetweenChar, skipSpeed, OnEndDescription);
    }

    string ReplaceString(string text)
    {
        string s = text;

        //replace string with data
        Replace(ref s, "{PlayerPokemon}", fightManager.currentPlayerPokemon);
        Replace(ref s, "{EnemyPokemon}", fightManager.currentEnemyPokemon);
        Replace(ref s, "{Skill}", fightManager.skillUsed);
        Replace(ref s, "{Pokemon}", fightManager.pokemonSelected);
        Replace(ref s, "{Item}", fightManager.itemUsed);

        return s;
    }

    void Replace(ref string text, string toReplace, IGetName control)
    {
        //if control != null -> replace string with object name
        if (text.Contains(toReplace) && control != null)
            text = text.Replace(toReplace, control.GetObjectName());
    }

    #endregion

    void OnEndDescription()
    {
        //deactive description
        fightManager.FightUIManager.EndDescription();

        //change state
        anim.SetTrigger("Next");
    }
}
