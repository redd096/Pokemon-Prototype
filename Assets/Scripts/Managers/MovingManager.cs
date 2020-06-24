using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct PokemonInGrass
{
    [Header("Sum must always to be 100%")]
    public PokemonData pokemon;
    public int level;
    [Range(0, 100)] public int percentage;
}

[System.Serializable]
public struct GrassStruct
{
    public Tilemap tile;
    public PokemonInGrass[] pokemons;
}

public class MovingManager : MonoBehaviour
{
    [Header("Pokemon")]
    [Range(0, 100)]
    [SerializeField] int percentageFindPokemon = 30;

    [Header("Tilemap")]
    [SerializeField] Tilemap collision = default;
    [SerializeField] GrassStruct[] grassStruct = default;

    [Header("Found Pokemon")]
    [Min(1)] [SerializeField] int minLevel = default;
    [Min(1)] [SerializeField] int minPokemons = 1;
    [Min(1)] [SerializeField] int maxPokemons = 1;

    #region private API

    bool CheckTile(Tilemap tileMap, Vector3 worldPosition)
    {
        //world to cell position
        Vector3Int cellPosition = tileMap.WorldToCell(worldPosition);

        //check if there is something here
        return tileMap.GetTile(cellPosition) != null;
    }

    bool PercentageFindPokemon()
    {
        int random = Random.Range(0, 100);

        return random < percentageFindPokemon;
    }

    void EnemySelectPokemons(PokemonInGrass[] pokemonList)
    {
        FightManager fightManager = GameManager.instance.levelManager.FightManager;
        fightManager.enemyPokemons = new List<PokemonModel>();

        //get random team quantity (max + 1 'cause last one is exclusive)
        int quantity = Random.Range(minPokemons, maxPokemons + 1);

        //fill the quantity with pokemonList
        for (int i = 0; i < quantity; i++)
        {
            //set a percentage
            int percentage = Random.Range(0, 100);

            PokemonData pokemon = null;
            int pokemonLevel = 0;
            int currentPercentage = 0;

            //cycle every pokemon and look for 'em percentages
            for (int j = 0; j < pokemonList.Length; j++)
            {
                //sum percentage (so the first maybe is 20%, the next is 10 in inspector, then is from 20 to 30%, and so on...)
                currentPercentage += pokemonList[j].percentage;

                //if is this one, set pokemon and stop cycle
                if (percentage < currentPercentage)
                {
                    pokemon = pokemonList[j].pokemon;
                    pokemonLevel = pokemonList[j].level;
                    break;
                }
            }

            //if no pokemon, set the first of the list (someone didn't set nice percentages in the array .-.)
            if (pokemon == null)
                pokemon = pokemonList[0].pokemon;

            fightManager.enemyPokemons.Add(new PokemonModel(pokemon, Mathf.Max(minLevel, pokemonLevel)));
        }
    }

    #endregion

    /// <summary>
    /// Before moving, check if there is path or there is a collision
    /// </summary>
    public bool CheckPath(Vector3 playerDestination)
    {
        //check if there is not collision tile
        return CheckTile(collision, playerDestination) == false;
    }

    /// <summary>
    /// After moving, check if the player found a pokemon
    /// </summary>
    public bool CheckPokemon(Vector3 playerPosition)
    {
        //check percentage find pokemon
        if (PercentageFindPokemon())
        {
            //foreach grass tile map
            foreach (GrassStruct grassTile in grassStruct)
            {
                //if player is in this tileMap
                if (CheckTile(grassTile.tile, playerPosition))
                {
                    EnemySelectPokemons(grassTile.pokemons);
                    FoundPokemon();
                    return true;
                }
            }
        }

        return false;
    }

    void FoundPokemon()
    {
        GameManager.instance.levelManager.StartFight();
    }
}
