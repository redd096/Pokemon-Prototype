using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player { get; private set; }
    public LevelManager levelManager { get; private set; }
    public MovingManager movingManager { get; private set; }
    public FightManager fightManager { get; private set; }

    protected override void SetDefaults()
    {
        player = FindObjectOfType<Player>();
        levelManager = FindObjectOfType<LevelManager>();
        movingManager = FindObjectOfType<MovingManager>();
        fightManager = FindObjectOfType<FightManager>();
    }
}