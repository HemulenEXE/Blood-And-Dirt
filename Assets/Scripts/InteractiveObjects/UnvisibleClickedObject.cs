using System;
using UnityEngine;

/// <summary>
/// Класс, реализующий "интерактивные объекты".<br/>
/// Над такими объектами не высвечивается интерактивный текст и их нельзя взять в инвентарь.
/// </summary>
public class UnvisibleClickedObject : MonoBehaviour, IInteractiveObject
{
    /// <summary>
    /// Главная камера.
    /// </summary>
    public static Camera _mainCamera;
    /// <summary>
    /// Тег сущности, которая может взаимодействовать с данным объектом.
    /// </summary>
    protected string _targetTag = "Player";
    /// <summary>
    /// Коллайдер объекта.
    /// </summary>
    protected CircleCollider2D _collider;
    /// <summary>
    /// Кнопка взаимодействия с объектом.
    /// </summary>
    [SerializeField] private KeyCode _key = KeyCode.E;
    /// <summary>
    /// Возвращает кнопку взаимодействия с объектом.
    /// </summary>
    public KeyCode Key { get => _key; }
    /// <summary>
    /// Радиус поля взаимодейтсвия.
    /// </summary>
    [SerializeField] private float _radius = 5.0f;
    /// <summary>
    /// Возвращает радиус поля взаимодействия.
    /// </summary>
    public float Radius { get => _radius; }
    /// <summary>
    /// Настройка и проверка полей.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    protected virtual void Awake()
    {
        _mainCamera = Camera.main;
        _collider = this.GetComponent<CircleCollider2D>();

        if (_mainCamera == null) throw new ArgumentNullException("UnvisibleClickedObject: _mainCamera is null");
        if (_collider == null) throw new ArgumentNullException("UnvisibleClickedObject: _collider is null");
        if (Radius <= 0) throw new ArgumentOutOfRangeException("UnvisibleClickedObject: _radius <= 0");

        _collider.radius = Radius;
        _collider.isTrigger = true;
    }
    /// <summary>
    /// Вызов взаимодействия с объектом при нажатии на кнопку.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag(_targetTag))
        {
            if (Input.GetKey(Key)) //Взаимодействие происходит при нажатии на кнопку.
            {
                Interact();
            }
        }
    }
    /// <summary>
    /// Взаимодействие с объектом.
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log("UnvisibleClickedObject");
    }
}
