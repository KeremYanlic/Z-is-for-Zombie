using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetailsSO_", menuName = "Scriptable Objects/Player/Movement Details")]
public class MovementDetailSO : ScriptableObject
{
    #region Header MOVEMENT DETAILS
    [Header("MOVEMENT DETAILS")]
    [Space(10)]
    #endregion Header MOVEMENT DETAILS

    #region Header WALK SPEED
    [Header("WALK SPEED")]
    [Space(10)]
    #endregion Header WALK SPEED

    #region Tooltip
    [Tooltip("The minimum movement speed of the player")]
    #endregion
    public float moveSpeedMin = 10f;
    #region Tooltip
    [Tooltip("The maximum movement speed of the player")]
    #endregion
    public float moveSpeedMax = 10f;

    #region Header RUN SPEED 
    [Header("RUN SPEED")]
    [Space(10)]
    #endregion Header RUN SPEED

    #region Tooltip
    [Tooltip("The minimum run speed of the player")]
    #endregion
    public float runSpeedMin = 10f;
    #region Tooltip
    [Tooltip("The maximum run speed of the player")]
    #endregion
    public float runSpeedMax = 10f;

    #region Header ROLL ADJUSTMENTS
    [Header("ROLL ADJUSTMENTS")]
    [Space(10)]
    #endregion Header ROLL ADJUSTMENTS
    #region Tooltip
    [Tooltip("The minimum roll speed of the player")]
    #endregion
    public float rollSpeedMin = 10f;
    #region Tooltip
    [Tooltip("The maximum roll speed of the player")]
    #endregion
    public float rollSpeedMax = 10f;

    #region Tooltip
    [Tooltip("The roll distance")]
    #endregion
    public float rollDistance = 3f;

    #region Tooltip
    [Tooltip("The roll cooldown timer")]
    #endregion
    public float rollCooldownTimer = 1f;

    // <summary>
    // Get a random move speed according to both moveSpeedMin and moveSpeedMax
    // </summary>
    public float GetMoveSpeed()
    {
        if (moveSpeedMin == moveSpeedMax)
        {
            return moveSpeedMin;
        }
        else
        {
            return Random.Range(moveSpeedMin, moveSpeedMax);
        }
    }


    // <summary>
    // Get a random run speed according to both runSpeedMin and runSpeedMax
    // </summary>
    public float GetRunSpeed()
    {
        if (runSpeedMin == runSpeedMax)
        {
            return runSpeedMin;
        }
        else
        {
            return Random.Range(runSpeedMin, runSpeedMax);
        }
    }

    // <summary>
    // Get a random roll speed according to both moveSpeedMin and moveSpeedMax
    // </summary>
    public float GetRollSpeed()
    {
        if (rollSpeedMin == rollSpeedMax)
        {
            return rollSpeedMin;
        }
        else
        {
            return Random.Range(rollSpeedMin, rollSpeedMax);
        }
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        
        UtilsClass.ValidateCheckPositiveValue(this, nameof(moveSpeedMin), moveSpeedMin, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(moveSpeedMax), moveSpeedMax, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(rollSpeedMin), rollSpeedMin, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(rollSpeedMax), rollSpeedMax, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(rollDistance), rollDistance, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(runSpeedMin), runSpeedMin,false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(runSpeedMax), runSpeedMax, false);
        UtilsClass.ValidateCheckPositiveRange(this, nameof(moveSpeedMin), moveSpeedMin, nameof(moveSpeedMax), moveSpeedMax);
        UtilsClass.ValidateCheckPositiveRange(this, nameof(runSpeedMin), runSpeedMin, nameof(runSpeedMax), runSpeedMax);
        UtilsClass.ValidateCheckPositiveRange(this, nameof(rollSpeedMin), rollSpeedMin, nameof(rollSpeedMax), rollSpeedMax);
       

    }
#endif
    #endregion
}
