using UnityEngine;

[CreateAssetMenu(fileName = "Ustionato", menuName = "Pokemon Prototype/Effects/Ustionato")]
public class EffectUstionato : EffectData
{
    [Header("Ustionato")]
    [Min(0)] [SerializeField] int value = 10;

    public override void ApplyEffect(PokemonModel pokemon)
    {
        pokemon.CurrentHealth -= value;
    }

    public override void RemoveEffect(PokemonModel pokemon) {}
}
