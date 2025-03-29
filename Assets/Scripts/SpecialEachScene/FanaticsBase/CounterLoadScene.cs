using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CounterLoadScene : MonoBehaviour
{
    /*
    public string SceneName;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneName)
        {
            int sceneLoadCount = PlayerPrefs.GetInt("LoadingNumber", 1); 
            sceneLoadCount++;
            PlayerPrefs.SetInt("LoadingNumber", sceneLoadCount);
            PlayerPrefs.Save(); 
            Debug.Log("Scene " + scene.name + " loaded for the " + sceneLoadCount + " time (saved in PlayerPrefs).");
        }
    }
    */
}
