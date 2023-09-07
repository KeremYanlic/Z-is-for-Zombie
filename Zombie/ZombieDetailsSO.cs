using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieDetails_", menuName = "Scriptable Objects/Zombie/ZombieDetailsSO")]
public class ZombieDetailsSO : ScriptableObject
{
    #region Header BASE ENEMY DETAILS
    [Space(10)]
    [Header("BASE ENEMY DETAILS")]
    #endregion

    #region Tooltip
    [Tooltip("The name of the enemy")]
    #endregion
    public string enemyName;

    #region Tooltip
    [Tooltip("The prefab of the enemy")]
    #endregion
    public GameObject enemyPrefab;

    #region Tooltip
    [Tooltip("The health of the enemy")]
    #endregion
    public int enemyHealth = 100;

    #region Header BASE ENEMY MOVEMENT DETAILS
    [Space(10)]
    [Header("BASE ENEMY MOVEMENT DETAILS")]
    #endregion

    #region Tooltip
    [Tooltip("The min movement speed of the enemy")]
    #endregion
    public float minMoveSpeed;

    #region Tooltip
    [Tooltip("The max movement speed of the enemy")]
    #endregion
    public float maxMoveSpeed;

    #region Header BASE ENEMY REACTION DETAILS
    [Space(10)]
    [Header("BASE ENEMY REACTION DETAILS")]
    #endregion

    #region Tooltip
    [Tooltip("The target check distance")]
    #endregion
    public float targetCheckDistance;

    #region Tooltip
    [Tooltip("The target attack distance")]
    #endregion
    public float targetAttackDistance;

    #region Tooltip
    [Tooltip("The target chase distance")]
    #endregion
    public float targetChaseDistance;

    #region Tooltip
    [Tooltip("The duration zombie stay as idle after losing track of player")]
    #endregion
    public float idleDuration = 3f;


    #region Tooltip
    [Tooltip("The enemy slowdown distance")]
    #endregion
    public float slowdownDistance = 0.6f;

    #region Tooltip
    [Tooltip("The enemy end reached distance")]
    #endregion
    public float endReachDistance = 0.2f;



    #region Header BASE ENEMY PATH CALCULATE DETAILS
    [Space(10)]
    [Header("BASE ENEMY PATH CALCULATE DETAILS")]
    #endregion

    #region Tooltip
    [Tooltip("The radius of the enemy.This value should be higher if the enemy's scale is high as well.")]
    #endregion
    public float radius = .5f;

    #region Tooltip
    [Tooltip("The enemy recalculate path interval. The value should be lower if the enemy's movement speed is high")]
    #endregion
    public float maxCalculatePathInterval = 2f;



    public float GetMoveSpeed()
    {
        if (minMoveSpeed == maxMoveSpeed)
            return minMoveSpeed;

        return Random.Range(minMoveSpeed, maxMoveSpeed);
    }
}
