using UnityEngine;

[CreateAssetMenu(fileName = "PP", menuName = "Pokemon Prototype/Effects/PP")]
public class EffectPP : EffectData
{
    [Header("Ricarica PP")]
    [Min(0)] [SerializeField] int value = 10;

    public override void ApplyEffect(PokemonModel pokemon)
    {
        //for every skill, add PP
        foreach(SkillModel skill in pokemon.CurrentSkills)
        {
            if(skill != null)
                skill.CurrentPP += value;
        }
    }

    public override void RemoveEffect(PokemonModel pokemon) {}
}
