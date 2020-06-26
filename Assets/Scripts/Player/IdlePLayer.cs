using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePLayer : PlayerState
{
    public IdlePLayer(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override IEnumerator Enter()
    {
        yield return base.Enter();

        player.movePlayer += MovePlayer;
    }

    public override IEnumerator Exit()
    {
        yield return base.Exit();

        player.movePlayer -= MovePlayer;
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

        //try to move if there is path in direction
        if(horizontal > 0)
        {
            MovePlayer("Right");
        }
        else if(horizontal < 0)
        {
            MovePlayer("Left");
        }
        else if(vertical > 0)
        {
            MovePlayer("Up");
        }
        else if (vertical < 0)
        {
            MovePlayer("Down");
        }
    }

    void MovePlayer(string direction)
    {
        Vector3 newPosition = Vector3.zero;

        switch (direction)
        {
            case "Right":
                if (CheckPath(Vector3.right, out newPosition))
                    Move(newPosition, direction);
                break;
            case "Left":
                if (CheckPath(Vector3.left, out newPosition))
                    Move(newPosition, direction);
                break;
            case "Up":
                if (CheckPath(Vector3.up, out newPosition))
                    Move(newPosition, direction);
                break;
            case "Down":
                if (CheckPath(Vector3.down, out newPosition))
                    Move(newPosition, direction);
                break;
            default:
                break;
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
