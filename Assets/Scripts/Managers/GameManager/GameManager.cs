using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player { get; private set; }
    public MovingManager movingManager { get; private set; }
    public FightManager fightManager { get; private set; }

    protected override void SetDefaults()
    {
        player = FindObjectOfType<Player>();
        movingManager = FindObjectOfType<MovingManager>();
        fightManager = FindObjectOfType<FightManager>();

        fightManager?.gameObject.SetActive(false);
    }

    #region events

    public System.Action onStartMovingPhase;
    public System.Action onEndMovingPhase;
    public System.Action onStartFightPhase;
    public System.Action onEndFightPhase;

    bool canCallEvent = true;

    public void StartMovingPhase()
    {
        if(canCallEvent)
        {
            canCallEvent = false;

            onEndFightPhase?.Invoke();

            Invoke("OnStartMovingPhase", 1);
        }
    }

    void OnStartMovingPhase()
    {
        onStartMovingPhase?.Invoke();

        canCallEvent = true;
    }

    public void StartFightPhase()
    {
        if (canCallEvent)
        {
            canCallEvent = false;

            onEndMovingPhase?.Invoke();

            Invoke("OnStartFightPhase", 1);
        }
    }

    void OnStartFightPhase()
    {
        onStartFightPhase?.Invoke();

        canCallEvent = true;
    }

    #endregion
}