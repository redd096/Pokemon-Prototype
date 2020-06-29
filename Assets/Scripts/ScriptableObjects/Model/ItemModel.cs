using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModel : IGetName
{
    public ItemData itemData;// { get; private set; }

    public int stack;

    public ItemModel(ItemData itemData)
    {
        //set data
        this.itemData = itemData;

        //add one stack
        AddItem(1);
    }

    public string GetButtonName()
    {
        return itemData.ItemName + " - x" + stack;
    }

    public string GetObjectName()
    {
        return itemData.ItemName;
    }

    public void AddItem(int quantity)
    {
        stack += quantity;
    }

    public void RemoveItem()
    {
        stack--;
    }

    /// <summary>
    /// Try catch with pokeball
    /// </summary>
    public bool TryCatch(PokemonModel pokemonToCatch, bool isWild)
    {
        //must to be a wild pokemon
        if (pokemonToCatch == null || pokemonToCatch.pokemonData == null || isWild == false)
            return false;

        int N = 0;
        switch (itemData.IsPokeball)
        {
            //if not a pokeball can't catch
            //if is a master ball, always catch 
            case EPokeBall.nothing:
                return false;
            case EPokeBall.masterBall:
                return true;

            //else generate random number
            case EPokeBall.pokeBall:
                N = Random.Range(0, 256);   //0 - 255
                break;
            case EPokeBall.megaBall:
                N = Random.Range(0, 201);   //0 - 200
                break;
            case EPokeBall.ultraBall:
                N = Random.Range(0, 151);    //0 - 150
                break;
            default:
                return false;
        }

        //( based on effects N should be decreased :/ )
        if (N < 0)
            return true;
        //if greater than catch rate, then catch is failed
        else if (N > pokemonToCatch.pokemonData.CatchRate)
            return false;

        //else generate random M from 0 to 255
        //generate ball, 8 for mega ball or 12 for other balls
        int M = Random.Range(0, 256);
        int ball = itemData.IsPokeball == EPokeBall.megaBall ? 8 : 12;

        //then f = ( (maxHealth * 255) / ball ) * (4 / currentHealth)
        //floor and clamp to 255
        float f = ((pokemonToCatch.MaxHealth * 255) / ball) * (4 / pokemonToCatch.CurrentHealth);
        f = Mathf.Min(Mathf.Floor(f), 255);

        //if f >= M catch - else fail
        if (f >= M)
            return true;
        else
            return false;
    }
}
