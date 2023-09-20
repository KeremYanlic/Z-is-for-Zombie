using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "Dialogue_", menuName = "New Dialogue", order = 0)]
public class DialogueSO : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private List<DialogueNode> nodes = new();

    private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
    private void Awake()
    {
        if (nodes.Count == 0)
        {
            DialogueNode dialogueNode = new DialogueNode();
            dialogueNode.SetUniqueID(System.Guid.NewGuid().ToString());
            nodes.Add(dialogueNode);
        }
    }
#endif
    private void OnValidate()
    {
        nodeLookup.Clear();

        foreach (DialogueNode node in GetAllNodes())
        {

            nodeLookup[node.GetUniqueID()] = node;
        }
    }


    public IEnumerable<DialogueNode> GetAllNodes()
    {
        return nodes;
    }
    public DialogueNode GetRootNode()
    {
        if (nodes[0] != null)
        {
            return nodes[0];
        }
        return null;

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

    public void CreateNode(DialogueNode parentNode)
    {
        //Create new node
        DialogueNode newNode = CreateInstance<DialogueNode>();
        string uniqueID = System.Guid.NewGuid().ToString();
        newNode.SetUniqueID(uniqueID);//Set new node's uniqueID
        newNode.name = uniqueID; //Set new node's name
        float xPositionModifier = 300;
        float yPositionModifier = (2 - parentNode.GetChildren().Count) * 150;
        Vector2 newNodePosition = new Vector2(parentNode.GetRect().position.x + xPositionModifier, parentNode.GetRect().position.y + yPositionModifier);
        newNode.SetPosition(newNodePosition);

        if (parentNode != null)
        {
            parentNode.AddChild(newNode.GetUniqueID());

        }
        Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");

        Undo.RecordObject(this, "Added Dialogue Node");
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
    private void CleanDanglingChildren(DialogueNode nodeToDelete)
    {
        foreach (DialogueNode node in GetAllNodes())
        {
            node.RemoveChild(nodeToDelete.GetUniqueID());
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
    }

    public void OnAfterDeserialize()
    {
    }
}


