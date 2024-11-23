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
    /// Переход на следующую сцену
    /// </summary>
    public void OnNextScene()
    {
        int curentInd = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(curentInd);
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
    /// Запускает корутину
    /// </summary>
    /// <param name="index"></param>
    public void OnSelectedScene(int index)
    { 
       _instance.StartCoroutine(_instance._OnSelectedScene(index)); 
    }
    /// <summary>
    /// Переход на заданную сцену по индексу сцены
    /// </summary>
    private IEnumerator _OnSelectedScene(int index)
    {

        if (index < 0) throw new ArgumentOutOfRangeException("index can't be < 0!");

        PlayerPrefs.SetInt("currentScene", index); //Сохраняет, что мы прошли текущий уровень 
        PlayerPrefs.Save();

        Fader.Instance.FadeIn(() => _isfade = true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        if (!_isfade && !asyncLoad.isDone)
            yield return null;
        
        Fader.Instance.FadeOut(() => _isfade = false);
    }
    /// <summary>
    /// Переход на заданную сцену по имени сцены
    /// </summary>
    /// 
    public void OnSelectedScene(string name)
    {
        Scene scene = SceneManager.GetSceneByName(name);
        if (!scene.IsValid()) throw new ArgumentNullException($"Scene with name '{name}' doesn't exist!");
        OnSelectedScene(scene.buildIndex); 
    }
}
