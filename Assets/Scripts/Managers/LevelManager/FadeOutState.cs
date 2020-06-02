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
            levelManager.TransitionImage.FadeOut(ref delta, timeToFadeOut);
        }
        else
        {
            StartGame();
        }
    }

    void SetManagers()
    {
        //deactive fight manager and active moving manager
        levelManager.FightManager.gameObject.SetActive(false);
        levelManager.MovingManager.gameObject.SetActive(true);
    }

    void StartGame()
    {
        //reset alpha but remove fill amount, so player can't see image
        levelManager.TransitionImage.fillAmount = 0;
        Color imageColor = levelManager.TransitionImage.color;
        levelManager.TransitionImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1);

        //start game
        levelManager.StartMoving();
    }
}
