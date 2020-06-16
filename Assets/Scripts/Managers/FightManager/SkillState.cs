using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    [Header("Skill Animation (half attack, half come back)")]
    [SerializeField] float durationAnimation = 1.0f;

    [Header("Update Health")]
    [SerializeField] float durationUpdateHealth = 0.7f;

    float startHealth;
    Animator anim;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //set start health (of the pokemon getting damage)
        startHealth = isPlayer ? fightManager.currentEnemyPokemon.CurrentHealth : fightManager.currentPlayerPokemon.CurrentHealth;

        //apply damage
        ApplyDamage();

        //start animation
        fightManager.FightUIManager.StartAnimation(AttackAnimation());

        //TODO
        //description super efficace?
        //apply possible effects
        //description effect attivato?
    }

    #region enter

    protected override void GetReferences(Animator anim)
    {
        base.GetReferences(anim);

        //get animator reference
        this.anim = anim;
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
        float efficiencyMultiplier = skill.EfficiencyMultiplier(otherPokemon.pokemonData.PokemonType);
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
        delta = 0;
        fightManager.FightUIManager.AttackAnimation(isPlayer, delta);

        //update health
        while (delta < 1)
        {
            delta += Time.deltaTime / durationUpdateHealth;
            fightManager.FightUIManager.UpdateHealth(isPlayer, startHealth, delta);
            yield return null;
        }

        //be sure to end animation
        fightManager.FightUIManager.UpdateHealth(isPlayer, startHealth, 1);
        fightManager.FightUIManager.EndAnimation();

        //call end
        EndTurn();
    }

    #endregion

    void EndTurn()
    {
        //change state
        anim.SetTrigger("Next");
    }
}
