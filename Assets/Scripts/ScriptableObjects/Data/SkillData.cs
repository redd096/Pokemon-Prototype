using UnityEngine;

public enum EEffect
{
    nothing,
    life,
    pp,
    precision,
    burned,
    paralyzed,
}

[CreateAssetMenu(fileName = "Skill", menuName = "PokemonPrototype/Skill")]
public class SkillData : ScriptableObject
{
    [Header("Important")]
    [SerializeField] string skillName = "Scontro";
    [SerializeField] EType skillType = EType.normal;
    [SerializeField] int pp = 0;
    [SerializeField] bool isSpecial = false;

    [Header("Damage")]
    [SerializeField] float power = 10;
    [Range(0, 100)] [SerializeField] float accuracy = 50;

    [Header("Effect")]
    [SerializeField] EEffect effect = EEffect.nothing;
    [SerializeField] float value = 0;
    [SerializeField] int duration = 0;

    //important
    public string SkillName => skillName;
    public EType SkillType => skillType;
    public int PP => pp;
    public bool IsSpecial => isSpecial;

    //damage
    public float Power => power;
    public float Accuracy => accuracy;

    //effect
    public EEffect Effect => effect;
    public float Value => value;
    public int Duration => duration;
}
