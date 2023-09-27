using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractManager : MonoBehaviour
{
    [SerializeField] private LayerMask dialogueInteractableLayer;
    private PlayerConversant playerConversant;

    private void Awake()
    {
        playerConversant = GetComponent<PlayerConversant>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D ray = Physics2D.Raycast(UtilsClass.GetWorldMousePosition(), Vector2.zero,Mathf.Infinity,dialogueInteractableLayer);
            if(ray.collider != null)
            {
                //Start dialogue with ai
                AIConversant aIConversant = ray.collider.gameObject.GetComponent<AIConversant>();
                StartDialogue(aIConversant.dialogueSO);
            }
        }
    }

    private void StartDialogue(DialogueSO dialogueToStart)
    {
        playerConversant.StartDialogue(dialogueToStart);
    }

}
