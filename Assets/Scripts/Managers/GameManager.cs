using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int maxSkillForPokemon = 4;

    public Player player { get; private set; }
    public LevelManager levelManager { get; private set; }

    protected override void SetDefaults()
    {
        player = FindObjectOfType<Player>();
        levelManager = FindObjectOfType<LevelManager>();
    }
}