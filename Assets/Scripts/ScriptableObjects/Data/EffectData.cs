using UnityEngine;

public abstract class EffectData : ScriptableObject
{
    [Header("Base Effect")]
    [Min(1)] [SerializeField] int duration = 1;
    [SerializeField] string descriptionOnPlayer = string.Empty;
    [SerializeField] string descriptionOnEnemy = string.Empty;

    public int Duration => duration;
    public string DescriptionOnPlayer => descriptionOnPlayer;
    public string DescriptionOnEnemy => descriptionOnEnemy;


    /// <summary>
    /// Apply effect (for example do damage) to a pokemon
    /// </summary>
    public abstract void ApplyEffect(PokemonModel pokemon);
}
