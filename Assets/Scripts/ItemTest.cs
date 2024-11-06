using UnityEngine;

/// <summary>
/// Класс, реализующий тестовый интерактивный объект.
/// </summary>
public class ItemTest : AbstractInteractiveObject
{
    /// <summary>
    /// Взаимодействие с интерактивным объектом.
    /// </summary>
    public override void Interact()
    {
        Debug.Log("ItemTest!");
    }
}
