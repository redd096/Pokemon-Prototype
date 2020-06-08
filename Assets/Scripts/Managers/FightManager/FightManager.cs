using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    public FightUIManager FightUIManager { get; private set; }

    [Header("TODO")]
    public PokemonData[] enemyPokemonsData;
    public PokemonModel[] enemyPokemons;

    [Header("Skill when everything at 0 PP")]
    public SkillData baseSkill;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        FightUIManager = GetComponent<FightUIManager>();

        //foreach data, create a pokemon model
        enemyPokemons = new PokemonModel[enemyPokemonsData.Length];

        for (int i = 0; i < enemyPokemonsData.Length; i++)
            enemyPokemons[i] = new PokemonModel(enemyPokemonsData[i], 5);
    }

    public void StartFightPhase()
    {
        anim.SetTrigger("StartFightPhase");
    }

    #region player menu

    public void FightClick()
    {
        FightUIManager.FightClick();
    }

    public void PokemonClick()
    {
        FightUIManager.PokemonClick();
    }

    public void BagClick()
    {
        FightUIManager.BagClick();
    }

    public void RunClick()
    {
        FightUIManager.RunClick();
        GameManager.instance.levelManager.StartMoving();
    }

    #endregion

    public void BackToPlayerMenu()
    {
        FightUIManager.BackToPlayerMenu();
    }
}
