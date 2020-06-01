using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void StartFight()
    {
        anim.SetTrigger("Fight");
    }

    public void StartMoving()
    {
        anim.SetTrigger("Moving");
    }
}
