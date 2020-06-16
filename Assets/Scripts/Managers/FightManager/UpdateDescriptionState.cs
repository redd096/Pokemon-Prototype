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
        s = s.Replace("{PlayerPokemon}", fightManager.currentPlayerPokemon.pokemonData.PokemonName);
        s = s.Replace("{EnemyPokemon}", fightManager.currentEnemyPokemon.pokemonData.PokemonName);
        s = s.Replace("{Skill}", fightManager.skillUsed?.skillData.SkillName);
        s = s.Replace("{Pokemon}", fightManager.pokemonSelected?.pokemonData.PokemonName);
        s = s.Replace("{Item}", fightManager.itemUsed?.ItemName);

        return s;
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
