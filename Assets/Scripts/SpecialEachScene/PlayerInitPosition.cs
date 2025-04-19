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

    private static List<Vector3> posStack;//Сохраняет последнюю позицию игрока на определённой сцене
    private static List<int> sceneStack;//На какой сцене
    private static List<Quaternion> rotStack;// Сохраняет последнюю позицию поворота игрока на определённой сцене
    public static PlayerInitPosition Instance
    {
        get 
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("PlayerInitPosition");
                instance = obj.AddComponent<PlayerInitPosition>();
                posStack = new List<Vector3>();
                sceneStack = new List<int>();
                rotStack = new List<Quaternion>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance; 
        }
    }
    public bool IsEmpty() {return posStack.Count == 0; }
    public Vector3 Position(int i) { 
        var res = posStack[i];
        posStack.RemoveAt(i);
        sceneStack.RemoveAt(i); //!
        return res;
    } 
    public List<int> OnScene() { return sceneStack; }
    public Quaternion Rotate(int i) {
        var res = rotStack[i];
        rotStack.RemoveAt(i);
        return res;
    }
    public void SavePosition(int scene, Vector3 pos, Quaternion rot)
    {
        Debug.Log("Новая позиция появления сохранена!");
        posStack.Add(pos);
        sceneStack.Add(scene);
        rotStack.Add(rot);
        Debug.Log($"Position: {pos}, onScene: {scene}, rotate: {rot}");
    }
}
