using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    private static GameOverMenu _instance;

    private GameObject _gameOverPanel;
    private TextMeshProUGUI _gameOverText;
    private Button _mainMenuButton;
    private Button _restartButton;

    public static GameOverMenu Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("GameOverMenu is null!");
            return _instance;
        }
    }

    public static event Action<string> AudioEvent;

    public void ShowGameOver()
    {
        Image fd = GameObject.FindWithTag("Fader")?.GetComponentInChildren<Image>();
        GameObject interactUI = GameObject.Find("InteractiveUI");
        GameObject inventUI = GameObject.Find("InventoryAndConsumableCounterUI");
        GameObject bE = GameObject.Find("BloodEffect");
        GameObject dW = GameObject.Find("DialogueWindow");
        GameObject gm = GameObject.Find("GameMenu");


        fd?.gameObject.SetActive(false);
        interactUI?.SetActive(false);
        inventUI?.SetActive(false);
        bE?.SetActive(false);
        if (dW != null && dW.GetComponent<DialogueWndState>().currentState == DialogueWndState.WindowState.StartPrint)
            dW.SetActive(false);
        gm?.SetActive(false);

        AudioEvent?.Invoke("game_over_audio");
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void LoadMainMenu()
    {
        ScenesManager.Instance.OnMainMenu();
    }

    private void RestartLevel()
    {
        Debug.Log("Restart");
        ScenesManager.Instance.OnSelectedScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Awake()
    {
        _gameOverText = this.GetComponentInChildren<TextMeshProUGUI>();
        _mainMenuButton = this.transform.Find("Panel/MainMenuButton").GetComponentInChildren<Button>();
        _restartButton = this.transform.Find("Panel/RestartButton").GetComponentInChildren<Button>();
        _gameOverPanel = this.transform.Find("Panel").gameObject;

        _gameOverPanel.SetActive(false);

        _mainMenuButton.onClick.AddListener(LoadMainMenu);
        _restartButton.onClick.AddListener(RestartLevel);

        _instance = this;
    }
}
