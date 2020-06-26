using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int MaxSkillForPokemon = 4;
    public int MaxPokemonInTeam = 6;

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

        //if paused, then resume
        if (isPaused)
        {
            //hide pause menu and enable player input
            instance.UiManager.PauseMenu(false);

            //set timeScale to 1
            Time.timeScale = 1;
        }
        //else pause
        else
        {
            //show pause menu and disable player input
            instance.UiManager.PauseMenu(true);

            //stop time
            Time.timeScale = 0;
        }
    }
}