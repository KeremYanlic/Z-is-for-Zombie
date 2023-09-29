using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueable
{
    DialogueSO GetDialogueSO();

    Vector3 GetDialoguePosition();

    AIDialogueConfigurationSO GetDialogueConfigurationSO();
}
