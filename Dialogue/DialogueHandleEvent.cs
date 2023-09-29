using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DialogueHandleEvent : MonoBehaviour
{
    public event Action<DialogueHandleEvent> OnStartDialogue;
    public event Action<DialogueHandleEvent> OnQuitDialogue;

    public void CallStartDialogueEvent()
    {
        OnStartDialogue?.Invoke(this);
    }
    public void CallStopDialogueEvent()
    {
        OnQuitDialogue?.Invoke(this);
    }
}
