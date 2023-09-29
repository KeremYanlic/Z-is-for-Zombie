using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIConversant : MonoBehaviour,IDialogueable
{
    [SerializeField] private DialogueSO dialogueSO;
    [SerializeField] private AIDialogueConfigurationSO aIDialogueConfigurationSO;
    [SerializeField] private Transform playerTargetPosition;
    public DialogueSO GetDialogueSO()
    {
        return dialogueSO;
    }
    public AIDialogueConfigurationSO GetDialogueConfigurationSO()
    {
        return aIDialogueConfigurationSO;
    }

    public Vector3 GetDialoguePosition()
    {
        return playerTargetPosition.position;
    }

   
}
