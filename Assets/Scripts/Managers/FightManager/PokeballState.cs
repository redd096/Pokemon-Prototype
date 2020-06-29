using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokeballState : FightManagerState
{
    [Header("Description")]
    [TextArea()] [SerializeField] string description = "Provi a catturare {EnemyPokemon} con {Item}...";
    [TextArea()] [SerializeField] string caughtDescription = "Sei riuscito a catturare {EnemyPokemon}.";
    [TextArea()] [SerializeField] string failedDescription = "{EnemyPokemon} si è liberato!";

    [Header("Try Catch")]
    [SerializeField] float speedCatchDescription = 0.2f;
    [SerializeField] string catchDescrption = "....";
    [SerializeField] float timeToClick = 5;
    int timeClicked;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //show description, then animation and try to catch enemy pokemon
        //if succeded go to state Caught
        //if failed or is a trainer's pokemon, show description you failed and go to enemy turn

        timeClicked = 0;
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
        //start animation enemy pokemon despawn
        fightManager.FightUIManager.PokemonSpawnAnimation(false, false, Wait);
    }

    #endregion

    #region wait

    void Wait()
    {
        //wait
        fightManager.FightUIManager.StartCoroutineState(WaitCoroutine());
    }

    IEnumerator WaitCoroutine()
    {
        //click n times
        while(timeClicked < timeToClick)
        {
            int click = timeClicked;

            //write description, then click
            fightManager.FightUIManager.SetDescription(catchDescrption, ResetDescription, speedCatchDescription);

            //wait click before continue loop
            while(click == timeClicked)
                yield return null;
        }

        //try to catch
        TryCatch();
    }

    void ResetDescription()
    {
        //set click and reset description
        timeClicked++;
        fightManager.FightUIManager.EndDescription();
    }

    #endregion

    void TryCatch()
    {
        //end coroutine
        fightManager.FightUIManager.EndCoroutineState();

        //try catch
        if (fightManager.ItemUsed.TryCatch(fightManager.currentEnemyPokemon, true))
        {
            //show caught description
            fightManager.FightUIManager.SetDescription(caughtDescription, Caught);
        }
        else
        {
            //show failed description
            fightManager.FightUIManager.SetDescription(failedDescription, Failed);
        }
    }

    void Caught()
    {
        fightManager.FightUIManager.EndDescription();

        anim.SetTrigger("Caught");
    }

    void Failed()
    {
        fightManager.FightUIManager.EndDescription();

        //start animation enemy pokemon respawn
        fightManager.FightUIManager.PokemonSpawnAnimation(true, false, ChangeState);
    }

    void ChangeState()
    {
        anim.SetTrigger("Next");
    }
}
