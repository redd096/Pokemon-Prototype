using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    [Header("Arena")]
    [SerializeField] Transform player;
    [SerializeField] Transform enemy;

    [Header("InfoBox")]
    [SerializeField] GameObject description;
    [SerializeField] GameObject playerMenu;
}
