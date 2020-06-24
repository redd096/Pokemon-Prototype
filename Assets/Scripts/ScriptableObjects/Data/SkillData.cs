using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Pokemon Prototype/Skill")]
public class SkillData : ScriptableObject
{
    [Header("Important")]
    [SerializeField] string skillName = "Skill";
    [SerializeField] EType skillType = EType.normal;
    [Min(0)] [SerializeField] int pp = 0;
    [SerializeField] bool isSpecial = false;

    [Header("Damage")]
    [SerializeField] float power = 10;
    [Range(30, 100)] [SerializeField] int accuracy = 50;

    [Header("Effect")]
    [SerializeField] EffectData effect = default;

    //important
    public string SkillName => skillName;
    public EType SkillType => skillType;
    public int PP => pp;
    public bool IsSpecial => isSpecial;

    //damage
    public float Power => power;
    public int Accuracy => accuracy;

    //effect
    public EffectData Effect => effect;
}
