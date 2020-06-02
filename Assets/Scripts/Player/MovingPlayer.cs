using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlayer : State
{
    Vector2 targetPosition;
    string direction;

    public MovingPlayer(StateMachine stateMachine, Vector2 targetPosition, string direction) : base(stateMachine)
    {
        this.targetPosition = targetPosition;
        this.direction = direction;
    }

    public override IEnumerator Enter()
    {
        //get references from player
        Player player = stateMachine as Player;
        Transform transform = player.transform;
        Animator anim = player.Anim;
        float durationMovement = player.DurationMovement;

        //start variables
        float delta = 0;
        Vector2 startPosition = transform.position;
        anim.SetTrigger(direction);

        //from 0 to 1, from startPosition to targetPosition
        while(delta < 1)
        {
            delta += Time.deltaTime / durationMovement;

            transform.position = Vector2.Lerp(startPosition, targetPosition, delta);

            yield return null;
        }

        //set final
        transform.position = targetPosition;
        anim.SetTrigger("Idle");

        //check destination
        CheckDestination();
    }

    void CheckDestination()
    {
        //check if there is a pokemon, otherwise come back to idle
        if(GameManager.instance.levelManager.MovingManager.CheckPokemon(stateMachine.transform.position) == false)
            stateMachine.SetState(new IdlePLayer(stateMachine));
    }
}
