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
    /// Возвращает всплывающий над интерактивным объектом текст.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public TextMeshProUGUI Description { get; set; }
    /// <summary>
    /// Возвращает и изменяет кнопку взаимодействия с объектом.
    /// </summary>
    public KeyCode Key { get; set; }
    /// <summary>
    /// Возвращает и изменяет название шрифта для интерактивного текста.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public string FontName { get; set; }
    /// <summary>
    /// Возвращает и изменяет размер шрифта.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public float FontSize { get; set; }
    /// <summary>
    /// Возвращает и изменяет радиус поля взаимодействия.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public float Radius { get; set; }
    /// <summary>
    /// Возвращает и изменяет вертикальное смещение всплывающего текста.
    /// </summary>
    public float OffSet { get; set; }
    /// <summary>
    /// Взаимодействие с объектом.
    /// </summary>
    public void Interact();
}
