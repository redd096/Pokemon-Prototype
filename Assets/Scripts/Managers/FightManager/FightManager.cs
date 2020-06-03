using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    Animator anim;

    [Header("Arena")]
    public Transform playerPokemon = default;
    public Transform enemyPokemon = default;

    [Header("InfoBox")]
    public Text description = default;
    public GameObject playerMenu = default;
    public GameObject fightMenu = default;

    [Header("Menu")]
    public GameObject pokemonMenu = default;
    public GameObject bagMenu = default;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void StartFightPhase()
    {
        anim.SetTrigger("StartFightPhase");
    }

    #region player menu

    public void FightClick()
    {
        playerMenu.SetActive(false);
        fightMenu.SetActive(true);
    }

    public void PokemonClick()
    {
        playerMenu.SetActive(false);
        pokemonMenu.SetActive(true);
    }

    public void BagClick()
    {
        playerMenu.SetActive(false);
        bagMenu.SetActive(true);
    }

    public void RunClick()
    {
        playerMenu.SetActive(false);
        GameManager.instance.levelManager.StartMoving();
    }

    #endregion

    public void BackToPlayerMenu()
    {
        fightMenu.SetActive(false);
        pokemonMenu.SetActive(false);
        bagMenu.SetActive(false);

        playerMenu.SetActive(true);
    }
}
