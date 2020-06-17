using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonState : FightManagerState
{
    [Header("Is Player Turn")]
    [SerializeField] bool isPlayer = true;

    [Header("Animation Despawn")]
    [SerializeField] float durationDespawn = 0.5f;

    [Header("Animation Spawn")]
    [SerializeField] float durationSpawn = 1.5f;

    Animator anim;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //change pokemon and make animation

        //start animation
        fightManager.FightUIManager.StartAnimation(RemovePokemon());
    }

    #region enter

    protected override void GetReferences(Animator anim)
    {
        base.GetReferences(anim);

        //get animator reference
        this.anim = anim;
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
            fightManager.SetCurrentPlayerPokemon(fightManager.pokemonSelected);
        else
            fightManager.SetCurrentEnemyPokemon(fightManager.pokemonSelected);

        //set UI new pokemon
        fightManager.FightUIManager.SetPokemonInArena(isPlayer);

        //if is player turn, set new skill
        if(isPlayer)
            fightManager.FightUIManager.SetSkillsList(fightManager.pokemonSelected);

        //start animation
        fightManager.FightUIManager.StartAnimation(SpawnNextPokemon());
    }

    void EndTurn()
    {
        //change state
        anim.SetTrigger("Next");
    }
}
