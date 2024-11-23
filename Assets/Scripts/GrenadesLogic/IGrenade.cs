using UnityEngine;

/// <summary>
/// Интерфейс гранат
/// </summary>
public interface IGrenade
{
    /// <summary>
    /// Доступ к камере для отслеживания положения курсора мыши
    /// </summary>
    public Camera Camera { get; }
    /// <summary>
    /// Префаб гранаты
    /// </summary>
    public GameObject Prefab { get; }
    /// <summary>
    /// За какое время совершается бросок
    /// </summary>
    public float Time { get; }
    /// <summary>
    /// Метод бросание гранаты
    /// </summary>
    public void ThrowGranade();
}
