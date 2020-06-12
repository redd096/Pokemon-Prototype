using UnityEngine;

public enum EType
{
    normal,
    fire,
    water,
    grass,
}

public enum ESpeedExperience
{
    fast,
    mid_fast,
    mid_slow,
    slow,
}

[System.Serializable]
public struct SPokemonSkill
{
    public SkillData skill;
    public int level;
}

[CreateAssetMenu(fileName = "Pokemon", menuName = "PokemonPrototype/Pokemon")]
public class PokemonData : ScriptableObject
{
    [Header("Important")]
    [SerializeField] string pokemonName = "Pokemon";
    [SerializeField] EType pokemonType = EType.normal;
    [SerializeField] ESpeedExperience speedExperience = ESpeedExperience.mid_fast;

    [Header("For the view")]
    [SerializeField] Sprite pokemonFront = default;
    [SerializeField] Sprite pokemonBack = default;

    [Header("Attack")]
    [SerializeField] float health = 100;
    [SerializeField] float speed = 45;
    [SerializeField] float physicsAttack = 10;
    [SerializeField] float physicsDefense = 10;
    [SerializeField] float specialAttack = 10;
    [SerializeField] float specialDefense = 10;
    [SerializeField] SPokemonSkill[] possibleSkills = default;

    [Header("Enemy")]
    [SerializeField] float experienceOnDeath = 64;
    [SerializeField] float catchRate = 45;

    [Header("Evolution")]
    [SerializeField] int evolutionLevel = 30;
    [SerializeField] PokemonData pokemonEvolution = default;

    //important
    public string PokemonName => pokemonName;
    public EType PokemonType => pokemonType;
    public ESpeedExperience SpeedExperience => speedExperience;

    //for the view
    public Sprite PokemonFront => pokemonFront;
    public Sprite PokemonBack => pokemonBack;

    //attack
    public float Health => health;
    public float Speed => speed;
    public float PhysicsAttack => physicsAttack;
    public float PhysicsDefense => physicsDefense;
    public float SpecialAttack => specialAttack;
    public float SpecialDefense => specialDefense;
    public SPokemonSkill[] PossibleSkills => possibleSkills;

    //enemy
    public float ExperienceOnDeath => experienceOnDeath;
    public float CatchRate => catchRate;

    //evolution
    public int EvolutionLevel => evolutionLevel;
    public PokemonData PokemonEvolution => pokemonEvolution;
}
