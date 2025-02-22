using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractiveButton : MonoBehaviour
{
    /// <summary>
    /// ��������� ��������������� ������� (������ ������ ������ ��� ��������� �������)
    /// </summary>
    /// <param name="btn"></param>
    private void Start()
    {
        Button btn = GetComponent<Button>();

        TextMeshProUGUI text = btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        EventTrigger eventTrigger = btn.gameObject.AddComponent<EventTrigger>();

        // ������� ������� ��� ��������� ������� �� ������
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        // ��������� ���������� �������
        pointerEnterEntry.callback.AddListener((data) => { text.fontSize = text.fontSize + 4; });
        eventTrigger.triggers.Add(pointerEnterEntry);

        // ������� ������� ��� ����� ������� � ������
        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        // ��������� ���������� �������
        pointerExitEntry.callback.AddListener((data) => { text.fontSize = text.fontSize - 4; });
        eventTrigger.triggers.Add(pointerExitEntry);
    }
}
