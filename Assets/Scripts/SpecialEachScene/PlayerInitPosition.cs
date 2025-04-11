using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

//Инициализирует позицию игрока на сцене. Используется для переходов между сценами с помощью двери,
//чтобы при переходах туда-сюда он спавнился около двери, а не в стартовой позиции
public class PlayerInitPosition : MonoBehaviour
{
    public static PlayerInitPosition Instance { get; private set; }

    private Queue<Vector3> posStack;//Сохраняет последнюю позицию игрока на определённой сцене
    private Queue<int> sceneStack;//На какой сцене
    private Queue<Quaternion> rotStack;// Сохраняет последнюю позицию поворота игрока на определённой сцене
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            posStack = new Queue<Vector3>();
            sceneStack = new Queue<int>();
            rotStack = new Queue<Quaternion>();
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
