using UnityEngine;

public abstract class EffectData : ScriptableObject
{
    [Header("Base Effect")]
    [Min(1)] [SerializeField] int duration = 1;
    [SerializeField] string descriptionOnPlayer = string.Empty;
    [SerializeField] string descriptionOnEnemy = string.Empty;

    [Header("Remove Effect")]
    [SerializeField] string descriptionRemoveFromPlayer = string.Empty;
    [SerializeField] string descriptionRemoveFromEnemy = string.Empty;

    public int Duration => duration;
    public string DescriptionOnPlayer => descriptionOnPlayer;
    public string DescriptionOnEnemy => descriptionOnEnemy;
    public string DescriptionRemoveFromPlayer => descriptionRemoveFromPlayer;
    public string DescriptionRemoveFromEnemy => descriptionRemoveFromEnemy;


    /// <summary>
    /// Apply effect (for example do damage) to a pokemon
    /// </summary>
    public abstract void ApplyEffect(PokemonModel pokemon);

    /// <summary>
    /// Remove effect (for example reset accuracy) to a pokemon
    /// </summary>
    public abstract void RemoveEffect(PokemonModel pokemon);
}
