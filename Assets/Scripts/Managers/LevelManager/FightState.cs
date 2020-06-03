using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightState : StateMachineBehaviour
{
    LevelManager levelManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GetReferences(animator);

        SetPlayer();

        SetFightManager();
    }

    void GetReferences(Animator anim)
    {
        //get level manager
        if (levelManager == null)
            levelManager = anim.GetComponent<LevelManager>();
    }

    void SetPlayer()
    {
        //set player
        GameManager.instance.player.StartFightPhase();
    }

    void SetFightManager()
    {
        //set fight manager
        levelManager.FightManager.StartFightPhase();
    }
}
