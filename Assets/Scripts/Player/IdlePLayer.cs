using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePLayer : PlayerState
{
    public IdlePLayer(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Execution()
    {
        base.Execution();

        //do only if not in pause
        if (Time.timeScale == 0)
            return;

        //get inputs
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 newPosition = Vector3.zero;

        //try to move if there is path in direction
        if(horizontal > 0 && CheckPath(Vector3.right, out newPosition))
        {
            Move(newPosition, "Right");
        }
        else if(horizontal < 0 && CheckPath(Vector3.left, out newPosition))
        {
            Move(newPosition, "Left");
        }
        else if(vertical > 0 && CheckPath(Vector3.up, out newPosition))
        {
            Move(newPosition, "Up");
        }
        else if (vertical < 0 && CheckPath(Vector3.down, out newPosition))
        {
            Move(newPosition, "Down");
        }
    }

    bool CheckPath(Vector3 direction, out Vector3 newPosition)
    {
        //out new possible position
        newPosition = stateMachine.transform.position + direction;

        //check if path is free or there is a collision
        return GameManager.instance.LevelManager.MovingManager.CheckPath(newPosition);
    }

    void Move(Vector3 newPosition, string direction)
    {
        //change state to move to new position
        stateMachine.SetState(new MovingPlayer(stateMachine, newPosition, direction));
    }
}
