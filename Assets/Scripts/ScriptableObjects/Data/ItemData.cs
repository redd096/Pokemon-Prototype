using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "PokemonPrototype/Item")]
public class ItemData : ScriptableObject, IGetButtonName
{
    [Header("Important")]
    [SerializeField] string itemName = "Life Potion";

    [Header("Effect")]
    [SerializeField] EEffect effect = EEffect.life;
    [SerializeField] float value = 30;
    [SerializeField] int duration = 0;

    //important
    public string ItemName => itemName;

    //effect
    public EEffect Effect => effect;
    public float Value => value;
    public int Duration => duration;

    public string GetButtonName()
    {
        return ItemName;
    }
}
