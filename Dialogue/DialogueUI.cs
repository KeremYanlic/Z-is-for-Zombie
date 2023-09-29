using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueUI : SingletonMonobehaviour<DialogueUI>
{
    private PlayerConversant playerConversant;

    
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Transform choiceRoot; 
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private Button quitBtn;


    [SerializeField] private Transform dialogueSection;
    protected override void Awake()
    {
        base.Awake();

        //Hide dialogueUI at start
        Hide();
        //Quit btn event
        quitBtn.onClick.AddListener(() =>
        {
            playerConversant.QuitDialogue();
            Hide();
        });

        
    }
  
    private void Start()
    {
        //Set player conversant at start
        playerConversant = GameManager.Instance.GetPlayer().GetComponent<PlayerConversant>();
        //Subscribe to dialogue update event
        playerConversant.OnDialogueUpdated += PlayerConversant_OnDialogueUpdated;
        //Update UI at the start of the game.
        UpdateUI();
    }
    
    private void PlayerConversant_OnDialogueUpdated(PlayerConversant obj)
    {
        UpdateUI();
    }
    //<summary>
    //Update the UI after choose an answer choice
    //</summary>
    private void UpdateUI()
    {
        //If there is no any dialogue is running on then return
        if(!playerConversant.IsActive()) return;

        //Update dialogue text
        dialogueText.text = playerConversant.GetText();

        //Destroy previous answer and create new answers.
        foreach(Transform item in choiceRoot)
        {
            Destroy(item.gameObject);
        }
        BuildChoiceList();
      
    }

    //<summary>
    //Create new choice list.
    //</summary>
    private void BuildChoiceList()
    {
        foreach (DialogueNode choiceNode in playerConversant.GetChoices())
        {
            GameObject tempChoicePf = Instantiate(choicePrefab, choiceRoot);
            var textComp = tempChoicePf.GetComponentInChildren<TextMeshProUGUI>();
            textComp.text = choiceNode.GetText();

            Button btn = tempChoicePf.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() =>
            {
                //If the answer that selected has a reply from AI then continue
                if (playerConversant.HasNext(choiceNode))
                {
                    //Select a choice.
                    playerConversant.SelectChoice(choiceNode);
                }
                //If the answer that selected has not a reply from AI then quit dialogue and hide the dialogue panel.
                else
                {
                    //Quit the dialogue
                    playerConversant.QuitDialogue();
                    //Hide the panel
                    Hide();
                }




            });
        }
    }
    public void Show()
    {
        dialogueSection.gameObject.SetActive(true);
    }
    public void Hide()
    {
        dialogueSection.gameObject.SetActive(false);
    }
}
