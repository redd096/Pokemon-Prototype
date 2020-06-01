using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightPlayer : State
{
    public FightPlayer(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Execution()
    {
        base.Execution();

        if (Input.GetKeyDown(KeyCode.L))
        {
            stateMachine.SetState(null);
            GameManager.instance.levelManager.StartMoving();
        }
    }
}
