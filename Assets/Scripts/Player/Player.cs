using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
    [Header("Pokemon")]
    [SerializeField] int pokemonsLevel = 5;
    [SerializeField] PokemonData[] playerPokemonsData = default;
    [SerializeField] List<PokemonModel> playerPokemons = new List<PokemonModel>();

    [Header("Items")]
    [SerializeField] ItemData[] playerItemsData = default;
    [SerializeField] List<ItemModel> playerItems = new List<ItemModel>();

    [Header("Moving Phase")]
    [SerializeField] float durationMovement = 1;
    public bool moveCamera = false;

    public List<PokemonModel> PlayerPokemons { get { return playerPokemons; } }
    public List<ItemModel> PlayerItems { get { return playerItems; } }
    public float DurationMovement { get { return durationMovement; } }

    #region for states

    public Camera cam { get; private set; }
    public Vector3 offsetCamera { get; private set; }

    public System.Action<string> movePlayer;

    #endregion

    private void Start()
    {
        //find camera and offset
        cam = Camera.main;
        offsetCamera = cam.transform.position - transform.position;

        //foreach data, create a pokemon model
        for (int i = 0; i < Mathf.Min(playerPokemonsData.Length, GameManager.instance.MaxPokemonInTeam); i++)
        {
            playerPokemons.Add(new PokemonModel(playerPokemonsData[i], pokemonsLevel));
        }

        //foreach data, add in inventory
        for (int i = 0; i < playerItemsData.Length; i++)
        {
            AddItem(new ItemModel(playerItemsData[i]));
        }
    }

    void Update()
    {
        state?.Execution();
    }

    #region public API

    #region state machine

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

    #region buttons

    public void MovePlayer(string direction)
    {
        movePlayer?.Invoke(direction);
    }

    #endregion

    #region for states

    public void CheckPause()
    {
        //if press back or start Pause/Resume
        //if press B button only Resume
        if ( Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7) || (Time.timeScale == 0 && Input.GetKeyDown(KeyCode.Joystick1Button1)) )
        {
            GameManager.instance.PauseResumeGame();
        }
    }

    #endregion

    #region pokemon

    public void ReplacePokemon(PokemonModel pokemonToReplace, PokemonModel newPokemon)
    {
        //look in the list
        for(int i = 0; i < playerPokemons.Count; i++)
        {
            //when found pokemonToReplace
            if(playerPokemons[i] == pokemonToReplace)
            {
                //remove
                playerPokemons.Remove(pokemonToReplace);

                //and add new one in the same position
                playerPokemons.Insert(i, newPokemon);
            }
        }
    }

    #endregion

    #region item

    public void AddItem(ItemModel item)
    {
        //look in inventory
        foreach(ItemModel itemInInventory in playerItems)
        {
            //if contain this item, add stacks
            if(itemInInventory.itemData == item.itemData)
            {
                itemInInventory.AddItem(item.stack);
                return;
            }
        }

        //if not in inventory, add to the list
        playerItems.Add(item);
    }

    public void RemoveItem(ItemModel item)
    {
        //remove stack
        item.RemoveItem();

        //if no item, remove from list
        if(item.stack <= 0)
            playerItems.Remove(item);
    }

    #endregion

    #endregion
}
