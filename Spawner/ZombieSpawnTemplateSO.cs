using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieSpawnTemplateSO_", menuName = "Scriptable Objects/Spawner/ZombieSpawnerTemplate")]
public class ZombieSpawnTemplateSO : ScriptableObject
{
    #region Header ZOMBIE TYPE SPAWN AMOUNT
    [Space(10)]
    [Header("ZOMBIE TYPE SPAWN AMOUNT")]
    #endregion

    #region Tooltip
    [Tooltip("Adult zombie spawn amount")]
    [Space(5)]
    #endregion
    public int adultZombieSpawnAmount = 10;

    public GameObject pfAdultZombie;

    #region Tooltip
    [Tooltip("Child zombie spawn amount")]
    [Space(5)]
    #endregion 
    public int childZombieSpawnAmount = 5;

    public GameObject pfChildZombie;


    #region Tooltip
    [Tooltip("Armored zombie spawn amount")]
    [Space(5)]
    #endregion
    public int armoredZombieSpawnAmount = 3;

    #region Tooltip
    [Tooltip("Giant zombie spawn amount")]
    [Space(5)]
    #endregion
    public int giantZombieSpawnAmount = 2;

    public GameObject pfGiantZombie;


    #region Tooltip
    [Tooltip("Runner zombie spawn amount")]
    [Space(5)]
    #endregion
    public int runnerZombieSpawnAmount = 2;

    #region Tooltip
    [Tooltip("Witch zombie spawn amount")]
    [Space(5)]
    #endregion
    public int witchZombieSpawnAmount = 2;

    public GameObject pfWitchZombie;

    #region Tooltip
    [Tooltip("Vomit zombie spawn amount")]
    [Space(5)]
    #endregion
    public int vomitZombieSpawnAmount = 2;
}
