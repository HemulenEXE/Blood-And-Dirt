using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Интерфейс, поддерживающий "интерактивные объекты".
/// </summary>
public interface IInteractiveObject
{
    /// <summary>
    /// Возвращает и изменяет кнопку взаимодействия с объектом.
    /// </summary>
    public KeyCode Key { get; set; }
    /// <summary>
    /// Возвращает и изменяет радиус поля взаимодействия.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public float Radius { get; set; }
    /// <summary>
    /// Взаимодействие с объектом.
    /// </summary>
    public void Interact();
}
