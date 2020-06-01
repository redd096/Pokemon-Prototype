using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingManager : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] float percentageFindPokemon = 30;

    [SerializeField] Tilemap grass = default;
    [SerializeField] Tilemap collision = default;

    public Tilemap Grass { get { return grass; } }
    public Tilemap Collision { get { return collision; } }

    public bool CheckPath(Vector3 playerDestination)
    {
        //world to cell position
        Vector3Int cellPosition = Collision.WorldToCell(playerDestination);

        //check if there is not collision tile
        return Collision.GetTile(cellPosition) == null;
    }

    public bool CheckPokemon(Vector3 playerPosition)
    {
        //world to cell position
        Vector3Int cellPosition = Grass.WorldToCell(playerPosition);

        //check if there is grass
        if(Grass.GetTile(cellPosition) != null)
        {
            int random = Random.Range(0, 100);

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
        //start fight
        GameManager.instance.fightManager.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
