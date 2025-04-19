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
        if (temp is Body && !PlayerData.HasSkill<LiveInNotVain>()) return;

        _interactiveText.gameObject.SetActive(true);
        _interactiveText.text = temp.Description.ToString();
        Renderer itemRenderer = item.GetComponentInChildren<Renderer>();
        if (itemRenderer != null)
        {
            Vector2 positionItem = item.transform.position;
            positionItem.y = itemRenderer.bounds.max.y + _offSet; // Получение верхней границы визуального представления объекта
            positionItem.x = (itemRenderer.bounds.max.x + itemRenderer.bounds.min.x) / 2;
            Vector2 positionInWorld = RectTransformUtility.WorldToScreenPoint(Camera.main, positionItem);
            _interactiveText.transform.position = positionInWorld;
        }
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
