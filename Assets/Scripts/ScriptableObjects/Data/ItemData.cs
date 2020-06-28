using UnityEngine;

public enum EPokeBall
{
    nothing,
    masterBall,
}

[CreateAssetMenu(fileName = "Item", menuName = "Pokemon Prototype/Item")]
public class ItemData : ScriptableObject
{
    [Header("Important")]
    [SerializeField] string itemName = "Item";
    [SerializeField] EPokeBall isPokeball = default;

    [Header("Effect")]
    [SerializeField] EffectData effect = default;

    //important
    public string ItemName => itemName;
    public EPokeBall IsPokeball => isPokeball;

    //effect
    public EffectData Effect => effect;
}
