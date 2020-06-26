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
    [SerializeField] Gradient HealthGradient = default;
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
    [SerializeField] GameObject yesNoMenu = default;
    [SerializeField] Button yesButton = default;
    [SerializeField] Button nopeButton = default;
    [SerializeField] GameObject learnSkillsMenu = default;
    [SerializeField] Transform contentLearnSkillMenu = default;
    [SerializeField] Button refuseSkillButton = default;
    #endregion

    #region private poolings
    Pooling<Button> skillsPooling = new Pooling<Button>(false);
    Pooling<Button> pokemonsPooling = new Pooling<Button>(false);
    Pooling<Button> itemsPooling = new Pooling<Button>(false);

    Pooling<Button> skillsToReplacePooling = new Pooling<Button>(false);
    #endregion

    #region private variables

    Coroutine doingAnimation;
    Coroutine updatingBar;

    Vector3 playerPosition;
    Vector3 enemyPosition;

    #endregion

    //TODO
    //VA MESSO UN CAP AL NUMERO DI POKEMON TRASPORTABILI DAL GIOCATORE
    //VANNO AGGIUNTO LE POKEBALL

    /*
        da fare se rimane tempo:
        NB. si potrebbe fare anche FightManager singleton e MovingManager differente per ogni scena, l'unico problema è l'event system che dovrà diventare singleton anche lui
        - interazioni nello stato di moving, come ad esempio comprare o trovare items
        - suoni
        - allenatori sparsi per la mappa con cui parlare (combattimenti con allenatore invece che pokemon selvatici, guarda la formula dell'exp ottenuta) (StartFightState description?)
        - salvataggio all'uscita dal gioco (o alla peggio, nel fade out quando finisce un combattimento, o quando si rigenerano i pokemon all'ospedale)
        - se si vuole esagerare, i pokemon mantengono i danni subiti e PP e bisogna farli curare, quindi aggiungere ospedali e rimuovere il Restore da RunClick()
        - aggiungere menù opzioni nel menù di pausa e nel main menu (con volume e full screen mode - si potrebbe mettere anche la velocità di scrittura)
        - andrebbe gestita anche la fuga, per ora è solo un clicca Run e si torna in fase di moving
     */

    #region private API

    #region buttons

    void UseSkill(Button button, SkillModel skill)
    {
        //don't do anything when there is no skill or PP are 0
        if (skill == null || skill.CurrentPP <= 0)
            return;

        //call it in fight manager
        GameManager.instance.LevelManager.FightManager.UseSkill(skill);

        //change PP in text
        button.GetComponentInChildren<Text>().text = skill.GetButtonName();
    }

    void ChangePokemon(Button button, PokemonModel pokemon)
    {
        //don't change if dead
        if (pokemon.CurrentHealth <= 0)
            return;

        //call it in fight manager
        GameManager.instance.LevelManager.FightManager.ChangePokemon(pokemon);

        //move current pokemon in arena to the list of pokemons
        SetButton(button, GameManager.instance.LevelManager.FightManager.currentPlayerPokemon, ChangePokemon);
    }

    void UseItem(Button button, ItemModel item)
    {
        //call it in fight manager
        GameManager.instance.LevelManager.FightManager.UseItem(item);

        //update stacks or remove button
        if(item.stack > 0)
            button.GetComponentInChildren<Text>().text = item.GetButtonName();
        else
            Pooling.Destroy(button.gameObject);
    }

    #endregion

    #region set lists

    void SetList<T>(Pooling<Button> poolingList, List<T> valueArray, Transform parent, System.Action<Button, T> function) where T : IGetName
    {
        //deactive every button
        poolingList.DeactiveAll();

        //add if there are not enough buttons in pool
        poolingList.InitCycle(prefabSimpleButton, valueArray.Count);

        //foreach value
        foreach (T value in valueArray)
        {
            //instantiate button from pool and set parent
            Button button = poolingList.Instantiate(prefabSimpleButton, parent, false);

            //and set it
            SetButton(button, value, function);
        }
    }

    void SetButton<T>(Button button, T value, System.Action<Button, T> function) where T : IGetName
    {
        //be sure to not have listeners
        button.onClick.RemoveAllListeners();

        //add new listener to button
        button.onClick.AddListener(() => function(button, value));

        //set button name or empty
        button.GetComponentInChildren<Text>().text = value != null ? value.GetButtonName() : "-";
    }

    #endregion

    #region update health

    IEnumerator UpdateHealth_Coroutine(bool isPlayer, float previousValue, float durationUpdate, System.Action onEndUpdate)
    {
        float delta = 0;

        //update bar
        while (delta < 1)
        {
            delta += Time.deltaTime / durationUpdate;
            SetHealthUI(isPlayer, previousValue, delta);
            yield return null;
        }

        //be sure to end animation
        SetHealthUI(isPlayer, previousValue, 1);

        onEndUpdate?.Invoke();

        updatingBar = null;
    }

    void SetHealthUI(bool isPlayer, float previousHealth, float delta)
    {
        //get what to edit
        Slider slider = isPlayer ? playerHealthSlider : enemyHealthSlider;
        Text text = isPlayer ? playerHealth : enemyHealth;
        PokemonModel pokemon = isPlayer ? GameManager.instance.LevelManager.FightManager.currentPlayerPokemon : GameManager.instance.LevelManager.FightManager.currentEnemyPokemon;

        //current health based on delta
        float currentHealth = Mathf.Lerp(previousHealth, pokemon.CurrentHealth, delta);

        //set slider and text
        slider.value = currentHealth / pokemon.pokemonData.Health;
        slider.fillRect.GetComponent<Image>().color = HealthGradient.Evaluate(currentHealth / pokemon.pokemonData.Health);
        text.text = currentHealth.ToString("F0") + " / " + pokemon.pokemonData.Health.ToString("F0");
    }

    #endregion

    #region update experience

    IEnumerator UpdateExperience_Coroutine(float previousValue, float durationUpdate, System.Action<float> onEndUpdate)
    {
        float delta = 0;
        float updatedExp = 0;

        //update bar
        while (delta < 1)
        {
            delta += Time.deltaTime / durationUpdate;

            //if true, break before end update
            if (SetExperienceUI(previousValue, delta, out updatedExp))
                break;

            yield return null;
        }

        updatingBar = null;

        //call function, with updatedExp to know where the animation end
        onEndUpdate?.Invoke(updatedExp);
    }

    bool SetExperienceUI(float previousExp, float delta, out float currentExp)
    {
        //get player pokemon
        PokemonModel pokemon = GameManager.instance.LevelManager.FightManager.currentPlayerPokemon;

        //current exp based on delta
        currentExp = Mathf.Lerp(previousExp, pokemon.CurrentExp, delta);

        //set player slider
        playerExpSlider.value = (currentExp - pokemon.ExpCurrentLevel) / (pokemon.ExpNextLevel - pokemon.ExpCurrentLevel);

        //return true if already reached next level (also if currentExp didn't reach pokemon.CurrentExp)
        if (currentExp > pokemon.ExpNextLevel)
            return true;

        return false;
    }

    #endregion

    #endregion

    #region states

    #region set arena state

    public void DeactiveEverything()
    {
        //deactive everything
        playerImage.gameObject.SetActive(false);

        EndDescription();

        DeactiveMenu();
    }

    public void SetArena()
    {
        SetPokemonInArena(true);
        SetPokemonInArena(false);

        //set default position (in set arena, because when player moves, can move camera too)
        playerPosition = playerImage.transform.position;
        enemyPosition = enemyImage.transform.position;
    }

    public void SetPokemonList()
    {
        List<PokemonModel> playerPokemons = GameManager.instance.Player.PlayerPokemons;
        PokemonModel pokemonInArena = GameManager.instance.LevelManager.FightManager.currentPlayerPokemon;

        //foreach pokemon of the player
        List<PokemonModel> pokemonsUsable = new List<PokemonModel>();
        for (int i = 0; i < playerPokemons.Count; i++)
        {
            //check if it isn't the pokemon in arena, add to the list
            if (playerPokemons[i] != pokemonInArena)
                pokemonsUsable.Add(playerPokemons[i]);
        }

        //list of pokemons not in arena
        SetList(pokemonsPooling, pokemonsUsable, contentPokemonMenu, ChangePokemon);
    }

    public void SetItemsList()
    {
        SetList(itemsPooling, GameManager.instance.Player.PlayerItems, contentBagMenu, UseItem);
    }

    #endregion

    #region start fight state

    public void ActivePokemonImage()
    {
        //reset player image for animation
        playerImage.transform.localScale = Vector3.zero;

        //active pokemon
        playerImage.gameObject.SetActive(true);
    }

    #endregion

    #region someone turn state

    public void ActivePlayerMenu()
    {
        playerMenu.SetActive(true);
    }

    #endregion

    #region skill state

    public void AttackAnimation(bool isPlayer, float delta)
    {
        //get what to edit
        Transform tr = isPlayer ? playerImage.transform : enemyImage.transform;
        Vector3 startPosition = isPlayer ? playerPosition : enemyPosition;
        Vector3 endPosition = isPlayer ? enemyPosition : playerPosition;

        //move player to enemy and come back (or viceversa)
        tr.position = Vector3.Lerp(startPosition, endPosition, delta);
    }

    #endregion

    #region experience player state

    public void UpdateExperience(float previousExp, float durationUpdateExp, System.Action<float> onEndUpdateExp = null)
    {
        //stop if already running
        if (updatingBar != null)
            StopCoroutine(updatingBar);

        //start coroutine
        updatingBar = StartCoroutine(UpdateExperience_Coroutine(previousExp, durationUpdateExp, onEndUpdateExp));
    }

    public void UpdateLevel(int level)
    {
        playerLevel.text = playerLevelString + level;
    }

    public void ShowYesNoMenu(System.Action yesFunc, System.Action noFunc)
    {
        //active yes no menu
        yesNoMenu.SetActive(true);

        //be sure to not have listeners
        yesButton.onClick.RemoveAllListeners();
        nopeButton.onClick.RemoveAllListeners();

        //add new listener to buttons
        yesButton.onClick.AddListener(() => yesFunc());
        nopeButton.onClick.AddListener(() => noFunc());
    }

    public void HideYesNoMenu()
    {
        yesNoMenu.SetActive(false);
    }

    #endregion

    #region learn skill state

    public void SetLearnSkillsMenu(System.Action<int> confirmFunc, System.Action refuseFunc)
    {
        //set refuse skill button
        refuseSkillButton.onClick.RemoveAllListeners();
        refuseSkillButton.onClick.AddListener(() => refuseFunc());

        //deactive every button in the pooling and add if there are not enough buttons in pool
        skillsToReplacePooling.DeactiveAll();
        skillsToReplacePooling.InitCycle(prefabSimpleButton, GameManager.instance.MaxSkillForPokemon);

        //get current skills of the pokemon
        List<SkillModel> currentSkills = GameManager.instance.LevelManager.FightManager.currentPlayerPokemon.CurrentSkills;

        for (int i = 0; i < Mathf.Min(currentSkills.Count +1, GameManager.instance.MaxSkillForPokemon); i++)
        {
            //instantiate button from pool and set parent
            Button button = skillsToReplacePooling.Instantiate(prefabSimpleButton, contentLearnSkillMenu, false);

            //be sure to not have listeners
            button.onClick.RemoveAllListeners();

            //add new listener to button
            int indexForLambda = i;                                         //for some reason, need to make a copy of anything you want to access for the lambda
            button.onClick.AddListener(() => confirmFunc(indexForLambda));

            //set text (name of the skill or empty)
            button.GetComponentInChildren<Text>().text = i < currentSkills.Count ? currentSkills[i].GetObjectName() : "-";
        }
    }

    public void ShowLearnSkillsMenu()
    {
        learnSkillsMenu.SetActive(true);
    }

    public void HideLearnSkillsMenu()
    {
        learnSkillsMenu.SetActive(false);
    }

    #endregion

    #region generic functions

    #region description

    public void SetDescription(string text, System.Action onEndDescription)
    {
        //reset description and active it
        description.text = string.Empty;
        description.gameObject.SetActive(true);

        //write description letter by letter. Then press a button and call OnEndDescription
        string s = Parse(text);

        description.WriteLetterByLetterAndWait_SkipAccelerate(s, onEndDescription);
    }

    public void EndDescription()
    {
        description.gameObject.SetActive(false);
    }

    string Parse(string text)
    {
        FightManager fightManager = GameManager.instance.LevelManager.FightManager;

        string s = text;

        //replace string with data
        Replace(ref s, "{PlayerPokemon}", fightManager.currentPlayerPokemon);
        Replace(ref s, "{EnemyPokemon}", fightManager.currentEnemyPokemon);
        Replace(ref s, "{Skill}", fightManager.SkillUsed);
        Replace(ref s, "{Pokemon}", fightManager.PokemonSelected);
        Replace(ref s, "{Item}", fightManager.ItemUsed);
        Replace(ref s, "{SkillToLearn}", fightManager.SkillToLearn);

        return s;
    }

    void Replace(ref string text, string toReplace, IGetName control)
    {
        //if contains && control != null -> replace string with object name
        if (text.Contains(toReplace) && control != null)
            text = text.Replace(toReplace, control.GetObjectName());
    }

    #endregion

    #region coroutine animations

    public void StartAnimation(IEnumerator animation)
    {
        //stop if already running
        if (doingAnimation != null)
            StopCoroutine(doingAnimation);

        //start coroutine
        doingAnimation = StartCoroutine(animation);
    }

    public void EndAnimation()
    {
        doingAnimation = null;
    }

    #endregion

    public void DeactiveMenu()
    {
        playerMenu.SetActive(false);
        fightMenu.SetActive(false);
        pokemonMenu.SetActive(false);
        bagMenu.SetActive(false);
        yesNoMenu.SetActive(false);
        learnSkillsMenu.SetActive(false);
    }

    public void UpdateHealth(bool isPlayer, float previousHealth, float durationUpdateHealth, System.Action onEndUpdateHealth = null)
    {
        //stop if already running
        if (updatingBar != null)
            StopCoroutine(updatingBar);

        //start coroutine
        updatingBar = StartCoroutine(UpdateHealth_Coroutine(isPlayer, previousHealth, durationUpdateHealth, onEndUpdateHealth));
    }

    public void SetSkillsList(PokemonModel pokemon)
    {
        //set list of player skills
        SetList(skillsPooling, pokemon.CurrentSkills, contentFightMenu, UseSkill);
    }

    public void PokemonSpawnAnimation(bool isPlayer, float delta)
    {
        Transform tr = isPlayer ? playerImage.transform : enemyImage.transform;

        //set size
        tr.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, delta);
    }

    public void SetPokemonInArena(bool isPlayer)
    {
        //get player or enemy pokemon
        PokemonModel pokemon = isPlayer ? GameManager.instance.LevelManager.FightManager.currentPlayerPokemon : GameManager.instance.LevelManager.FightManager.currentEnemyPokemon;

        if (isPlayer)
        {
            //set player sprite, life and all
            playerImage.sprite = pokemon.pokemonData.PokemonBack;
            playerName.text = pokemon.GetObjectName();
            playerLevel.text = playerLevelString + pokemon.CurrentLevel;
            SetHealthUI(true, 0, 1);
            playerExpSlider.value = (pokemon.CurrentExp - pokemon.ExpCurrentLevel) / (pokemon.ExpNextLevel - pokemon.ExpCurrentLevel);
        }
        else
        {
            //set enemy sprite, life and all
            enemyImage.sprite = pokemon.pokemonData.PokemonFront;
            enemyName.text = pokemon.GetObjectName();
            enemyLevel.text = enemyLevelString + pokemon.CurrentLevel;
            SetHealthUI(false, 0, 1);
            enemyExpSlider.value = (pokemon.CurrentExp - pokemon.ExpCurrentLevel) / (pokemon.ExpNextLevel - pokemon.ExpCurrentLevel);
        }
    }

    #endregion

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

    public void BackToPlayerMenu()
    {
        //deactive other menu
        fightMenu.SetActive(false);
        pokemonMenu.SetActive(false);
        bagMenu.SetActive(false);

        //and active player menu
        playerMenu.SetActive(true);
    }

    public void BackButton()
    {
        //if in an other menu, back to player menu
        if (playerMenu.activeInHierarchy == false)
        {
            BackToPlayerMenu();
        }
        //else go to pause menu
        else
        {
            GameManager.instance.PauseResumeGame();
        }
    }

    #endregion
}
