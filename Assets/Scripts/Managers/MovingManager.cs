using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct PokemonInGrass
{
    [Header("Sum must always to be 100%")]
    public PokemonData pokemon;
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
    [SerializeField] Tilemap grass = default;
    [SerializeField] GrassStruct[] grassStruct = default;

    [Header("Found Pokemon")]
    [SerializeField] string pathPokemon = "ScriptableObjects/Pokemons";
    [Min(1)] [SerializeField] int minPokemons = 1;
    [Min(1)] [SerializeField] int maxPokemons = 1;
    [SerializeField] int pokemonsLevel = 5;

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

    #endregion

    /// <summary>
    /// Before moving, check if there is path or there is a collision
    /// </summary>
    public bool CheckPath(Vector3 playerDestination)
    {
        //check if there is not collision tile
        return CheckTile(collision, playerDestination) == false;
    }

    #region random pokemon

    /// <summary>
    /// After moving, check if the player found a pokemon - random from pathPokemon
    /// </summary>
    public bool CheckRandomPokemon(Vector3 playerPosition)
    {
        //check percentage find pokemon
        if (PercentageFindPokemon())
        {
            //check if there is grass
            if (CheckTile(grass, playerPosition))
            {
                EnemySelectRandomPokemons();
                FoundPokemon();
                return true;
            }
        }

        return false;
    }

    void EnemySelectRandomPokemons()
    {
        FightManager fightManager = GameManager.instance.levelManager.FightManager;

        fightManager.enemyPokemons = new List<PokemonModel>();

        //get list of pokemons and random team quantity
        PokemonData[] pokemonDatas = Resources.LoadAll<PokemonData>(pathPokemon);
        int quantity = Random.Range(minPokemons, maxPokemons);

        //fill the quantity with random pokemon
        for (int i = 0; i < quantity; i++)
        {
            int random = Random.Range(0, pokemonDatas.Length);

            fightManager.enemyPokemons.Add(new PokemonModel(pokemonDatas[random], pokemonsLevel));
        }
    }

    #endregion

    #region pokemon

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

    void EnemySelectPokemons(PokemonInGrass[] pokemonList)
    {
        FightManager fightManager = GameManager.instance.levelManager.FightManager;

        fightManager.enemyPokemons = new List<PokemonModel>();

        //get random team quantity
        int quantity = Random.Range(minPokemons, maxPokemons);

        //fill the quantity with pokemonList
        for (int i = 0; i < quantity; i++)
        {
            //set a percentage
            int percentage = Random.Range(0, 100);

            PokemonData pokemon = null;
            int currentPercentage = 0;

            //cycle every pokemon and look for 'em percentages
            for(int j = 0; j < pokemonList.Length; j++)
            {
                //sum percentage (so the first maybe is 20%, the next is 10 in inspector, then is from 20 to 30%, and so on...)
                currentPercentage += pokemonList[j].percentage;

                //if is this one, set pokemon and stop cycle
                if(percentage < currentPercentage)
                {
                    pokemon = pokemonList[j].pokemon;
                    break;
                }
            }

            //if no pokemon, set the first of the list (someone didn't set nice percentages in the array .-.)
            if (pokemon == null)
                pokemon = pokemonList[0].pokemon;

            fightManager.enemyPokemons.Add(new PokemonModel(pokemon, pokemonsLevel));
        }
    }

    #endregion

    void FoundPokemon()
    {
        GameManager.instance.levelManager.StartFight();
    }
}
