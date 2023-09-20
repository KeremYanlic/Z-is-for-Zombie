using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace Survival.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private DialogueSO selectedDialogue = null;
        [NonSerialized] private GUIStyle nodeStyle;
        [NonSerialized] private DialogueNode draggingNode = null;
        [NonSerialized] private DialogueNode creatingNode = null;
        [NonSerialized] private DialogueNode deletingNode = null;
        [NonSerialized] private DialogueNode linkinkParentNode = null;

        private const float gridLarge = 100f;
        private const float gridSmall = 25f;

        private Vector2 graphOffset;
        private Vector2 graphDrag;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            EditorWindow editorWindow = GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");

        }
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            DialogueSO dialogueSO = EditorUtility.InstanceIDToObject(instanceID) as DialogueSO;
            if (dialogueSO != null)
            {
                ShowEditorWindow();

                return true;
            }
            return false;
        }
        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChange;

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

        }
        private void OnSelectionChange()
        {
            DialogueSO newDialogue = Selection.activeObject as DialogueSO;
            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();

            }
        }

        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue Selected.");
            }
            else
            {
                DrawBackgroundGrid(gridSmall, 0.2f, Color.gray);
                DrawBackgroundGrid(gridLarge, 0.3f, Color.gray);

                //Process events like mouse dragging or up
                ProcessEvents();

                //Draw all the nodes 
                DrawNodeEvent();

                //Draw connections between nodes
                DrawConnectionsEvent();

                //Add a new dialogue
                AddDialogueNode();

                //Remove a dialogue
                DeleteDialogueNode();

            }
        }
        private void ProcessEvents()
        {
            // Reset graph drag
            graphDrag = Vector2.zero;

            switch (Event.current.type)
            {
                //Mouse down event
                case EventType.MouseDown:
                    MouseDownEvent();
                    break;

                //Mouse drag event
                case EventType.MouseDrag:
                    MouseDragEvent();
                    break;

                //Mouse up event
                case EventType.MouseUp:
                    MouseUpEvent();
                    break;
            }
        }
        //<summary>
        //Mouse Down event
        //</summary>
        private void MouseDownEvent()
        {
            if (draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (draggingNode != null)
                {
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    Selection.activeObject = selectedDialogue;
                }
            }
        }
        //<summary>
        //Mouse Drag event
        //</summary>
        private void MouseDragEvent()
        {
            if (draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
                draggingNode.AddPosition(Event.current.delta);
                GUI.changed = true;
            }
            else
            {
                graphDrag = Event.current.delta;
                selectedDialogue.DragNode(graphDrag);
                GUI.changed = true;
            }
        }
        //<summary>
        //Mouse Up event
        //</summary>
        private void MouseUpEvent()
        {
            if (draggingNode != null)
            {
                draggingNode = null;

            }
        }

        //<summary>
        //Add a new dialogue node
        //</summary>
        private void AddDialogueNode()
        {
            if (creatingNode != null)
            {
                selectedDialogue.CreateNode(creatingNode);
                creatingNode = null;
            }
        }
        //<summary>
        //Delete an existing dialogue node
        //</summary>
        private void DeleteDialogueNode()
        {
            if (deletingNode != null && deletingNode != selectedDialogue.GetRootNode())
            {
                selectedDialogue.DeleteNode(deletingNode);
                deletingNode = null;
            }
        }

        //<summary>
        //Draw node event
        //</summary>
        private void DrawNodeEvent()
        {
            foreach (DialogueNode dialogueNode in selectedDialogue.GetAllNodes())
            {
                DrawNode(dialogueNode);

            }
        }
        //<summary>
        //Draw node processes
        //</summary>
        private void DrawNode(DialogueNode dialogueNode)
        {
            GUILayout.BeginArea(dialogueNode.GetRect(), nodeStyle);
            EditorGUI.BeginChangeCheck();
            string newText = EditorGUILayout.TextField(dialogueNode.GetText());

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(dialogueNode, "Node Text");
                dialogueNode.SetText(newText);
            }
            EditorGUILayout.BeginHorizontal();

            //Add a new node button functionality
            AddNodeBtn(dialogueNode);
            //Link a node to an existing node functionality
            LinkNodeOperationBtn(dialogueNode);
            //Remove a node functionality
            RemoveNodeBtn(dialogueNode);

            EditorGUILayout.EndHorizontal();

            GUILayout.EndArea();
        }



        private void AddNodeBtn(DialogueNode dialogueNode)
        {
            if (GUILayout.Button("+"))
            {
                creatingNode = dialogueNode;
            }
        }
        private void LinkNodeOperationBtn(DialogueNode dialogueNode)
        {
            if (linkinkParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkinkParentNode = dialogueNode;
                }
            }
            else if (linkinkParentNode == dialogueNode)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkinkParentNode = null;
                }
            }
            else if (linkinkParentNode.GetChildren().Contains(dialogueNode.GetUniqueID()))
            {
                if (GUILayout.Button("Unlink"))
                {

                    linkinkParentNode.RemoveChild(dialogueNode.GetUniqueID());
                    linkinkParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    if (!linkinkParentNode.GetChildren().Contains(dialogueNode.GetUniqueID()))
                    {
                        linkinkParentNode.AddChild(dialogueNode.GetUniqueID());
                        linkinkParentNode = null;
                    }

                }
            }
        }

        private void RemoveNodeBtn(DialogueNode dialogueNode)
        {
            if (GUILayout.Button("-"))
            {
                deletingNode = dialogueNode;
            }
        }
        //<summary>
        //Draw connections events
        //</summmary>
        private void DrawConnectionsEvent()
        {
            foreach (DialogueNode dialogueNode in selectedDialogue.GetAllNodes())
            {
                DrawConnections(dialogueNode);
            }
        }

        //<summary>
        //Draw connections between nodes
        //</summmary>
        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector3(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                if (childNode != null)
                {
                    Vector3 endPosition = new Vector3(childNode.GetRect().xMin, childNode.GetRect().center.y);
                    Vector3 controlPointOffset = endPosition - startPosition;
                    controlPointOffset.y = 0;
                    controlPointOffset.x *= 0.8f;
                    Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset, endPosition - controlPointOffset, Color.white, null, 3f);
                }


            }
        }
        private DialogueNode GetNodeAtPoint(Vector2 mousePosition)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(mousePosition))
                {
                    foundNode = node;
                }


            }
            return foundNode;
        }
        private void DrawBackgroundGrid(float gridSize, float gridOpacity, Color gridColor)
        {
            int verticalLineCount = Mathf.CeilToInt((position.width + gridSize) / gridSize);
            int horizontalLineCount = Mathf.CeilToInt((position.height + gridSize) / gridSize);

            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            graphOffset += graphDrag * 0.5f;

            Vector3 gridOffset = new Vector3(graphOffset.x % gridSize, graphOffset.y % gridSize, 0);

            for (int i = 0; i < verticalLineCount; i++)
            {
                Handles.DrawLine(
                    new Vector3(gridSize * i, -gridSize, 0) + gridOffset,
                    new Vector3(gridSize * i, position.height + gridSize, 0f) + gridOffset);
            }

            for (int j = 0; j < horizontalLineCount; j++)
            {
                Handles.DrawLine(
                    new Vector3(-gridSize, gridSize * j, 0) + gridOffset,
                    new Vector3(position.width + gridSize, gridSize * j, 0f) + gridOffset);
            }

            Handles.color = Color.white;
        }
    }


}
