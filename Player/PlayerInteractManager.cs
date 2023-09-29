using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerConversant))]
[RequireComponent(typeof(DialogueHandleEvent))]
[DisallowMultipleComponent]
public class PlayerInteractManager : MonoBehaviour
{
    [SerializeField] private LayerMask dialogueInteractableLayer;

    private PlayerConversant playerConversant;
    private DialogueHandleEvent dialogueHandleEvent;
    private MoveToDestinationEvent moveToDestinationEvent;
    public bool isHeading = false;
    public bool inDialogue = false;
    private IDialogueable currentDialogueableAI;
    private void Awake()
    {
        playerConversant = GetComponent<PlayerConversant>();
        dialogueHandleEvent = GetComponent<DialogueHandleEvent>();
        moveToDestinationEvent = GetComponent<MoveToDestinationEvent>();
    }
    private void OnEnable()
    {
        //Subscribe to dialogue handle event
        dialogueHandleEvent.OnStartDialogue += DialogueHandleEvent_OnStartDialogue;

        //Subscribe to dialogue quit event
        dialogueHandleEvent.OnQuitDialogue += DialogueHandleEvent_OnQuitDialogue;
    }
    private void OnDisable()
    {
        //Unsubscribe from dialogue handle event
        dialogueHandleEvent.OnStartDialogue -= DialogueHandleEvent_OnStartDialogue;

        //Unsubscribe from dialogue quit event
        dialogueHandleEvent.OnQuitDialogue -= DialogueHandleEvent_OnQuitDialogue;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isHeading && !inDialogue)
        {
            RaycastHit2D ray = Physics2D.Raycast(UtilsClass.GetWorldMousePosition(), Vector2.zero,Mathf.Infinity,dialogueInteractableLayer);
            if(ray.collider != null)
            {
                currentDialogueableAI = ray.collider.gameObject.GetComponent<IDialogueable>();                
                isHeading = true;  
            }
        }
        if (isHeading)
        {
            Vector2 moveDir = currentDialogueableAI.GetDialoguePosition() - transform.position;
            moveToDestinationEvent.CallMoveToDestinationEvent(transform.position, currentDialogueableAI.GetDialoguePosition(),moveDir);
          

            if (Vector3.Distance(currentDialogueableAI.GetDialoguePosition(), transform.position) < currentDialogueableAI.GetDialogueConfigurationSO().endReachDistance)
            {
                isHeading = false;

                moveToDestinationEvent.CallStopDestinationEvent();

                StartDialogue(currentDialogueableAI.GetDialogueSO());

            }
        }
    }

    private void StartDialogue(DialogueSO dialogueToStart)
    {
        playerConversant.StartDialogue(dialogueToStart);
    }
    private void DialogueHandleEvent_OnStartDialogue(DialogueHandleEvent obj)
    {
        inDialogue = true;
    }


    private void DialogueHandleEvent_OnQuitDialogue(DialogueHandleEvent obj)
    {
        inDialogue = false;
        currentDialogueableAI = null;
    }

  

}
