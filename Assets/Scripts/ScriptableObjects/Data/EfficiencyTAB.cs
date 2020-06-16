using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FloatArray
{
    public float[] SkillArray;
}

[CreateAssetMenu(fileName = "Efficiency TAB", menuName = "Pokemon Prototype/Efficiency TAB")]
public class EfficiencyTAB : ScriptableObject
{
    [SerializeField] FloatArray[] pokemonArray = default;

    public FloatArray[] PokemonArray { get { return pokemonArray; } }
}
