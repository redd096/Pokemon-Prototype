using UnityEngine;

[CreateAssetMenu(fileName = "Vita", menuName = "Pokemon Prototype/Effects/Vita")]
public class EffectVita : EffectData
{
    [Header("Ricarica Vita")]
    [Min(0)] [SerializeField] int value = 10;

    public override void ApplyEffect(PokemonModel pokemon)
    {
        pokemon.CurrentHealth += value;
    }

    public override void RemoveEffect(PokemonModel pokemon) {}
}
