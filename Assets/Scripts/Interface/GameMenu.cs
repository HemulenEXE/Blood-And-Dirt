using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Скрипт управления внутриигровым меню. Навешивается на GameMenu
/// </summary>
public class GameMenu : MonoBehaviour
{
    /// <summary>
    /// Управляет аниацией открытия меню
    /// </summary>
    [SerializeField] private Animator _animator;
    ///  <summary>
    /// Кнопка сохранения настроек
    /// </summary>
    private Button _save;
    /// <summary>
    /// Кнопка выхода в главное меню
    /// </summary>
    private Button _inMainMenu;
    /// <summary>
    /// Кнопка регулирования аудио.
    /// </summary>
    private Slider _audio;
    /// <summary>
    /// Кнопка перезапуска сцены
    /// </summary>
    private Button _restartScene;
    /// <summary>
    /// Кнопка включения/выключения дерева прокачики
    /// </summary>
    private Button _onSkillTree;
    //Открыто ли меню или его вложенные элементы (дерево прокачки)
    private bool isOpen = false;
    //Объеекты, которые нужно отключать, если включено меню
    [NonSerialized]
    public Image fader;
    GameObject interactiveUI;
    GameObject invantoryUI;
    GameObject bloodEffect;
    GameObject dialogueWnd;

    private AudioClip _oldBackGroundAudio;
    public static event Action<string> AudioEvent;

    /// <summary>
    /// Проверка и настройка полей.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    private void Start()
    {
        GameObject menu = GameObject.Find("SettingsMenu");
        if (menu == null) throw new ArgumentNullException("GameMenu: menu is null");
        _save = menu?.transform?.GetChild(2)?.GetComponent<Button>();
        _inMainMenu = menu?.transform?.GetChild(3)?.GetComponent<Button>();
        _restartScene = menu?.transform?.GetChild(4)?.GetComponent<Button>();
        _audio = menu?.transform?.GetChild(1)?.GetComponent<Slider>();
        _onSkillTree = menu.transform.GetChild(5).GetComponent<Button>();
        if (_save == null) throw new ArgumentNullException("GameMenu: _save is null");
        if (_inMainMenu == null) throw new ArgumentNullException("GameMenu: _inMainMenu is null");
        if (_restartScene == null) throw new ArgumentNullException("GameMenu: _restartScene is null");
        if (_audio == null) throw new ArgumentNullException("GameMenu: _audio is null");
        _save.onClick.AddListener(Save);
        _inMainMenu.onClick.AddListener(InMainMenu);
        _restartScene.onClick.AddListener(RestartScene);
        _audio.onValueChanged.AddListener(SetVolume);
        _audio.value = SettingData.Volume;
        _onSkillTree.onClick.AddListener(OnSkillTree);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ControllMenu();
    }
    private void OnSkillTree()
    {
        GameObject skillTree = this.transform.GetChild(0).gameObject;
        if (skillTree.activeSelf)
        {
            _onSkillTree.GetComponentInChildren<TextMeshProUGUI>().text = "К дереву прокачки";
            skillTree.transform.GetChild(1).localScale = skillTree.GetComponentInChildren<ZoomAndMotion>().StartScale();
            skillTree.transform.GetChild(1).position = skillTree.GetComponentInChildren<ZoomAndMotion>().StartPoint();
            skillTree.SetActive(false);
        }
        else
        {
            skillTree.SetActive(true);
            _onSkillTree.GetComponentInChildren<TextMeshProUGUI>().text = "Обратно";
        }
    }
    /// <summary>
    /// Переключение на главное меню (без сохранения прогресса на текущей сцене)
    /// </summary>
    private void InMainMenu()
    {
        transform.Find("PopUpNotice").gameObject.SetActive(true);
    }
    /// <summary>
    /// Установка громокости звука.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void SetVolume(float volume)
    {
        SettingData.SetVolume(volume);
        SettingData.ApplySettings();
    }
    /// <summary>
    /// Закрытие меню с сохранением настроек
    /// </summary>
    private void Save()
    {
        _animator.SetBool(name: "startOpen", false);
        if (!transform.GetChild(0).gameObject.activeSelf)
        {
            isOpen = false;
            AudioEvent?.Invoke(_oldBackGroundAudio.name);
            ChangeWeaponActivity();
        }
        SettingData.SaveData();
    }
    /// <summary>
    /// Запускает заново сцену
    /// </summary>
    private void RestartScene()
    {
        GameObject notice = transform.Find("PopUpNotice").gameObject;
        notice.SetActive(true);
        //Указание на какую сцену перейти.
        notice.GetComponent<PopUpNotice>().SceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    /// <summary>
    /// Открытие/закрытие меню по нажатию на иконку менюшки без сохранения настроек
    /// </summary>
    private void ControllMenu()
    {
        if (_animator.GetBool(name: "startOpen"))
        {
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
                isOpen = false;
                AudioEvent?.Invoke(_oldBackGroundAudio.name);
                ChangeWeaponActivity();
            }
            _animator.SetBool(name: "startOpen", false);
        }
        else
        {
            if (!isOpen)
                ChangeWeaponActivity();
            isOpen = true;
            _oldBackGroundAudio = SoundManager._currentBackGroundAudio;
            AudioEvent?.Invoke("pause_audio");
            _animator.SetBool(name: "startOpen", true);
        }
    }
    //Выключает/включает остальные UI элементы и игровое время 
    private void ChangeWeaponActivity()
    {
        Debug.Log("CHANGE WEAPON ACTIVITY");

        Time.timeScale = Time.timeScale == 1 ? 0 : 1;

        Image fd = GameObject.FindWithTag("Fader")?.GetComponentInChildren<Image>();
        GameObject interactUI = GameObject.Find("InteractiveUI");
        GameObject invantUI = GameObject.Find("InventoryAndConsumableCounterUI");
        GameObject bE = GameObject.Find("BloodEffect");
        GameObject dW = GameObject.Find("DialogueWindow");

        //Сохранение ссылок на объекты для их последующего включения
        if (fd != null && fader == null)
            fader = fd;
        if (interactUI != null && interactiveUI == null)
            interactiveUI = interactUI;
        if (invantUI != null && invantoryUI == null)
            invantoryUI = invantUI;
        if (bE != null && bloodEffect == null)
            bloodEffect = bE;
        if (dW != null && dialogueWnd == null)
            dialogueWnd = dW;

        fader?.gameObject.SetActive(!fader.gameObject.activeSelf);
        interactiveUI?.SetActive(!interactiveUI.activeSelf);
        invantoryUI?.SetActive(!invantoryUI.activeSelf);
        bloodEffect?.SetActive(!bloodEffect.activeSelf);
        if (dialogueWnd != null && dialogueWnd.GetComponent<DialogueWndState>().currentState == DialogueWndState.WindowState.StartPrint)
            dialogueWnd.SetActive(!dialogueWnd.activeSelf);

        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<PlayerKnife>().enabled = !player.GetComponent<PlayerKnife>().enabled;
        player.GetComponent<PlayerShooting>().enabled = !player.GetComponent<PlayerShooting>().enabled;
        player.GetComponent<PlayerGrenade>().enabled = !player.GetComponent<PlayerGrenade>().enabled;
    }
}

