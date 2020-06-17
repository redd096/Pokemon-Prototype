using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingManager : MonoBehaviour
{
    [Header("Pokemon")]
    [Range(0, 100)]
    [SerializeField] float percentageFindPokemon = 30;

    [Header("Tilemap")]
    [SerializeField] Tilemap grass = default;
    [SerializeField] Tilemap collision = default;

    [Header("Found Pokemon")]
    [SerializeField] string pathPokemon = "ScriptableObjects/Pokemons";
    [Min(1)] public int minPokemons = 1;
    [Min(1)] public int maxPokemons = 1;

    /// <summary>
    /// Before moving, check if there is path or there is a collision
    /// </summary>
    public bool CheckPath(Vector3 playerDestination)
    {
        //world to cell position
        Vector3Int cellPosition = collision.WorldToCell(playerDestination);

        //check if there is not collision tile
        return collision.GetTile(cellPosition) == null;
    }

    /// <summary>
    /// After moving, check if the player found a pokemon
    /// </summary>
    public bool CheckPokemon(Vector3 playerPosition)
    {
        //world to cell position
        Vector3Int cellPosition = grass.WorldToCell(playerPosition);

        //check if there is grass
        if(grass.GetTile(cellPosition) != null)
        {
            int random = Random.Range(0, 100);

            //check percentage find pokemon
            if(random < percentageFindPokemon)
            {
                FoundPokemon();
                return true;
            }
        }

        return false;
    }

    void FoundPokemon()
    {
        EnemySelectPokemons();

        GameManager.instance.levelManager.StartFight();
    }

    void EnemySelectPokemons()
    {
        FightManager fightManager = GameManager.instance.levelManager.FightManager;

        //get list of pokemons and random team quantity
        PokemonData[] pokemonDatas = Resources.LoadAll<PokemonData>(pathPokemon);
        int quantity = Random.Range(minPokemons, maxPokemons);

        fightManager.enemyPokemons = new List<PokemonModel>();

        //fill the quantity with random pokemon
        for (int i = 0; i < quantity; i++)
        {
            int random = Random.Range(0, pokemonDatas.Length);

            fightManager.enemyPokemons.Add(new PokemonModel(pokemonDatas[random], 5));
        }
    }
}
