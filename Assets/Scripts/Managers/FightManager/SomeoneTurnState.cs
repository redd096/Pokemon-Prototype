using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeoneTurnState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //check if there are effects
        //check if dead, then show player menu or do enemy attack

        //TODO se ha qualche effetto attivo, deve mostrarlo

        CheckIsAlive(animator);
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

    void CheckIsAlive(Animator anim)
    {
        PokemonModel pokemon = isPlayer ? fightManager.currentPlayerPokemon : fightManager.currentEnemyPokemon;

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
