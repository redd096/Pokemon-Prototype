using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFight : FightManagerState
{
    [Header("Player Won?")]
    [SerializeField] bool isWin = true;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        //TODO devi calcolare un botto di stronzate per salire di livello e item droppati        

        fightManager.RunClick();

        //HO INVERTITO LA STATE MACHINE, QUANDO IL PLAYER SKILLA E UCCIDE IL NEMICO, DOVREBBE ANDARE NEL CHANGE POKEMON DEL NEMICO E NON DEL PLAYER
        //E STESSA COSA QUANDO IL NEMICO SKILLA E UCCIDE IL GIOCATORE

        //QUANDO SKILLA IL NEMICO SI ROMPE QUALCOSA
        //A CAMBIARE POKEMON NON VEDI LA VITA A VOLTE, NON AGGIORNA LA BARRA, A VOLTE RIMETTE IL POKEMON GIà PRESENTE, ECC.........

        //CHECKA SetArenaState PERCHé MI SA CHE AL NEMICO NON SI REFULLANO I POKEMON

        //CAMBIEREI IL NOME DEL SetupState IN SetPlayerMenuState
    }

}
