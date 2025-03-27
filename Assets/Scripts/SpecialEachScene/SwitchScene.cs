using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

//Скрипт перехода на следующую сцену по срабатыванию триггеру. То, на какую сцену будет осуществляться переход настраивается.
//Базовый класс для конкретных триггеров. Хранит общие настройки.
public class SwitchScene : MonoBehaviour
{
    public enum States { Next, Previous, Current, ByName, ByIndex, ByDialogue};
    public States SwitchOn; // На какую сцену нужно переместиться. Тип ByDialogue выбирать только тогда, когда решение о том, на какую сцену переходить принималось в зависимость от реплик, выбранных в диалоге
    public string Name = null; //Если перемещение по имени     
    public int Index = -1; //Если перемещение по индексу

    protected void Switch()
    {
        switch (SwitchOn)
        {
            case States.Next: ScenesManager.Instance.OnNextScene(); break;
            case States.Previous: ScenesManager.Instance.OnPreviousScene(); break;
            case States.Current: ScenesManager.Instance.OnSelectedScene(PlayerPrefs.GetInt("currentScene")); break;
            case States.ByName: ScenesManager.Instance.OnSelectedScene(Name); break;
            case States.ByIndex: ScenesManager.Instance.OnSelectedScene(Index); break;
            case States.ByDialogue: ScenesManager.Instance.OnSelectedScene(PlayerPrefs.GetInt("nextScene")); break;
        }
    }
}
