using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт управления всплывающими окошками. Навешивается на само окно
/// </summary>
public class PopUpNotice : MonoBehaviour
{
    
    public string TitleText;
    public string DescriptionText;
    public string ButtonText;
    /// <summary>
    /// На какую сцену преместимся при нажатии кнопки 
    /// </summary>
    public int SceneIndex;

    /// <summary>
    /// Заголовочный текст окна
    /// </summary>
    private TextMeshProUGUI _title;
    /// <summary>
    /// Текст описания над кнопкой
    /// </summary>
    private TextMeshProUGUI _description;
    /// <summary>
    /// Кнопка перехода куда либо
    /// </summary>
    private Button _button;
    /// <summary>
    /// Кнопка закрытия окна
    /// </summary>
    private Button _cancelButton;

    private void Awake()
    {
        _title = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _description = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _button = transform.GetChild(2).GetComponent<Button>();
        _cancelButton = transform.GetChild(3).GetComponent<Button>();

        _title.text = TitleText;
        _description.text = DescriptionText;
        _button.GetComponentInChildren<TextMeshProUGUI>().text = ButtonText;

        _button.onClick.AddListener(OnButtonClick);
        _cancelButton.onClick.AddListener(OnCancelClick);
    }
    /// <summary>
    /// Переход на другую сцену
    /// </summary>
    private void OnButtonClick()
    {
        if (SceneIndex == 0)
            ScenesManager.Instance.OnMainMenu();
        else
            ScenesManager.Instance.OnSelectedScene(SceneIndex);
    }
    /// <summary>
    /// Выключение окна
    /// </summary>
    private void OnCancelClick()
    {
        this.gameObject.SetActive(false);
    }
}
