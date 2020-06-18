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

    float startHealth;
    string efficiencyText;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show attack description
        //do attack and show animation - show also description of efficiency
        //check if kill the other pokemon, else go to next state

        //set start health (inverse, cause is the other pokemon getting damage)
        startHealth = isPlayer ? fightManager.currentEnemyPokemon.CurrentHealth : fightManager.currentPlayerPokemon.CurrentHealth;

        SetDescription();

        ApplyDamage();

        //TODO
        //apply possible effects
        //description effect attivato?
        //PENSO CI VOGLIA UN'ALTRA STATE ANCORA PER ATTACCARE L'EFFECT E MOSTRARE UNA DESCRIZIONE
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
        SkillModel skill = fightManager.skillUsed;

        //get this pokemon and the other pokemon, based if is player or enemy turn
        PokemonModel thisPokemon = isPlayer ? fightManager.currentPlayerPokemon : fightManager.currentEnemyPokemon;
        PokemonModel otherPokemon = isPlayer ? fightManager.currentEnemyPokemon : fightManager.currentPlayerPokemon;

        //get attack and defense, based to the skill if special or physics
        float attack = fightManager.skillUsed.skillData.IsSpecial ? thisPokemon.SpecialAttack : thisPokemon.PhysicsAttack;
        float defense = fightManager.skillUsed.skillData.IsSpecial ? otherPokemon.SpecialDefense : otherPokemon.PhysicsDefense;

        //get multipliers
        float efficiencyMultiplier = skill.EfficiencyMultiplier(otherPokemon.pokemonData.PokemonType, out efficiencyText);
        float stab = skill.STAB(thisPokemon.pokemonData.PokemonType);
        float nRandom = skill.NRandom();

        //((( (2 * Livello Pokemon + 10) * Attacco Pokemon * Potenza Mossa ) / (250 * Difesa Fisica o Difesa Speciale del Nemico)) +2 ) * Efficacia * STAB * N
        float damage = ((((2 * thisPokemon.CurrentLevel + 10) * attack * skill.skillData.Power) / (250 * defense)) + 2) * efficiencyMultiplier * stab * nRandom;

        //apply damage
        otherPokemon.GetDamage(damage);
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

    IEnumerator UpdateHealth()
    {
        float delta = 0;

        //update health
        while (delta < 1)
        {
            delta += Time.deltaTime / durationUpdateHealth;
            fightManager.FightUIManager.UpdateHealth(!isPlayer, startHealth, delta);    // !isPlayer, because update the health of the other pokemon (who's been attacked)
            yield return null;
        }

        //be sure to end animation
        fightManager.FightUIManager.UpdateHealth(!isPlayer, startHealth, 1);            // !isPlayer, because update the health of the other pokemon (who's been attacked)
        fightManager.FightUIManager.EndAnimation();
    }

    #endregion

    void EndAttackAnimation()
    {
        //update health and set description efficiency
        fightManager.FightUIManager.StartAnimation(UpdateHealth());
        SetDescription_Efficiency();
    }

    void SetDescription_Efficiency()
    {
        //set Description letter by letter, then call EndTurn
        fightManager.FightUIManager.SetDescription(efficiencyText, EndTurn);
    }

    void EndTurn()
    {
        //remove description
        fightManager.FightUIManager.EndDescription();

        PokemonModel otherPokemon = isPlayer ? fightManager.currentEnemyPokemon : fightManager.currentPlayerPokemon;

        if (otherPokemon.CurrentHealth <= 0)
        {
            //change state to pokemon dead
            anim.SetTrigger("PokemonDead");
        }
        else
        {
            //go to next state
            anim.SetTrigger("Next");
        }
    }
}
