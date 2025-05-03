using System;
using System.Collections.Generic;
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
    GameObject inventoryUI;
    GameObject bloodEffect;
    GameObject dialogueWnd;

    private AudioClip _oldBackGroundAudio;
    public static event Action<string> AudioEvent;
    private List<bool> StartState; //отключено ли управление у игрока в самом начале включения меню
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
        Debug.Log("ON SKILL TREE");
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
            AudioEvent?.Invoke(_oldBackGroundAudio != null ? _oldBackGroundAudio.name : "");
            ChangeWeaponActivity(true);
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
        Counter.Instance().points = PlayerData.Score;
        Debug.Log("ON CONTROLL MENU");
        Debug.Log(this);
        if (_animator.GetBool(name: "startOpen"))
        {
            Debug.Log("CLOSE");
            if (!transform.GetChild(0).gameObject.activeSelf)
            {
                isOpen = false;
                AudioEvent?.Invoke(_oldBackGroundAudio != null ? _oldBackGroundAudio.name : "");
                ChangeWeaponActivity(true);
            }
            GameObject popUp = transform.Find("PopUpNotice").gameObject;
            if (popUp.activeSelf)
                popUp.SetActive(false);
            _animator.SetBool(name: "startOpen", false);
        }
        else
        {
            Debug.Log("OPEN");
            if (!isOpen)
                ChangeWeaponActivity(false);
            isOpen = true;
            _oldBackGroundAudio = SoundManager._currentBackGroundAudio;
            AudioEvent?.Invoke("pause_audio");
            _animator.SetBool(name: "startOpen", true);
        }

        _audio.enabled = isOpen;
        _save.enabled = isOpen;
        _onSkillTree.enabled = isOpen;
        _inMainMenu.enabled = isOpen;
        _restartScene.enabled = isOpen;

    }
    //Выключает/включает остальные UI элементы и игровое время 
    private void ChangeWeaponActivity(bool active)
    {
        Debug.Log("CHANGE WEAPON ACTIVITY");
        Debug.Log(this);

        if (active) //включение компонентов
        {
            Time.timeScale = 1;

            fader?.gameObject.SetActive(true);
            interactiveUI?.SetActive(true);
            inventoryUI?.SetActive(true);
            bloodEffect?.SetActive(true);
            dialogueWnd?.SetActive(true);

            SetAct(true);
        }
        else //Выключение компонентов
        {
            Time.timeScale = 0;

            Image fd = null;
            GameObject interactUI = null;
            GameObject invantUI = null;

            //Объекты, которые на сцене в единственном экземпляре ищутся только один раз
            if (fader == null)
                fd = GameObject.FindWithTag("Fader")?.GetComponentInChildren<Image>();
            if (interactiveUI == null)
                interactUI = GameObject.Find("InteractiveUI");
            if (inventoryUI == null)
                invantUI = GameObject.Find("InventoryAndConsumableCounterUI");

            //Всегда ищется активное диалоговое окно и BloodEffect
            GameObject dW = GameObject.FindWithTag("DialogueWindow");
            GameObject bE = GameObject.Find("BloodEffect");

            //Сохранение ссылок на объекты для их последующего включения
            if (fd != null && fader == null)
                fader = fd;
            if (interactUI != null && interactiveUI == null)
                interactiveUI = interactUI;
            if (invantUI != null && inventoryUI == null)
                inventoryUI = invantUI;
            if (bE != null)
                bloodEffect = bE;
            if (dW != null)
                dialogueWnd = dW;

            Debug.Log($"FADER: {fader}");
            Debug.Log($"INTERECT: {interactiveUI}");
            Debug.Log($"INVENT: {inventoryUI}");
            Debug.Log($"BLOOD EFFECT: {bloodEffect}");
            Debug.Log($"DIALOGUE WND: {dialogueWnd}");

            fader?.gameObject.SetActive(false);
            interactiveUI?.SetActive(false);
            inventoryUI?.SetActive(false);
            bloodEffect?.SetActive(false);
            dialogueWnd?.SetActive(false);
            SetAct(false); 
           
        }
    }
    private void SetAct(bool active)
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (StartState == null)
        {
            StartState = new List<bool> { true, true, true };
            if (player.GetComponent<PlayerGrenade>() != null) StartState[0] = player.GetComponent<PlayerGrenade>().enabled;
            if (player.GetComponent<PlayerKnife>() != null) StartState[1] = player.GetComponent<PlayerKnife>().enabled;
            if (player.GetComponent<PlayerShooting>() != null) StartState[2] = player.GetComponent<PlayerShooting>().enabled;
        }

        if (active)
        {
            if (player.GetComponent<PlayerGrenade>() != null && StartState[0]) player.GetComponent<PlayerGrenade>().enabled = true;
            if (player.GetComponent<PlayerKnife>() != null && StartState[1]) player.GetComponent<PlayerKnife>().enabled = true;
            if (player.GetComponent<PlayerShooting>() != null && StartState[2]) player.GetComponent<PlayerShooting>().enabled = true;
        }
        else
        {
            if (player.GetComponent<PlayerGrenade>() != null) player.GetComponent<PlayerGrenade>().enabled = false;
            if (player.GetComponent<PlayerKnife>() != null) player.GetComponent<PlayerKnife>().enabled = false;
            if (player.GetComponent<PlayerShooting>() != null) player.GetComponent<PlayerShooting>().enabled = false;
        }
    }
}

