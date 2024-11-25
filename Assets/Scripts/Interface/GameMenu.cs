using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Скрипт управления внутриигровым меню. Навешивается на GameMenu
/// </summary>
public class GameMenu : MonoBehaviour
{
    /// <summary>
    /// Управляет аниацией открытия меню
    /// </summary>
    [SerializeField]
    private Animator _animator;
    /// <summary>
    /// Кнопка сохранения настроек
    /// </summary>
    private Button _save;
    /// <summary>
    /// Кнопка выхода в главное меню
    /// </summary>
    private Button _inMainMenu;
    /// <summary>
    /// Всплывающее окно при нажатии кнопки "В главное меню"
    /// </summary>
    private GameObject _notice;
    private void Start()
    {
        Button icon = GameObject.Find("MenuIcon").GetComponent<Button>();
        icon.onClick.AddListener(ControllMenu);
        
        GameObject menu = GameObject.Find("SettingsMenu");
        _save = menu.transform.GetChild(3).GetComponent<Button>();
        _inMainMenu = menu.transform.GetChild(4).GetComponent<Button>();
        _save.onClick.AddListener(Save);
        _inMainMenu.onClick.AddListener(InMainMenu);

        _notice = transform.GetChild(3).gameObject;
    }
    /// <summary>
    /// Переключение на главное меню (без сохранения прогресса на текущей сцене)
    /// </summary>
    private void InMainMenu()
    {
        _notice.SetActive(true);
    }
    /// <summary>
    /// Закрытие меню с сохранением настроек
    /// </summary>
    private void Save()
    {
        _animator.SetBool(name: "startOpen", false);
    }
    /// <summary>
    /// Открытие/закрытие меню по нажатию на иконку менюшки без сохранения настроек
    /// </summary>
    private void ControllMenu()
    {
        if (_animator.GetBool(name: "startOpen"))
            _animator.SetBool(name: "startOpen", false);
        else
            _animator.SetBool(name: "startOpen", true);
    }
}
