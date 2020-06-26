using System.Collections;

public class PlayerState : State
{
    protected Player player;

    public PlayerState(StateMachine stateMachine) : base(stateMachine)
    {
        player = stateMachine as Player;
    }

    public override void Execution()
    {
        base.Execution();

        player.CheckPause();
    }
}
