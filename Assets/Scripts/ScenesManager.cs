using CameraLogic.CameraEffects;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Скрипт управления переходами между сценами + сохранением состояния сцен. Методы вызываются в других скриптах
/// </summary>
public class ScenesManager : MonoBehaviour
{ 
    /// <summary>
    /// Идёт ли затемнение экрана
    /// </summary>
    private static bool _isfade = false;
    private bool isLoad = false; //Идёт ли загрузка сцены
    /// <summary>
    /// Объект класса через который идёт обращение к нему
    /// </summary>
    private static ScenesManager _instance;
    public static ScenesManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("ScenesManager");
                _instance = obj.AddComponent<ScenesManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    /// <summary>
    /// Переход на главное меню (Текущий уровень не сохраняется)
    /// </summary>
    public void OnMainMenu()
    {
        StartCoroutine(_OnMainMenu());
        PlayerData.SaveData();
    }
    private IEnumerator _OnMainMenu()
    {
        if (!isLoad)
        {
            isLoad = true;
            Time.timeScale = 0;

            Fader.Instance.FadeIn(() => _isfade = true);

            while (!_isfade)
                yield return null;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            if (!asyncLoad.isDone)
                yield return null;

            Fader.Instance.FadeOut(() => _isfade = false);
            Time.timeScale = 1;
            isLoad = false;
        }
    }
    /// <summary>
    /// Переход на следующую сцену
    /// </summary>
    public void OnNextScene()
    {
        int curentInd = SceneManager.GetActiveScene().buildIndex;
        OnSelectedScene(curentInd + 1);
    }
    /// <summary>
    /// Переход на предыдущую сцену
    /// </summary>
    public void OnPreviousScene()
    {
        int curentInd = SceneManager.GetActiveScene().buildIndex;
        OnSelectedScene(curentInd - 1);
    }
    /// <summary>
    /// Переход на заданную сцену по индексу сцены
    /// </summary>
    /// <param name="index"></param>
    public void OnSelectedScene(int index)
    {
        _instance.StartCoroutine(_instance._OnSelectedScene(index));
        if (SceneManager.GetActiveScene().buildIndex != index)
        PlayerData.SaveData();
    }
    private IEnumerator _OnSelectedScene(int index)
    {
        if (!isLoad)
        {
            isLoad = true;
            if (index < 0) throw new ArgumentOutOfRangeException("index can't be < 0!"); //Добавить проверку, что индекс не больше, чем есть индексы у сцен

            Time.timeScale = 0;
            PlayerPrefs.SetInt("currentScene", index); //Сохраняет, что мы перешли на указанный уровень 
            PlayerPrefs.Save();
            Fader.Instance.FadeIn(() => _isfade = true);
            while (!_isfade)
                yield return null;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);

            while (!asyncLoad.isDone)
                yield return null;

            Fader.Instance.FadeOut(() => _isfade = false);
            InitPosition(index);

            while (_isfade)
                yield return null;
            
            Time.timeScale = 1;
            isLoad = false;
        }
    }
    /// <summary>
    /// Переход на заданную сцену по имени сцены
    /// </summary>
    public void OnSelectedScene(string name)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            if (scenePath.EndsWith(name + ".unity"))
                OnSelectedScene(i);
        }
        throw new ArgumentNullException($"Scene with name '{name}' doesn't exist!"); //Уточнить правильно ли осуществляется проверка!
    }
    //Инициализирует позицию игрока на новой сцене, если есть сохранённая
    private void InitPosition(int index)
    {

        //Меняем позицию, если есть сохранённая на этой сцене
        if (!PlayerInitPosition.Instance.IsEmpty())
        {
            int ind = PlayerInitPosition.Instance.OnScene().FindLastIndex(x => x == index);
            if (ind != -1)
            {
                Debug.Log($"Метод инициализации позиции запущен!");

                Transform player = GameObject.FindWithTag("Player").transform;
                player.position = PlayerInitPosition.Instance.Position(ind);
                player.rotation = PlayerInitPosition.Instance.Rotate(ind);
                Transform camera = GameObject.FindWithTag("MainCamera").transform;
                camera.position = new Vector3(player.position.x, player.position.y, -10);
            }
        }
    }
}
