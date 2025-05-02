using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowDialogueWithLogging : ShowDialogueDubl
{
    public static event Action<ShowDialogueWithLogging,Dialogue.Answer> OnAnswerChosen;
    // Переопределим метод GoToAnswers
    protected override void GoToAnswers()
    {
        Debug.Log("GoToAnswers запущен!");
        _replicParts.Clear();
        _continue.gameObject.SetActive(false);
        _npcName.gameObject.SetActive(false);
        _panelForText.gameObject.SetActive(false);

        //Очистка старых кнопок
        var buttons = _panelForButtons.GetComponentsInChildren<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.RemoveAllListeners();
            DestroyImmediate(btn.gameObject);
        }
        _panelForButtons.gameObject.SetActive(true);
        //Добавление новых кнопок
        foreach (Dialogue.Answer answ in _dialogue.GetCurentNode().answers)
        {
            Dialogue.Answer locAnsw = answ;
            Button btn = Instantiate(_prefab, _panelForButtons);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = locAnsw.text;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                _audio.Play();
                Debug.Log($"Игрок выбрал ответ: {locAnsw.text}, toNode: {locAnsw.toNode}, toScene: {locAnsw.toScene}");
                if(locAnsw.toNode ==11)
                {
                    _dialogue.Nodes[11].npcText = "У тебя " + BalancePlayer.Instance.CurrentMoney() + " очков";
                }
                OnAnswerChosen?.Invoke(this,locAnsw);
                if (locAnsw.toScene != 0)
                {
                    PlayerPrefs.SetInt("nextScene", locAnsw.toScene); //Сохраняет ключевые ответы 
                    PlayerPrefs.Save();
                }
                if (locAnsw.exit == "True")
                    EndDialogue();
                else
                {
                    _dialogue.ToNodeWithInd(locAnsw.toNode);
                    if (_dialogue.GetCurentNode().npcText != null)
                        GoToReplic();
                    else GoToAnswers();
                }
            });
        }
    }
}
