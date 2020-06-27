using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFightState : FightManagerState
{
    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "Trovato {EnemyPokemon} selvatico";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show description and spawn player pokemon

        ActiveElements();
        SpawnAnimation();

        SetDescription();
    }

    #region enter

    void ActiveElements()
    {
        //active pokemon
        fightManager.FightUIManager.ActivePokemonImage();
    }

    void SpawnAnimation()
    {
        fightManager.FightUIManager.PokemonSpawnAnimation(true, true);
    }

    void SetDescription()
    {
        //set Description letter by letter, then call OnEndDescription
        fightManager.FightUIManager.SetDescription(description, OnEndDescription);
    }

    #endregion

    void OnEndDescription()
    {
        //end description and, if still running, skip spawn animation
        fightManager.FightUIManager.EndDescription();
        fightManager.FightUIManager.SkipAnimationSpawn();

        //change state
        anim.SetTrigger("PlayerRound");
    }
}
