﻿using System.Collections;
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

    public void Awake()
    {
        switch (SwitchOn) 
        {
            case States.Next: Index = SceneManager.GetActiveScene().buildIndex + 1; break;
            case States.Previous: Index = SceneManager.GetActiveScene().buildIndex - 1; break;
            case States.Current: Index = SceneManager.GetActiveScene().buildIndex; break;
        }
    }
    public void Switch()
    {
        switch (SwitchOn)
        {
            case States.Next: ScenesManager.Instance.OnNextScene(); break;
            case States.Previous:
                {
                    if (Index == 0)
                        ScenesManager.Instance.OnMainMenu();
                    else ScenesManager.Instance.OnPreviousScene();
                    break; 
                }
            case States.Current: ScenesManager.Instance.OnSelectedScene(SceneManager.GetActiveScene().buildIndex); break;
            case States.ByName:
                { 
                    if (Name.StartsWith("MainMenu"))
                        ScenesManager.Instance.OnMainMenu();
                    else ScenesManager.Instance.OnSelectedScene(Name); 
                    break; 
                }
            case States.ByIndex:
                {
                    if (Index == 0)
                        ScenesManager.Instance.OnMainMenu();
                    else ScenesManager.Instance.OnSelectedScene(Index); 
                    break; 
                }
            case States.ByDialogue: ScenesManager.Instance.OnSelectedScene(PlayerPrefs.GetInt("nextScene")); break;
        }
    }
}
