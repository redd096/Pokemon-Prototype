using UnityEngine;

[CreateAssetMenu(fileName = "Accuracy", menuName = "Pokemon Prototype/Effects/Accuracy")]
public class EffectAccuracy : EffectData
{
    [Header("Set Accuracy")]
    [Min(0)] [SerializeField] int value = 30;

    public override void ApplyEffect(PokemonModel pokemon)
    {
        pokemon.CurrentMaxAccuracy = value;
    }

    public override void RemoveEffect(PokemonModel pokemon)
    {
        pokemon.CurrentMaxAccuracy = 100;
    }
}
