using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] MovingManager movingManager = default;
    [SerializeField] FightManager fightManager = default;

    [Header("Music")]
    [SerializeField] AudioClip musicMovingPhase = default;
    [SerializeField] AudioClip musicFightPhase = default;

    public MovingManager MovingManager { get { return movingManager; } }
    public FightManager FightManager { get { return fightManager; } }
    public AudioClip MusicMovingPhase { get { return musicMovingPhase; } }
    public AudioClip MusicFightPhase { get { return musicFightPhase; } }

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    #region for states

    public void Wait(float duration, System.Action onEnd)
    {
        StartCoroutine(Wait_Coroutine(duration, onEnd));
    }

    IEnumerator Wait_Coroutine(float duration, System.Action onEnd)
    {
        yield return new WaitForSeconds(duration);

        onEnd?.Invoke();
    }

    #endregion

    public void StartFight()
    {
        anim.SetTrigger("Fight");
    }

    public void StartMoving()
    {
        anim.SetTrigger("Moving");
    }

    #region moving and fight manager

    /// <summary>
    /// Before moving, check if there is path or there is a collision
    /// </summary>
    public bool CheckPath(Vector3 playerDestination)
    {
        return movingManager.CheckPath(playerDestination);
    }

    /// <summary>
    /// After moving, check if the player found a pokemon
    /// </summary>
    public bool CheckPokemon(Vector3 playerPosition)
    {
        return movingManager.CheckPokemon(playerPosition);
    }

    /// <summary>
    /// Set list of pokemons for this fight
    /// </summary>
    public void SetEnemyPokemons(List<PokemonModel> pokemons)
    {
        fightManager.enemyPokemons = pokemons;
    }

    #endregion
}
