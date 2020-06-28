using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeoneTurnState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

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

        //if player turn
        if(isPlayer)
        {
            #region pad
            //Start for pause and resume
            if (Input.GetKeyDown(KeyCode.JoystickButton7))
            {
                GameManager.instance.PauseResumeGame();
                return;
            }

            //B button
            if(Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                //if paused, Resume
                if(Time.timeScale == 0)
                {
                    GameManager.instance.PauseResumeGame();
                }
                //else back to player menu only if pokemon is alive (because pokemon menu is shown also when the player has to replace his dead pokemon)
                else if (fightManager.currentPlayerPokemon.CurrentHealth > 0)
                {
                    fightManager.FightUIManager.BackToPlayerMenu();
                }

                return;
            }
            #endregion

            #region keyboard and phone   
            //esc on keyboard or back button on smartphone
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //if paused and press back, resume
                if (Time.timeScale == 0)
                {
                    GameManager.instance.PauseResumeGame();
                }
                //else back button
                else
                {
                    fightManager.BackButton();
                }
            }
            #endregion
        }
    }

    #region enter

    void CheckEffects()
    {
        //start coroutine
        fightManager.FightUIManager.StartCoroutineState(CheckAllEffects());
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
        fightManager.FightUIManager.EndCoroutineState();

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
            fightManager.FightUIManager.UpdateHealth(isPlayer, previousHealth, NextEffect);
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
        fightManager.FightUIManager.BackToPlayerMenu();
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
