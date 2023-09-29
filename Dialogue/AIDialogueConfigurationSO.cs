using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIConfigurationSO_",menuName = "Scriptable Objects/Dialogue/AIConfigurationSO")]
public class AIDialogueConfigurationSO : ScriptableObject
{
    #region Header BASE DETAILS

  
    #region Tooltip
    [Tooltip("The slowdown distance when moving toward to an ai")]
    #endregion
    public float slowdownDistance = 0.6f;

    #region Tooltip
    [Tooltip("The end reached distance when moving toward to an ai")]
    #endregion
    public float endReachDistance = 0.2f;
    #endregion
}
