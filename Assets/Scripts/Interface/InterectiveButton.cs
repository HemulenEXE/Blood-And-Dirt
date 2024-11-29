using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Отвечает за изменение размера кнопки при наведении на неё курсора. Навешивается на кнопку
/// </summary>
public class InterectiveButton : MonoBehaviour
{
    /// <summary>
    /// Размер, на каторый увеличивается кнопка
    /// </summary>
    public float SizeIncrease = 0.1f;
    /// <summary>
    /// Размер кнопки 
    /// </summary>
    private Vector3 scale;
    void Start()
    {
        scale = this.GetComponent<Button>().transform.localScale;

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        // Создаем событие для наведения курсора на кнопку
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        // Добавляем обработчик события
        pointerEnterEntry.callback.AddListener((data) => IncreaseScale());
        eventTrigger.triggers.Add(pointerEnterEntry);

        // Создаем событие для ухода курсора с кнопки
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        // Добавляем обработчик события
        pointerExitEntry.callback.AddListener((data) => DecreaseScale());
        eventTrigger.triggers.Add(pointerExitEntry);
    }
    /// <summary>
    /// Увеличение размера кнопки
    /// </summary>
    private void IncreaseScale()
    {
        scale.x += SizeIncrease; 
        scale.y += SizeIncrease;
        this.GetComponent<Button>().transform.localScale = new Vector3(scale.x, scale.y, scale.z);
    }
    /// <summary>
    /// Уменьшить размер кнопки 
    /// </summary>
    private void DecreaseScale()
    {
        scale.x -= SizeIncrease;
        scale.y -= SizeIncrease;
        this.GetComponent<Button>().transform.localScale = new Vector3(scale.x, scale.y, scale.z);
    }
}
