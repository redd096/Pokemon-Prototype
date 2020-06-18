using UnityEngine;

public class FightManagerState : StateMachineBehaviour
{
    protected FightManager fightManager;
    protected Animator anim;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GetReferences(animator);
    }

    protected virtual void GetReferences(Animator anim)
    {
        if (fightManager == null)
            fightManager = anim.GetComponent<FightManager>();

        if (this.anim == null)
            this.anim = anim;
    }
}
