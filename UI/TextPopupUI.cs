using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopupUI : SingletonMonobehaviour<TextPopupUI>
{
    private TextMeshProUGUI popupText;
    private Coroutine updateTextCoroutine;
    private Coroutine fadeInTextCoroutine;

    protected override void Awake()
    {
        base.Awake();

        popupText = transform.Find("popupText").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        ResetCoroutine();
    }


    private IEnumerator UpdateTextRoutine(string text)
    {
        popupText.text = text;

        Color color = popupText.GetComponent<TextMeshProUGUI>().color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime;
            yield return null;
            popupText.GetComponent<TextMeshProUGUI>().color = color;
        }
        FadeIn();

    }

    public void UpdateText(string text)
    {

        updateTextCoroutine = StartCoroutine(UpdateTextRoutine(text));
    }

    private void FadeIn()
    {
        fadeInTextCoroutine = StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        Color color = popupText.GetComponent<TextMeshProUGUI>().color;
        yield return new WaitForSeconds(2f);

        while (color.a > 0f)
        {
            color.a -= Time.deltaTime;
            yield return null;
            popupText.GetComponent<TextMeshProUGUI>().color = color;
        }

        ResetText();


    }

    private void ResetCoroutine()
    {
        ResetText();

        if (updateTextCoroutine != null)
        {
            StopCoroutine(UpdateTextRoutine(""));
        }
    }

    private void ResetText()
    {
        popupText.text = "";

        Color color = popupText.GetComponent<TextMeshProUGUI>().color;
        color.a = 0f;

        popupText.GetComponent<TextMeshProUGUI>().color = color;
    }

}
