using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
    [Header("Pokemon")]
    [SerializeField] PokemonData[] playerPokemonsData = default;
    [SerializeField] PokemonModel[] playerPokemons = default;

    [Header("Items")]
    [SerializeField] ItemData[] playerItemsData = default;

    [Header("Moving Phase")]
    [SerializeField] float durationMovement = 1;

    public PokemonModel[] PlayerPokemons { get { return playerPokemons; } }
    public ItemData[] PlayerItems { get { return playerItemsData; } }
    public float DurationMovement { get { return durationMovement; } }

    private void Start()
    {
        //foreach data, create a pokemon model
        playerPokemons = new PokemonModel[playerPokemonsData.Length];

        for (int i = 0; i < playerPokemonsData.Length; i++)
            playerPokemons[i] = new PokemonModel(playerPokemonsData[i], 5);
    }

    void Update()
    {
        state?.Execution();
    }

    #region public API

    public void StartMovingPhase()
    {
        //start state machine for moving
        SetState(new IdlePLayer(this));
    }

    public void StartFightPhase()
    {
        //start state machine for fight
        SetState(null);
    }

    #endregion
}
