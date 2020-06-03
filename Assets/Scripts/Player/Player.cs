using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
    [Header("Moving Phase")]
    [SerializeField] float durationMovement = 1;

    public Animator Anim { get; private set; }
    public float DurationMovement { get { return durationMovement; } }

    void Start()
    {
        //get animator
        Anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        state?.Execution();
    }

    #region public API

    public void StartMovingPhase()
    {
        //start state machine for moving
        SetState(new IdlePLayer(this));
    }

    public void StartFightPhase()
    {
        //start state machine for fight
        SetState(null);
    }

    #endregion
}
