using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Pokemon Prototype/Item")]
public class ItemData : ScriptableObject
{
    [Header("Important")]
    [SerializeField] string itemName = "Item";

    [Header("Effect")]
    [SerializeField] EffectData effect = default;

    //important
    public string ItemName => itemName;

    //effect
    public EffectData Effect => effect;
}
