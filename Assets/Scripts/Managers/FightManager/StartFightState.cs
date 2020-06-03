using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFightState : FightManagerState
{
    [Header("Description")]
    [SerializeField] string description = "Trovato pokemon selvatico";
    [SerializeField] float timeBetweenChar = 0.05f;
    [SerializeField] float skip = 0.01f;

    [Header("Player Pokemon")]
    [SerializeField] float durationAnimation = 1.5f;

    float delta;

    Animator anim;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        ResetElements();

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

    void ResetElements()
    {
        //set to 0
        delta = 0;

        fightManager.playerPokemon.localScale = Vector3.zero;
        fightManager.description.text = string.Empty;
    }

    void ActiveElements()
    {
        //active pokemon
        fightManager.playerPokemon.gameObject.SetActive(true);

        //active description
        fightManager.description.gameObject.SetActive(true);
    }

    void SetDescription()
    {
        //write description letter by letter. Then press a button and call OnEndDescription
        UtilityMonoBehaviour.instance.WriteLetterByLetter(fightManager.description, description, timeBetweenChar, skip, OnEndDescription);
    }

    #endregion

    #region update

    void PokemonAnimation()
    {
        if (delta < 1)
        {
            delta += Time.deltaTime / durationAnimation;

            //increase size
            fightManager.playerPokemon.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, delta);
        }
    }

    #endregion

    void OnEndDescription()
    {
        //be sure to comlete animation
        fightManager.playerPokemon.localScale = Vector3.one;

        //change state
        anim.SetTrigger("PlayerRound");
    }
}
