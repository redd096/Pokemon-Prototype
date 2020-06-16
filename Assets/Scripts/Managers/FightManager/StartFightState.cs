using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFightState : FightManagerState
{
    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "Trovato {0} selvatico";
    [SerializeField] float timeBetweenChar = 0.05f;
    [SerializeField] float skipSpeed = 0.01f;

    [Header("Player Pokemon")]
    [SerializeField] float durationAnimation = 1.5f;

    float delta;

    Animator anim;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

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

    protected override void GetReferences(Animator anim)
    {
        base.GetReferences(anim);

        //get animator reference
        this.anim = anim;
    }

    #region enter

    void ActiveElements()
    {
        //active pokemon
        fightManager.FightUIManager.ActivePokemonImage();
    }

    void SetDescription()
    {
        //select description args and Set Description letter by letter, then call OnEndDescription
        string[] args = new string[] { fightManager.enemyPokemons[0].pokemonData.PokemonName };
        fightManager.FightUIManager.SetDescription(description, args, timeBetweenChar, skipSpeed, OnEndDescription);
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
