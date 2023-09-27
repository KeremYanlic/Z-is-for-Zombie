using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "Dialogue_", menuName = "New Dialogue", order = 0)]
public class DialogueSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();

    private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        nodeLookup.Clear();

        foreach (DialogueNode node in GetAllNodes())
        {

            nodeLookup[node.name] = node;
        }
    }

#endif


    public IEnumerable<DialogueNode> GetAllNodes()
    {
        return nodes;
    }
    public DialogueNode GetRootNode()
    {
        return nodes[0];

    }
    public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
    {
        foreach (string childID in parentNode.GetChildren())
        {
            if (nodeLookup.ContainsKey(childID))
            {
                yield return nodeLookup[childID];
            }
        }

    }
    public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
    {
        foreach(DialogueNode node in GetAllChildren(currentNode))
        {
            if (node.IsPlayerSpeaking())
            {
                yield return node;
            }
        }
    }
    public DialogueNode GetChildren(DialogueNode parentNode)
    {
        foreach (string childID in parentNode.GetChildren())
        {
            if (nodeLookup.ContainsKey(childID))
            {
                return nodeLookup[childID];
            }
        }
        return null;
    }
   

    public void CreateNode(DialogueNode parentNode)
    {
        //Create new node
        DialogueNode newNode = MakeNode(parentNode);

        float xPositionModifier = 300;
        float yPositionModifier = (2 - parentNode.GetChildren().Count) * 150;
        Vector2 newNodePosition = new Vector2(parentNode.GetRect().position.x + xPositionModifier, parentNode.GetRect().position.y + yPositionModifier);
        newNode.SetPosition(newNodePosition);

        Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
        Undo.RecordObject(this, "Added Dialogue Node");

        AddNode(newNode); 

    }
    private void AddNode(DialogueNode newNode)
    {
        nodes.Add(newNode);
        OnValidate();
    }
    public void DeleteNode(DialogueNode nodeToDelete)
    {
        Undo.RecordObject(this, "Deleted Dialogue Node");
        nodes.Remove(nodeToDelete);
        OnValidate();
        CleanDanglingChildren(nodeToDelete);
        Undo.DestroyObjectImmediate(nodeToDelete);

    }
    private DialogueNode MakeNode(DialogueNode parent)
    {
        DialogueNode newNode = CreateInstance<DialogueNode>();

        newNode.name = Guid.NewGuid().ToString();
        if(parent != null)
        {
            parent.AddChild(newNode.name);
            newNode.SetPlayerSpeaking(parent.IsPlayerSpeaking());

        }
        return newNode;
    }
    private void CleanDanglingChildren(DialogueNode nodeToDelete)
    {
        foreach (DialogueNode node in GetAllNodes())
        {
            node.RemoveChild(nodeToDelete.name);
        }
    }

    public void DragNode(Vector2 delta)
    {
        foreach (DialogueNode dialogueNode in GetAllNodes())
        {
            dialogueNode.AddPosition(delta); 
            EditorUtility.SetDirty(this);

        }
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (nodes.Count == 0)
        {
            DialogueNode newNode = MakeNode(null);
            AddNode(newNode);
        }

        if (AssetDatabase.GetAssetPath(this) != "")
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                if (AssetDatabase.GetAssetPath(node) == "")
                {
                    AssetDatabase.AddObjectToAsset(node, this);
                }
            }
        }
#endif
    }
    public void OnAfterDeserialize()
    {
    }
}


