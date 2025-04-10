﻿using UnityEngine;

/// <summary>
/// Класс, реализующий "интерактивные объекты, которые нельзя взять в инвентарь".
/// </summary>
public class ClickedObject : MonoBehaviour, IInteractable
{
    public string Name { get; }
    public virtual string Description { get; } = SettingData.Interact.ToString();

    public virtual void Interact()
    {
        // Логика взаимодействия
        Debug.Log("ClickedObject");
    }
}