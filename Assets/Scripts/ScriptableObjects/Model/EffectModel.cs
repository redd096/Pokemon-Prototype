using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectModel
{
    public EffectData effectData;

    public int CurrentDuration;

    public EffectModel(EffectData effectData)
    {
        //set data
        this.effectData = effectData;

        //set default duration
        CurrentDuration = effectData.Duration;
    }

    public void ApplyEffect(PokemonModel pokemon, bool isPlayerPokemon, out string effectDescription)
    {
        CurrentDuration--;

        //apply effect to the pokemon
        effectData.ApplyEffect(pokemon);

        //if duration <= 0, remove from the list of the pokemon
        if (CurrentDuration <= 0)
            pokemon.RemoveEffect(effectData);

        //out description
        effectDescription = isPlayerPokemon ? effectData.DescriptionOnPlayer : effectData.DescriptionOnEnemy;
    }
}
