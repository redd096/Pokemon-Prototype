using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] MovingManager movingManager = default;
    [SerializeField] FightManager fightManager = default;

    public MovingManager MovingManager { get { return movingManager; } }
    public FightManager FightManager { get { return fightManager; } }

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    #region for states

    public void Wait(float duration, System.Action onEnd)
    {
        StartCoroutine(Wait_Coroutine(duration, onEnd));
    }

    IEnumerator Wait_Coroutine(float duration, System.Action onEnd)
    {
        yield return new WaitForSeconds(duration);

        onEnd?.Invoke();
    }

    #endregion

    public void StartFight()
    {
        anim.SetTrigger("Fight");
    }

    public void StartMoving()
    {
        anim.SetTrigger("Moving");
    }
}
