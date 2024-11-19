using System;
using UnityEngine;

/// <summary>
/// Интерфейс, поддерживающий "интерактивные объекты".
/// </summary>
public interface IInteractiveObject
{
    /// <summary>
    /// Возвращает кнопку взаимодействия с объектом.
    /// </summary>
    public KeyCode Key { get; }
    /// <summary>
    /// Возвращает радиус поля взаимодействия.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public float Radius { get; }
    /// <summary>
    /// Взаимодействие с объектом.
    /// </summary>
    public void Interact();
}
