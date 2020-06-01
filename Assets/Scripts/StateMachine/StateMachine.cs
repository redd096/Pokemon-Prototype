using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected State state;

    public void SetState(State stateToSet)
    {
        //exit from previous
        if (state != null)
            StartCoroutine(state.Exit());

        //set new one
        state = stateToSet;

        //enter in new one
        if(state != null)
            StartCoroutine(state.Enter());
    }
}
