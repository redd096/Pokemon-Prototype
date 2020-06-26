using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionState : StateMachineBehaviour
{
    [SerializeField] float timeToFadeIn = 1;
    [SerializeField] float delayBetweenFade = 0.5f;
    [SerializeField] float timeToFadeOut = 1;

    [SerializeField] bool startFight = false;

    LevelManager levelManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //set level manager
        if(levelManager == null)
            levelManager = animator.GetComponent<LevelManager>();

        //do fade in, then call EndFadeIn
        GameManager.instance.UiManager.FadeIn_Fill(timeToFadeIn, EndFadeIn);
    }

    void EndFadeIn()
    {
        //change scene behind transition image
        SetManagers();

        //wait, then call fade out
        levelManager.Wait(delayBetweenFade, StartFadeOut);
    }

    void SetManagers()
    {
        //active and deactive based on startFight
        levelManager.FightManager.gameObject.SetActive(startFight);
        levelManager.MovingManager.gameObject.SetActive(!startFight);

        //start setup waiting the fade out
        if (startFight)
            levelManager.FightManager.SetupState();
    }

    void StartFadeOut()
    {
        //do fade out, then call StartGame
        GameManager.instance.UiManager.FadeOut_Fill(timeToFadeOut, StartGame);
    }

    void StartGame()
    {
        //start fight or moving
        if (startFight)
            levelManager.StartFight();
        else
            levelManager.StartMoving();
    }
}
