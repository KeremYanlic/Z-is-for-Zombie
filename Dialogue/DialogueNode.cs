using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
public class DialogueNode : ScriptableObject
{
    [SerializeField] private string uniqueID;
    [SerializeField] private string text;
    [SerializeField] private List<string> children = new List<string>();
    [SerializeField] private Rect rect = new Rect(0, 0, 200, 100);

    public string GetText()
    {
        return text;
    }

    public Rect GetRect()
    {
        return rect;
    }
    public string GetUniqueID()
    {
        return uniqueID;
    }
    public List<string> GetChildren()
    {
        return children;
    }

#if UNITY_EDITOR

    public void SetPosition(Vector2 newPosition)
    {
        //Undo.RecordObject(this, "Move Dialogue Node");
        rect.position = newPosition;
        EditorUtility.SetDirty(this);
    }

    public void AddPosition(Vector2 newPosition)
    {
        Undo.RecordObject(this, "Move Dialogue Node");
        rect.position += newPosition;
        EditorUtility.SetDirty(this);
    }
    public void SetText(string newText)
    {
        Undo.RecordObject(this, "Set Dialogue Node Text");
        text = newText;
        EditorUtility.SetDirty(this);
    }
    public void SetUniqueID(string newID)
    {
        uniqueID = newID;
        EditorUtility.SetDirty(this);
    }
    public void AddChild(string childID)
    {
        Undo.RecordObject(this, "Add Dialogue Link");
        children.Add(childID);
        EditorUtility.SetDirty(this);
    }
    public void RemoveChild(string childID)
    {
        Undo.RecordObject(this, "Remove Dialogue Link");
        children.Remove(childID);
        EditorUtility.SetDirty(this);
    }
#endif
}



