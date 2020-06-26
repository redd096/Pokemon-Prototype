using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    [Header("Is Evolving")]
    [SerializeField] bool isEvolving = false;

    [Header("Description")]
    [TextArea()]
    [SerializeField] string description = "Torna {PlayerPokemon}.\nVai {Pokemon}! Scelgo te!";

    [Header("Animation Despawn")]
    [SerializeField] float durationDespawn = 0.5f;

    [Header("Animation Spawn")]
    [SerializeField] float durationSpawn = 1.5f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show description changing pokemon
        //change pokemon and make animation

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

        //start animation
        fightManager.FightUIManager.StartAnimation(RemovePokemon());
    }

    #endregion

    #region animations

    IEnumerator RemovePokemon()
    {
        float delta = 1;

        //reduce animation
        while(delta > 0)
        {
            delta -= Time.deltaTime / durationDespawn;
            fightManager.FightUIManager.PokemonSpawnAnimation(isPlayer, delta);
            yield return null;
        }

        //end animation and change pokemon
        fightManager.FightUIManager.EndAnimation();
        NextPokemon();
    }

    IEnumerator SpawnNextPokemon()
    {
        float delta = 0;

        //increase animation
        while (delta < 1)
        {
            delta += Time.deltaTime / durationSpawn;
            fightManager.FightUIManager.PokemonSpawnAnimation(isPlayer, delta);
            yield return null;
        }

        //be sure to end animation
        fightManager.FightUIManager.PokemonSpawnAnimation(isPlayer, 1);

        //end animation and change pokemon
        fightManager.FightUIManager.EndAnimation();
        EndTurn();
    }

    #endregion

    void NextPokemon()
    {
        //set new pokemon
        if (isPlayer)
            fightManager.SetCurrentPlayerPokemon(fightManager.PokemonSelected, isEvolving);
        else
            fightManager.SetCurrentEnemyPokemon(fightManager.PokemonSelected);

        //set UI new pokemon
        fightManager.FightUIManager.SetPokemonInArena(isPlayer);

        //if is player turn, set new skill
        if(isPlayer)
            fightManager.FightUIManager.SetSkillsList(fightManager.currentPlayerPokemon);

        //start animation
        fightManager.FightUIManager.StartAnimation(SpawnNextPokemon());
    }

    void EndTurn()
    {
        //change state
        anim.SetTrigger("Next");
    }
}
