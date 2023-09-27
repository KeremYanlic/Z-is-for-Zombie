using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class PlayerConversant : MonoBehaviour
{

    private DialogueSO currentDialogueSO;
    private DialogueNode currentNode = null;
    public event Action<PlayerConversant> OnDialogueUpdated; 
   
    public void StartDialogue(DialogueSO dialogueToStart)
    {
        DialogueUI.Instance.Show();

        currentDialogueSO = dialogueToStart;
        currentNode = currentDialogueSO.GetRootNode();
        OnDialogueUpdated?.Invoke(this);
    }
    public string GetText()
    {
        if(currentNode == null)
        {
            return "";
        }
        return currentNode.GetText();
    }
    public IEnumerable<DialogueNode> GetChoices()
    {
       
        foreach(DialogueNode node in currentDialogueSO.GetAllChildren(currentNode))
        {
            yield return node;
        }
       

    } 
    public void SelectChoice(DialogueNode chosenNode)
    {
        currentNode = currentDialogueSO.GetChildren(chosenNode);
        OnDialogueUpdated?.Invoke(this);
       
    }
    public void QuitDialogue()
    {
        currentDialogueSO = null;
        currentNode = null;
        OnDialogueUpdated?.Invoke(this);
    }
    //public void Next()
    //{
    //    DialogueNode[] children = currentDialogueSO.GetAllChildren(currentNode).ToArray();
    //    DialogueNode speakerText = currentDialogueSO.GetChildren(children[0]);
    //    currentNode = speakerText;
    //}
    public bool HasNext(DialogueNode dialogueNodeToCheck)
    {
        DialogueNode[] children = currentDialogueSO.GetAllChildren(dialogueNodeToCheck).ToArray();
        return children.Count() > 0;
    }

    public bool IsActive()
    {
        return currentDialogueSO != null;
    }
}
