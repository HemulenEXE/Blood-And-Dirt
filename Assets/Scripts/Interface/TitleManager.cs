using CameraLogic.CameraEffects;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    private GameObject _panel;
    private TextMeshProUGUI _text;

    public float normalScrollSpeed = 50f;
    public float fastScrollSpeed = 150f;
    public float displayTime = 3f;
    private bool isScrolling = false;

    public string _credits = "Игра закончена\n\n\n\n\nСоздатели\n\nKасеев Артём\nОбезьяна с плёткой\n\nKорчагина Мария\n-\n\nИгнатьев Михаил\nЗастрелите меня\n\nСентюрина Дарья\n-\n\nЕвтухов Дмитрий\n-\n\nАлешкин Никита\nЧеремша\n\nДьяков Владимир\nА где...?";
    public static event Action<string> AudioEvent;

    public void ShowCredits()
    {
        Image fd = GameObject.FindWithTag("Fader")?.GetComponentInChildren<Image>();
        GameObject interactUI = GameObject.Find("InteractiveUI");
        GameObject inventUI = GameObject.Find("InventoryAndConsumableCounterUI");
        GameObject bE = GameObject.Find("BloodEffect");
        GameObject dW = GameObject.Find("DialogueWindow");
        GameObject gm = GameObject.Find("GameMenu");


        fd?.gameObject.SetActive(false);
        interactUI?.SetActive(false);
        inventUI?.SetActive(false);
        bE?.SetActive(false);
        if (dW != null && dW.GetComponent<DialogueWndState>().currentState == DialogueWndState.WindowState.StartPrint)
            dW.SetActive(false);
        gm?.SetActive(false);

        StartCoroutine(LoadCredits());
    }

    private IEnumerator LoadCredits()
    {
        AudioEvent?.Invoke("title");
        string credits = _credits;

        yield return new WaitForSecondsRealtime(displayTime);

        _text.text = credits;
        _panel.SetActive(true);

        Time.timeScale = 0;

        

        isScrolling = true;
        StartCoroutine(ScrollCredits());
    }

    private IEnumerator ScrollCredits()
    {
        RectTransform creditsRect = _text.GetComponent<RectTransform>();
        Vector3 startPosition = creditsRect.anchoredPosition;

        while (isScrolling)
        {
            float currentScrollSpeed = Input.GetKey(KeyCode.Space) ? fastScrollSpeed : normalScrollSpeed;
            creditsRect.anchoredPosition += new Vector2(0, currentScrollSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
    }

    public void StopCredits()
    {
        isScrolling = false;
        _panel.SetActive(false);
        ScenesManager.Instance.OnMainMenu();
    }

    private void Awake()
    {
        _panel = this.transform.Find("Panel").gameObject;
        _text = this.GetComponentInChildren<TextMeshProUGUI>();
        _panel.SetActive(false);
    }
}
