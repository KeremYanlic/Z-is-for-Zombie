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

        quitBtn.onClick.AddListener(() =>
        {
            playerConversant.QuitDialogue();
            gameObject.SetActive(false);
        });

        Hide();
    }
    private void Start()
    {
        playerConversant = GameManager.Instance.GetPlayer().GetComponent<PlayerConversant>();
        playerConversant.OnDialogueUpdated += PlayerConversant_OnDialogueUpdated;
        //Update UI at the start of the game.
        UpdateUI();
    }

    private void PlayerConversant_OnDialogueUpdated(PlayerConversant obj)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if(!playerConversant.IsActive()) return;

        dialogueText.text = playerConversant.GetText();
        //gameObject.SetActive(playerConversant.HasNext());
        foreach(Transform item in choiceRoot)
        {
            Destroy(item.gameObject);
        }
        BuildChoiceList();
      
    }
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
                if (playerConversant.HasNext(choiceNode))
                {
                    playerConversant.SelectChoice(choiceNode);
                }
                else
                {
                    playerConversant.QuitDialogue();

                    gameObject.SetActive(false);

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
