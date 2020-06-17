using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StateMachine
{
    [Header("Pokemon")]
    [SerializeField] PokemonData[] playerPokemonsData = default;
    [SerializeField] List<PokemonModel> playerPokemons = new List<PokemonModel>();

    [Header("Items")]
    [SerializeField] ItemData[] playerItemsData = default;
    [SerializeField] List<ItemModel> playerItems = new List<ItemModel>();

    [Header("Moving Phase")]
    [SerializeField] float durationMovement = 1;
    [SerializeField] bool moveCamera = false;

    public List<PokemonModel> PlayerPokemons { get { return playerPokemons; } }
    public List<ItemModel> PlayerItems { get { return playerItems; } }
    public float DurationMovement { get { return durationMovement; } }

    Transform cam;
    Vector3 offsetCamera;

    private void Start()
    {
        //find camera and offset
        cam = Camera.main.transform;
        offsetCamera = cam.position - transform.position;

        //foreach data, create a pokemon model
        for (int i = 0; i < playerPokemonsData.Length; i++)
            playerPokemons.Add( new PokemonModel(playerPokemonsData[i], 5) );

        //foreach data, create a item model
        for (int i = 0; i < playerItemsData.Length; i++)
            playerItems.Add(new ItemModel(playerItemsData[i]));
    }

    void Update()
    {
        state?.Execution();
    }

    private void LateUpdate()
    {
        //move camera
        if(moveCamera)
            cam.position = transform.position + offsetCamera;
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
}
