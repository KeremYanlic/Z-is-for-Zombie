using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Car : MonoBehaviour
{
    public CarDetailsSO carDetailsSO;


    #region Tooltip
    [Tooltip("Populate with the transform that the player spawn after the player leaves the car")]
    [SerializeField] private Transform playerSpawnPosition;
    #endregion

    public bool isCarActive = false;
    public float gasAmount;

    private void Start()
    {
        //Set start gas amount
        gasAmount = carDetailsSO.startGasAmount;
    }
    public Vector3 GetPlayerSpawnPosition()
    {
        return playerSpawnPosition.position;
    }


}
