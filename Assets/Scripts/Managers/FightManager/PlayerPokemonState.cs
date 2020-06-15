using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPokemonState : FightManagerState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (fightManager.pokemonSelected == fightManager.currentPlayerPokemon)
        {
            //dovrà esserci una infobox pure in fondo alla lista pokemon
            //che mi dirà che questo è già il pokemon selezionato e tanti saluti

            //return;
        }
        //TODO
        //hide pokemon menu
        //show description from which pokemon to new one
        //show animation spawn new pokemon
        //set new skills pooling
        //start enemy turn
        fightManager.SetCurrentPlayerPokemon(fightManager.pokemonSelected);
    }
}
