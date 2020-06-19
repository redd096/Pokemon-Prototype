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

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //check if there are effects - apply and show description
        //check if dead
        //if alive, show player menu or do enemy attack

        pokemon = isPlayer ? fightManager.currentPlayerPokemon : fightManager.currentEnemyPokemon;

        ApplyEffects();
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

    void ApplyEffects()
    {
        //start coroutine
        fightManager.FightUIManager.StartAnimation(ApplyAllEffects());
    }

    IEnumerator ApplyAllEffects()
    {
        //create a copy, because the list can be modified (when apply, can remove effect from the list)
        List<EffectModel> activeEffects = CopyList(pokemon.ActiveEffects);

        //foreach effect applied on this pokemon
        foreach (EffectModel effect in activeEffects)
        {
            previousHealth = pokemon.CurrentHealth;

            //apply effect
            string effectDescription;
            effect.ApplyEffect(pokemon, isPlayer, out effectDescription);

            //show description
            fightManager.FightUIManager.SetDescription(effectDescription, CheckLife);
            waitDescription = true;

            //wait to finish writing description
            while(waitDescription)
            {
                yield return null;
            }
        }
        
        //end animation and check if alive
        fightManager.FightUIManager.EndAnimation();

        CheckIsAlive();
    }

    List<T> CopyList<T>(List<T> list)
    {
        List<T> newList = new List<T>();

        //add every element in new list
        foreach(T element in list)
        {
            newList.Add(element);
        }

        return newList;
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
            if (skill.CurrentPP > 0)
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
