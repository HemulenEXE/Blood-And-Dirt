using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

//Инициализирует позицию игрока на сцене. Используется для переходов между сценами с помощью двери,
//чтобы при переходах туда-сюда он спавнился около двери, а не в стартовой позиции
public class PlayerInitPosition : MonoBehaviour
{
    public static PlayerInitPosition Instance { get; private set; }

    public Vector3 position; //Сохраняет последнюю позицию игрока на определённой сцене
    public int onScene; //На какой сцене 
    public Quaternion rotation; // Сохраняет последнюю позицию поворота игрока на определённой сцене
    private void Awake()
    {
        Debug.Log("Awake происходит");
        if (Instance == null)
        {
            Debug.Log("Instance - null");
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Сохраняем объект при загрузке новой сцены
        }
    }
    public void SavePosition(int scene, Vector3 pos, Quaternion rot)
    {
        Debug.Log("Новая позиция появления сохранена!");
        position = pos;
        onScene = scene;
        rotation = rot;
        Debug.Log($"Position: {position}, onScene: {onScene}");
    }
}
