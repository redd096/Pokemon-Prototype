using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
    [SerializeField] float durationMovement = 1;

    public Animator Anim { get; private set; }
    public float DurationMovement { get { return durationMovement; } }

    void Start()
    {
        //get animator
        Anim = GetComponentInChildren<Animator>();

        //start state machine for moving
        SetState(new IdlePLayer(this));
    }

    void Update()
    {
        state.Execution();
    }
}
