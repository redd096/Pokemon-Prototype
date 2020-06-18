using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFightState : FightManagerState
{
    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "Trovato {EnemyPokemon} selvatico";

    [Header("Player Pokemon")]
    [SerializeField] float durationAnimation = 1.5f;

    float delta;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show description and spawn player pokemon

        //set to 0
        delta = 0;

        ActiveElements();

        SetDescription();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        //pokemon animation spawn
        PokemonAnimation();
    }

    #region enter

    void ActiveElements()
    {
        //active pokemon
        fightManager.FightUIManager.ActivePokemonImage();
    }

    void SetDescription()
    {
        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(description, OnEndDescription);
    }

    #endregion

    #region update

    void PokemonAnimation()
    {
        if (delta < 1)
        {
            delta += Time.deltaTime / durationAnimation;

            //increase size
            fightManager.FightUIManager.PokemonSpawnAnimation(true, delta);
        }
    }

    #endregion

    void OnEndDescription()
    {
        //be sure to complete animation
        fightManager.FightUIManager.OnEndDescription();

        //change state
        anim.SetTrigger("PlayerRound");
    }
}
