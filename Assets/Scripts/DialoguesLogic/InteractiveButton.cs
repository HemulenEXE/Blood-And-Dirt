using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractiveButton : MonoBehaviour
{
    /// <summary>
    /// Добавляет интерактивности кнопкам (меняет размер текста при наведении курсора)
    /// </summary>
    /// <param name="btn"></param>
    private void Start()
    {
        Button btn = GetComponent<Button>();

        TextMeshProUGUI text = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        EventTrigger eventTrigger = btn.gameObject.AddComponent<EventTrigger>();

        // Создаем событие для наведения курсора на кнопку
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        // Добавляем обработчик события
        pointerEnterEntry.callback.AddListener((data) => { text.fontSize = text.fontSize + 4; });
        eventTrigger.triggers.Add(pointerEnterEntry);

        // Создаем событие для ухода курсора с кнопки
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        // Добавляем обработчик события
        pointerExitEntry.callback.AddListener((data) => { text.fontSize = text.fontSize - 4; });
        eventTrigger.triggers.Add(pointerExitEntry);
    }
}
