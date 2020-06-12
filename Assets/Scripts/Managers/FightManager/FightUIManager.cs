using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightUIManager : MonoBehaviour
{
    #region serialized variables
    [Header("Prefabs")]
    [SerializeField] Button prefabSimpleButton = default;

    [Header("ArenaPlayer")]
    [SerializeField] Image playerImage = default;
    [SerializeField] Text playerName = default;
    [SerializeField] Text playerLevel = default;
    [SerializeField] string playerLevelString = ":L ";
    [SerializeField] Slider playerHealthSlider = default;
    [SerializeField] Text playerHealth = default;
    [SerializeField] Slider playerExpSlider = default;

    [Header("ArenaEnemy")]
    [SerializeField] Image enemyImage = default;
    [SerializeField] Text enemyName = default;
    [SerializeField] Text enemyLevel = default;
    [SerializeField] string enemyLevelString = ":L ";
    [SerializeField] Slider enemyHealthSlider = default;
    [SerializeField] Text enemyHealth = default;
    [SerializeField] Slider enemyExpSlider = default;

    [Header("InfoBox")]
    [SerializeField] Text description = default;
    [SerializeField] GameObject playerMenu = default;
    [SerializeField] GameObject fightMenu = default;
    [SerializeField] Transform contentFightMenu = default;

    [Header("Menu")]
    [SerializeField] GameObject pokemonMenu = default;
    [SerializeField] Transform contentPokemonMenu = default;
    [SerializeField] GameObject bagMenu = default;
    [SerializeField] Transform contentBagMenu = default;
    #endregion

    #region private poolings
    Pooling<Button> skillsPooling = new Pooling<Button>();
    Pooling<Button> pokemonsPooling = new Pooling<Button>();
    Pooling<Button> itemsPooling = new Pooling<Button>();
    #endregion

    private void Start()
    {
        //remove every button from fight menu
        foreach(Transform child in contentFightMenu)
        {
            Destroy(child.gameObject);
        }
        //remove every button from pokemon menu
        foreach (Transform child in contentPokemonMenu)
        {
            Destroy(child.gameObject);
        }
        //remove every button from bag menu
        foreach (Transform child in contentBagMenu)
        {
            Destroy(child.gameObject);
        }
    }

    #region private API

    void UseSkill(Button button, SkillModel skill)
    {
        //call it in fight manager
        GameManager.instance.levelManager.FightManager.UseSkill(skill);

        //change PP in text
        button.GetComponentInChildren<Text>().text = skill.GetButtonName();
    }

    void ChangePokemon(Button button, PokemonModel pokemon)
    {
        //call it in fight manager
        GameManager.instance.levelManager.FightManager.ChangePokemon(pokemon);
    }

    void UseItem(Button button, ItemData item)
    {
        //call it in fight manager
        GameManager.instance.levelManager.FightManager.UseItem(item);

        //deactive this button item
        button.gameObject.SetActive(false);
    }

    void SetList<T>(Pooling<Button> poolingList, T[] valueArray, Transform parent, System.Action<Button, T> function) where T : IGetButtonName
    {
        //deactive every button
        poolingList.DeactiveAll();

        //foreach value
        foreach (T value in valueArray)
        {
            //instantiate button from pool and set parent
            Button button = poolingList.Instantiate(prefabSimpleButton);
            button.transform.SetParent(parent, false);

            //and set it
            SetButton(button, value, function, value.GetButtonName());
        }
    }

    void SetButton<T>(Button button, T value, System.Action<Button, T> function, string text)
    {
        //be sure to not have listeners
        button.onClick.RemoveAllListeners();

        //add new listener to button
        button.onClick.AddListener(() => function(button, value));

        //set text
        button.GetComponentInChildren<Text>().text = text;
    }

    #endregion

    #region set arena state

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

    #endregion

    #region setup fight state

    public void SetSkillsList(PokemonModel pokemon)
    {
        SetList(skillsPooling, pokemon.CurrentSkills, contentFightMenu, UseSkill);
    }

    public void SetPokemonList()
    {
        SetList(pokemonsPooling, GameManager.instance.player.PlayerPokemons, contentPokemonMenu, ChangePokemon);
    }

    public void SetItemsList()
    {
        SetList(itemsPooling, GameManager.instance.player.PlayerItems, contentBagMenu, UseItem);
    }

    #endregion

    #region start fight state

    public void ResetElements()
    {
        //reset player image for animation
        playerImage.transform.localScale = Vector3.zero;

        //reset description before set it
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
        //from player menu to fight menu
        playerMenu.SetActive(false);
        fightMenu.SetActive(true);
    }

    public void PokemonClick()
    {
        //from player menu to pokemon menu
        playerMenu.SetActive(false);
        pokemonMenu.SetActive(true);
    }

    public void BagClick()
    {
        //from player menu to bag menu
        playerMenu.SetActive(false);
        bagMenu.SetActive(true);
    }

    public void RunClick()
    {
        //remove player menu, cause the player will try to run away
        playerMenu.SetActive(false);
    }

    #endregion

    public void BackToPlayerMenu()
    {
        //deactive other menu
        fightMenu.SetActive(false);
        pokemonMenu.SetActive(false);
        bagMenu.SetActive(false);

        //and active player menu
        playerMenu.SetActive(true);
    }
}
