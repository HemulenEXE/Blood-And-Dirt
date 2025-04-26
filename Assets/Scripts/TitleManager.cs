using System;
using System.Collections;
using TMPro;
using UnityEngine;

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

    private void Awake()
    {
        _panel = transform.Find("Panel").gameObject;
        _text = this.GetComponentInChildren<TextMeshProUGUI>();
        _panel.SetActive(false);
    }

    public void ShowCredits()
    {
        Time.timeScale = 0;
        StartCoroutine(LoadCredits());
    }

    private IEnumerator LoadCredits()
    {
        AudioEvent?.Invoke("title");
        string credits = _credits;

        _text.text = credits;
        _panel.SetActive(true);

        yield return new WaitForSeconds(displayTime);

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
            creditsRect.anchoredPosition += new Vector2(0, currentScrollSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void StopCredits()
    {
        isScrolling = false;
        _panel.SetActive(false);
        // SceneManager.LoadScene("MainMenu");
    }

}
