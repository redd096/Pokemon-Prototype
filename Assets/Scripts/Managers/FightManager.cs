using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            EndFight();
        }
    }
    
    void EndFight()
    {
        //end fight
        GameManager.instance.movingManager.gameObject.SetActive(true);
        gameObject.SetActive(false);

        GameManager.instance.player.SetState(new IdlePLayer(GameManager.instance.player));
    }
}
