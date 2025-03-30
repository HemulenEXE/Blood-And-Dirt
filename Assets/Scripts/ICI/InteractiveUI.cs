using System;
using TMPro;
using UnityEngine;

public class InteractiveUI : MonoBehaviour
{
    public TextMeshProUGUI _interactiveText;
    [SerializeField] private float _offSet = 0.5f;

    public void TurnOnText(GameObject item)
    {
        var temp = item.GetComponent<IInteractable>();
        if (temp == null) return;

        _interactiveText.gameObject.SetActive(true);
        // Корректировка интерактивного текста
        _interactiveText.text = temp.Description.ToString();
        Vector3 positionItem = item.transform.position;
        positionItem.y = item.GetComponent<Renderer>().bounds.max.y + _offSet; // Получение верхней границы визуального представления объекта
        positionItem.x = (item.GetComponent<Renderer>().bounds.max.x + item.GetComponent<Renderer>().bounds.min.x) / 2;
        Vector3 positionInWorld = RectTransformUtility.WorldToScreenPoint(Camera.main, positionItem);
        _interactiveText.transform.position = positionInWorld;
    }
    public void TurnOffText()
    {
        _interactiveText.text = "";
        _interactiveText.gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (_interactiveText == null) throw new ArgumentNullException("InteractiveUI: _interactiveText is null");
    }
}
