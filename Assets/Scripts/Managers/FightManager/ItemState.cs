using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "Usi {Item} su {PlayerPokemon}...";

    PokemonModel pokemon;
    float previousHealth;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show description using item
        //add item effect

        SetDescription();
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

        //add effect to pokemon list
        AddEffect();
    }

    #endregion

    void AddEffect()
    {
        //add effect to pokemon list
        pokemon = isPlayer ? fightManager.currentPlayerPokemon : fightManager.currentEnemyPokemon;
        EffectModel effect = pokemon.AddEffect(fightManager.ItemUsed.itemData.Effect);

        previousHealth = pokemon.CurrentHealth;

        //apply effect
        string effectDescription;
        effect.ApplyEffect(pokemon, isPlayer, out effectDescription);

        //show effect description
        fightManager.FightUIManager.SetDescription(effectDescription, CheckLife);
    }

    void CheckLife()
    {
        //if changed health, update UI before end turn
        if (pokemon.CurrentHealth != previousHealth)
        {
            fightManager.FightUIManager.UpdateHealth(isPlayer, previousHealth, EndTurn);
            return;
        }

        //else end turn immediatly
        EndTurn();
    }

    void EndTurn()
    {
        //remove description
        fightManager.FightUIManager.EndDescription();

        if (pokemon.CurrentHealth > 0)
        {
            //go to next state
            anim.SetTrigger("Next");
        }
        else
        {
            //change state to pokemon dead
            anim.SetTrigger("PokemonDead");
        }
    }
}
