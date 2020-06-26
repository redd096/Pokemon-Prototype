using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int MaxSkillForPokemon = 4;
    public int MaxPokemonInTeam = 6;

    [Header("Efficiency TAB")]
    public EfficiencyTAB EfficiencyTAB;

    public Player Player { get; private set; }
    public LevelManager LevelManager { get; private set; }
    public UIManager UiManager { get; private set; }

    protected override void SetDefaults()
    {
        Player = FindObjectOfType<Player>();
        LevelManager = FindObjectOfType<LevelManager>();
        UiManager = FindObjectOfType<UIManager>();
    }

    /// <summary>
    /// Pause or resume
    /// </summary>
    public void PauseResumeGame()
    {
        bool isPaused = Time.timeScale == 0;

        //show or hide pause menu and set time
        instance.UiManager.PauseMenu(!isPaused);
        Time.timeScale = isPaused ? 1 : 0;
    }
}