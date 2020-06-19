using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "{PlayerPokemon} usa {Skill}...";

    [Header("Skill Animation (half attack, half come back)")]
    [SerializeField] float durationAnimation = 1.0f;

    [Header("Update Health")]
    [SerializeField] float durationUpdateHealth = 0.7f;

    PokemonModel otherPokemon;

    float previousHealth;
    string efficiencyText;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show attack description
        //do attack (damage and effect) and show animation - show also description of efficiency
        //check if kill the other pokemon, else go to next state

        //set previous health (inverse, cause is the other pokemon getting damage)
        previousHealth = isPlayer ? fightManager.currentEnemyPokemon.CurrentHealth : fightManager.currentPlayerPokemon.CurrentHealth;

        SetDescription();

        ApplyDamage();
    }

    #region enter

    void SetDescription()
    {
        //deactive menu, to be sure to read description
        fightManager.FightUIManager.DeactiveMenu();

        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(description, OnEndDescription);
    }

    void OnEndDescription()
    {
        //deactive description
        fightManager.FightUIManager.EndDescription();

        //start animation
        fightManager.FightUIManager.StartAnimation(AttackAnimation());
    }

    void ApplyDamage()
    {
        //get this and other pokemon, based if is player or enemy turn
        PokemonModel thisPokemon = isPlayer ? fightManager.currentPlayerPokemon : fightManager.currentEnemyPokemon;
        otherPokemon = isPlayer ? fightManager.currentEnemyPokemon : fightManager.currentPlayerPokemon;

        //apply damage and effect
        otherPokemon.GetDamage(fightManager.SkillUsed, thisPokemon, out efficiencyText);
    }

    #endregion

    #region animation

    IEnumerator AttackAnimation()
    {
        float delta = 0;

        //attack
        while(delta < 1)
        {
            delta += Time.deltaTime / (durationAnimation / 2);
            fightManager.FightUIManager.AttackAnimation(isPlayer, delta);
            yield return null;
        }

        //come back
        while(delta > 0)
        {
            delta -= Time.deltaTime / (durationAnimation / 2);
            fightManager.FightUIManager.AttackAnimation(isPlayer, delta);
            yield return null;
        }

        //be sure to end animation
        fightManager.FightUIManager.AttackAnimation(isPlayer, 0);
        fightManager.FightUIManager.EndAnimation();

        EndAttackAnimation();
    }

    #endregion

    void EndAttackAnimation()
    {
        //update health and set description efficiency
        fightManager.FightUIManager.UpdateHealth(!isPlayer, previousHealth, durationUpdateHealth);  // !isPlayer, because update the health of the other pokemon (who's been attacked)
        SetDescription_Efficiency();
    }

    void SetDescription_Efficiency()
    {
        //set Description letter by letter, then call OnEndDescription_Efficiency
        fightManager.FightUIManager.SetDescription(efficiencyText, EndTurn);
    }

    void EndTurn()
    {
        //remove description
        fightManager.FightUIManager.EndDescription();

        if (otherPokemon.CurrentHealth > 0)
        {
            //go to next state
            anim.SetTrigger("Next");
        }
        else
        {
            //change state to pokemon dead
            anim.SetTrigger("PokemonDead");
        }
    }
}
