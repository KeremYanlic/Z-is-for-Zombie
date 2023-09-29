using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerConversant : MonoBehaviour
{
    public event Action<PlayerConversant> OnDialogueUpdated;

    private Player player;
    
    private DialogueSO currentDialogueSO;
    private DialogueNode currentNode = null;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void OnEnable()
    {
        //Subscribe to dialogue start event
        player.dialogueHandleEvent.OnStartDialogue += DialogueHandleEvent_OnStartDialogue;
        //Subscribe to dialogue quit event
        player.dialogueHandleEvent.OnQuitDialogue += DialogueHandleEvent_OnQuitDialogue;
    }
    private void OnDisable()
    {
        //Unsubscribe from dialogue start event
        player.dialogueHandleEvent.OnStartDialogue += DialogueHandleEvent_OnStartDialogue;
        //Unsubscribe from dialogue quit event
        player.dialogueHandleEvent.OnQuitDialogue += DialogueHandleEvent_OnQuitDialogue;
    }

  
    //<summary>
    //Start dialogue when interact with an ai.
    //</summary>
    public void StartDialogue(DialogueSO dialogueToStart)
    {
        //Open dialogue menu
        DialogueUI.Instance.Show();

        //Set current dialogueSO
        currentDialogueSO = dialogueToStart;

        //Set current node to start to dialogue
        currentNode = currentDialogueSO.GetRootNode();

        //Update the dialogue.
        OnDialogueUpdated?.Invoke(this);

        //Call start dialogue event
        player.dialogueHandleEvent.CallStartDialogueEvent();
    }
    //<summary>
    //Selecting choice function that runs when select an answer choice.
    //</summary>
    public void SelectChoice(DialogueNode chosenNode)
    {
        //Update current node in order to forward the dialogue.
        currentNode = currentDialogueSO.GetChildren(chosenNode);
        OnDialogueUpdated?.Invoke(this);

    }
    //<summary>
    //Quit the dialogue.Reset all the dialogue variables for a fresh dialogue.
    //</summary>
    public void QuitDialogue()
    {
        //Reset currentDialogueSO
        currentDialogueSO = null;
        //Reset current node
        currentNode = null;
        //Update the dialogue.
        OnDialogueUpdated?.Invoke(this);

        //Quit dialogue
        player.dialogueHandleEvent.CallStopDialogueEvent();
    }

    private void DialogueHandleEvent_OnStartDialogue(DialogueHandleEvent obj)
    {
        player.playerController.DisableMovement();
    }
    private void DialogueHandleEvent_OnQuitDialogue(DialogueHandleEvent obj)
    {
        player.playerController.EnablePlayer();
    }


    //<summary>
    //Return all the predefined answers.
    //</summary>
    public IEnumerable<DialogueNode> GetChoices()
    {
        foreach (DialogueNode node in currentDialogueSO.GetAllChildren(currentNode))
        {
            yield return node;
        }
    }
    //<summary>
    //Check if the answer you select has a reply from ai
    //</summary>
    public bool HasNext(DialogueNode dialogueNodeToCheck)
    {
        DialogueNode[] children = currentDialogueSO.GetAllChildren(dialogueNodeToCheck).ToArray();
        return children.Count() > 0;
    }
    //<summary>
    //Get current node text.
    //</summary>
    public string GetText()
    {
        if (currentNode == null)
        {
            return "";
        }
        return currentNode.GetText();
    }

    //<summary>
    //Check if dialogue is active.
    //</summary>
    public bool IsActive()
    {
        return currentDialogueSO != null;
    }
}
