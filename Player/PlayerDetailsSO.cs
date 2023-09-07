using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetailsSO_", menuName = "Scriptable Objects/Player/PlayerDetails")]
public class PlayerDetailsSO : ScriptableObject
{
    #region Header
    [Header("PLAYER DETAILS")]
    [Space(10)]
    #endregion

    #region Header BASE DETAILS
    [Header("BASE DETAILS")]
    #endregion Header BASE DETAILS

    #region Tooltip
    [Tooltip("The name of the player")]
    #endregion
    public string playerName;

    #region Tooltip
    [Tooltip("The minimum starting health of the player. Starting health is the random value between minStartingHealth and maxStartingHealth")]
    #endregion
    public int minStartingHealth = 100;

    #region Tooltip
    [Tooltip("The maximum starting health of the player. Starting health is the random value between minStartingHealth and maxStartingHealth")]
    #endregion
    public int maxStartingHealth = 100;

    #region Tooltip
    [Tooltip("The minimum starting stamina amount of the player.Stamina amount is the random value between minStartingStamina and maxStartingStamina")]
    #endregion
    public int minStartingStamina = 100;

    #region Tooltip
    [Tooltip("The maximum starting stamina amount of the player.Stamina amount is the random value between minStartingStamina and maxStartingStamina")]
    #endregion
    public int maxStartingStamina = 100;

    public GameObject playerPrefab;

    #region Tooltip
    [Tooltip("Starting Weapon")]
    #endregion
    public WeaponDetailsSO startingWeapon;

    #region Tooltip
    [Tooltip("Starting weapon array")]
    #endregion
    [SerializeField] private List<WeaponDetailsSO> _startingWeaponList;


    public List<WeaponDetailsSO> StartingWeaponList => _startingWeaponList;

    public int GetStartingHealth()
    {
        if (minStartingHealth == maxStartingHealth)
        {
            return minStartingHealth;
        }
        else
        {
            return Random.Range(minStartingHealth, maxStartingHealth);
        }
    }
    public int GetStartingStamina()
    {
        if (minStartingStamina == maxStartingStamina)
        {
            return minStartingStamina;
        }
        else
        {
            return Random.Range(minStartingStamina, maxStartingStamina);
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        UtilsClass.ValidateCheckPositiveValue(this, nameof(minStartingHealth), minStartingHealth, false);
        UtilsClass.ValidateCheckPositiveValue(this, nameof(maxStartingHealth), maxStartingHealth, false);
        UtilsClass.ValidateCheckPositiveRange(this, nameof(minStartingHealth), minStartingHealth, nameof(maxStartingHealth), maxStartingHealth);
    }
#endif
    #endregion
}
