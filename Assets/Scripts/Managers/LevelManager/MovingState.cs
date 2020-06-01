using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : StateMachineBehaviour
{
    LevelManager levelManager;
    GameManager gm;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GetReferences(animator);

        SetManagers();

        SetPlayer();
    }

    void GetReferences(Animator anim)
    {
        //get level manager reference
        if (levelManager == null)
            levelManager = anim.GetComponent<LevelManager>();

        //get game manager
        if (gm == null)
            gm = GameManager.instance;
    }

    void SetManagers()
    {
        //deactive fight manager and active moving manager
        gm.fightManager.gameObject.SetActive(false);
        gm.movingManager.gameObject.SetActive(true);
    }

    void SetPlayer()
    {
        //set player
        gm.player.StartMovingPhase();
    }
}
