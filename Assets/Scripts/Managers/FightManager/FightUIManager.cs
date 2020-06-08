using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightUIManager : MonoBehaviour
{
    [Header("ArenaPlayer")]
    public Image playerImage = default;
    public Text playerName = default;
    public Text playerLevel = default;
    public string playerLevelString = ":L ";
    public Slider playerHealthSlider = default;
    public Text playerHealth = default;
    public Slider playerExpSlider = default;

    [Header("ArenaEnemy")]
    public Image enemyImage = default;
    public Text enemyName = default;
    public Text enemyLevel = default;
    public string enemyLevelString = ":L ";
    public Slider enemyHealthSlider = default;
    public Text enemyHealth = default;
    public Slider enemyExpSlider = default;

    [Header("InfoBox")]
    public Text description = default;
    public GameObject playerMenu = default;
    public GameObject fightMenu = default;

    [Header("Menu")]
    public GameObject pokemonMenu = default;
    public GameObject bagMenu = default;


    #region setup fight state

    public void DeactiveEverything()
    {
        //deactive everything
        playerImage.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
        playerMenu.SetActive(false);
        fightMenu.SetActive(false);
        pokemonMenu.SetActive(false);
        bagMenu.SetActive(false);
    }

    public void SetArena(PokemonModel playerPokemon, PokemonModel enemyPokemon)
    {
        //set player sprite, life and all
        playerImage.sprite = playerPokemon.pokemonData.PokemonBack;
        playerName.text = playerPokemon.pokemonData.PokemonName;
        playerLevel.text = playerLevelString + playerPokemon.CurrentLevel;
        playerHealthSlider.value = playerPokemon.CurrentHealth / playerPokemon.pokemonData.Health;
        playerHealth.text = playerPokemon.CurrentHealth + " / " + playerPokemon.pokemonData.Health;
        playerExpSlider.value = (playerPokemon.CurrentExp - playerPokemon.ExpCurrentLevel) / (playerPokemon.ExpNextLevel - playerPokemon.ExpCurrentLevel);

        //set enemy sprite, life and all
        enemyImage.sprite = enemyPokemon.pokemonData.PokemonFront;
        enemyName.text = enemyPokemon.pokemonData.PokemonName;
        enemyLevel.text = enemyLevelString + enemyPokemon.CurrentLevel;
        enemyHealthSlider.value = enemyPokemon.CurrentHealth / enemyPokemon.pokemonData.Health;
        enemyHealth.text = enemyPokemon.CurrentHealth + " / " + enemyPokemon.pokemonData.Health;
        enemyExpSlider.value = (enemyPokemon.CurrentExp - enemyPokemon.ExpCurrentLevel) / (enemyPokemon.ExpNextLevel - enemyPokemon.ExpCurrentLevel);
    }

    public void SetPokemonList()
    {

    }

    public void SetItemsList()
    {

    }

    #endregion

    #region start fight state

    public void ResetElements()
    {
        playerImage.transform.localScale = Vector3.zero;
        description.text = string.Empty;
    }

    public void ActiveElements()
    {
        //active pokemon
        playerImage.gameObject.SetActive(true);

        //active description
        description.gameObject.SetActive(true);
    }

    public void SetDescription(string text, string[] args, float timeBetweenChar, float skipSpeed, System.Action onEndDescription)
    {
        //write description letter by letter. Then press a button and call OnEndDescription
        string s = string.Format(text, args);
        description.WriteLetterByLetterAndWait(s, timeBetweenChar, skipSpeed, onEndDescription);
    }

    public void PokemonAnimation(float delta)
    {
        //increase size
        playerImage.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, delta);
    }

    public void OnEndDescription()
    {
        //be sure to complete animation
        playerImage.transform.localScale = Vector3.one;
    }

    #endregion

    #region player round state

    public void DeactiveDescription()
    {
        description.gameObject.SetActive(false);
    }

    public void ActivePlayerMenu()
    {
        playerMenu.SetActive(true);
    }

    #endregion

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
