using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarDetailsSO_", menuName = "Scriptable Objects/Car/CarDetailsSO")]
public class CarDetailsSO : ScriptableObject
{
    #region Header CAR BASE DETAILS
    [Space(10)]
    [Header("CAR BASE DETAILS")]
    #endregion

    #region Tooltip
    [Tooltip("Name of the car")]
    #endregion
    public string carName;

    #region Header FEATURES
    [Space(10)]
    [Header("FEATURES")]
    #endregion
    #region Tooltip
    [Tooltip("The max speed of the car")]
    #endregion
    public float maxSpeed = 50f;

    #region Tooltip
    [Tooltip("Start gas amount")]
    #endregion
    public float startGasAmount;

    #region Tooltip
    [Tooltip("Start car durability")]
    #endregion
    public int startCarDurability;

    #region Header OPERATING VALUES
    [Space(10)]
    [Header("OPERATING VALUES")]
    #endregion
    #region Tooltip
    [Tooltip("The acceleration speed of the car")]
    #endregion
    public float accelerationFactor = 30f;

    #region Tooltip
    [Tooltip("The turn factor of the car")]
    #endregion
    public float turnFactor = 3.5f;

    #region Tooltip
    [Tooltip("The drift factor of the car")]
    #endregion
    public float driftFactor = 0.95f;

    #region Tooltip
    [Tooltip("This variable is for more smooth transition when the car is steering.The more the number is the smoothly faster the car steer")]
    #endregion
    public float rotationSpeed = 50f;



}
