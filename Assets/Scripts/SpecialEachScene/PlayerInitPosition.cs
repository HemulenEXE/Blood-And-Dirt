using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

//Инициализирует позицию игрока на сцене. Используется для переходов между сценами с помощью двери,
//чтобы при переходах туда-сюда он спавнился около двери, а не в стартовой позиции
public class PlayerInitPosition : MonoBehaviour
{
    private static PlayerInitPosition instance;

    private static Queue<Vector3> posStack;//Сохраняет последнюю позицию игрока на определённой сцене
    private static Queue<int> sceneStack;//На какой сцене
    private static Queue<Quaternion> rotStack;// Сохраняет последнюю позицию поворота игрока на определённой сцене
    public static PlayerInitPosition Instance
    {
        get 
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("PlayerInitPosition");
                instance = obj.AddComponent<PlayerInitPosition>();
                posStack = new Queue<Vector3>();
                sceneStack = new Queue<int>();
                rotStack = new Queue<Quaternion>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance; 
        }
    }
    public bool IsEmpty() { Debug.Log(posStack.Count); return posStack.Count == 0; }
    public Vector3 Position() { sceneStack.Dequeue(); return posStack.Dequeue(); } 
    public int OnScene() { return sceneStack.Peek(); }
    public Quaternion Rotate() { return rotStack.Dequeue(); }
    public void SavePosition(int scene, Vector3 pos, Quaternion rot)
    {
        Debug.Log("Новая позиция появления сохранена!");
        posStack.Enqueue(pos);
        sceneStack.Enqueue(scene);
        rotStack.Enqueue(rot);
        Debug.Log($"Position: {pos}, onScene: {scene}, rotate: {rot}");
    }
}
