using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeoneTurnState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    [Header("Update Health")]
    [SerializeField] float durationUpdateHealth = 0.7f;

    PokemonModel pokemon;
    bool waitDescription;
    float previousHealth;
    bool removingOldEffects;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //check if there are effects - apply and show description
        //check if dead
        //if alive, show player menu or do enemy attack

        pokemon = isPlayer ? fightManager.currentPlayerPokemon : fightManager.currentEnemyPokemon;

        //start removing old effects, then apply new ones
        removingOldEffects = true;
        CheckEffects();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        //if player turn and press back - back to player menu
        if(isPlayer && Input.GetKeyDown(KeyCode.Escape))
        {
            fightManager.BackToPlayerMenu();
        }
    }

    #region enter

    void CheckEffects()
    {
        //start coroutine
        fightManager.FightUIManager.StartAnimation(CheckAllEffects());
    }

    IEnumerator CheckAllEffects()
    {
        //create a copy, because the list can be modified (when apply, can remove effect from the list)
        List<EffectModel> effects = Utility.CreateCopy(removingOldEffects ? pokemon.RemovedEffects : pokemon.ActiveEffects);

        //foreach effect removed or applied on this pokemon
        foreach (EffectModel effect in effects)
        {
            previousHealth = pokemon.CurrentHealth;

            //remove or apply effect
            string effectDescription;
            if (removingOldEffects)
                effect.RemoveEffect(pokemon, isPlayer, out effectDescription);
            else
                effect.ApplyEffect(pokemon, isPlayer, out effectDescription);

            //show description
            waitDescription = true;
            fightManager.FightUIManager.SetDescription(effectDescription, CheckLife);

            //wait to finish writing description
            while(waitDescription)
            {
                yield return null;
            }
        }
        
        //end animation
        fightManager.FightUIManager.EndAnimation();

        //if removing old effects, now check every effects applied
        if (removingOldEffects)
        {
            removingOldEffects = false;
            CheckEffects();
        }
        //else, check is alive
        else
        {
            CheckIsAlive();
        }
    }

    void CheckLife()
    {
        //if changed health, update UI before go to next effect
        if (pokemon.CurrentHealth != previousHealth)
        {
            fightManager.FightUIManager.UpdateHealth(isPlayer, previousHealth, durationUpdateHealth, NextEffect);
            return;
        }

        //else go immediatly to next effect
        NextEffect();
    }

    void NextEffect()
    {
        //end description
        waitDescription = false;
    }

    #endregion

    void CheckIsAlive()
    {
        fightManager.FightUIManager.EndDescription();

        //check is alive
        if (pokemon.CurrentHealth > 0)
        {
            //if player, show player menu - else, enemy attack
            if (isPlayer)
                ActivePlayerMenu();
            else
                Fight();
        }
        //else dead
        else
        {
            anim.SetTrigger("PokemonDead");
        }
    }

    void ActivePlayerMenu()
    {
        fightManager.FightUIManager.ActivePlayerMenu();
    }

    void Fight()
    {
        List<SkillModel> skillsUsable = new List<SkillModel>();

        //create a list of skill with PP > 0
        foreach (SkillModel skill in fightManager.currentEnemyPokemon.CurrentSkills)
        {
            if (skill != null && skill.CurrentPP > 0)
            {
                skillsUsable.Add(skill);
            }
        }

        //if every skill has PP 0 then use baseSkill
        if (skillsUsable.Count < 1)
        {
            fightManager.UseSkill(new SkillModel(fightManager.baseSkill));
        }
        //else select a random skill
        else
        {
            SkillModel skillToUse = skillsUsable[Random.Range(0, skillsUsable.Count)];

            fightManager.UseSkill(skillToUse);
        }
    }
}
