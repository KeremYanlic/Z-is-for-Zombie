using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FieldDetails_", menuName = "Scriptable Objects/Field/FieldSO")]
public class FieldDetailsSO : ScriptableObject
{

    #region Header BASE FIELD DETAILS
    [Space(10)]
    [Header("BASE FIELD DETAILS")]
    #endregion

    #region Tooltip
    [Tooltip("The name of the field")]
    #endregion
    public string fieldName;

    #region Tooltip
    [Tooltip("Is this area enemy field ?")]
    #endregion
    public bool isEnemyArea = false;
}
