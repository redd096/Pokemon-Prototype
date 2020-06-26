using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutState : StateMachineBehaviour
{
    [SerializeField] float timeToFadeOut = 1;

    LevelManager levelManager;
    float delta;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //set
        levelManager = animator.GetComponent<LevelManager>();
        delta = 0;
        SetManagers();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        //do fade out and start game
        if(delta < 1)
        {
            GameManager.instance.UiManager.FadeOut(ref delta, timeToFadeOut);
        }
        else
        {
            StartGame();
        }
    }

    void SetManagers()
    {
        //deactive fight manager
        levelManager.FightManager.gameObject.SetActive(false);

        //and active moving manager
        levelManager.MovingManager.gameObject.SetActive(true);
    }

    void StartGame()
    {
        //reset alpha but remove fill amount, so player can't see image
        GameManager.instance.UiManager.ResetTransitionImage();

        //start game
        levelManager.StartMoving();
    }
}
