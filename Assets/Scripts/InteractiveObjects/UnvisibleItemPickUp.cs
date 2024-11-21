using System;
using UnityEngine;

/// <summary>
/// Класс, реализующий "интерактивные предметы".<br/>
/// Над такими предметами не высвечивается интерактивный текст, но их можно взять в инвентарь.
/// </summary>
public class UnvisibleItemPickUp : MonoBehaviour, IInteractiveObject
{
    /// <summary>
    /// Главная камера.
    /// </summary>
    public static Camera _mainCamera;
    /// <summary>
    /// Тег сущности, которая может взаимодействовать с предметом.
    /// </summary>
    protected string _targetTag = "Player";
    /// <summary>
    /// Коллайдер предмета.
    /// </summary>
    protected CircleCollider2D _collider;
    /// <summary>
    /// Инвентарная иконка предмета.
    /// </summary>
    [SerializeField] private Sprite _icon;
    /// <summary>
    /// Возвращает инвентарную иконку предмета.
    /// </summary>
    public Sprite Icon { get => _icon; }
    /// <summary>
    /// Кнопка взаимодействия с предметом.
    /// </summary>
    [SerializeField] private KeyCode _key = KeyCode.E;
    /// <summary>
    /// Возвращает кнопку взаимодействия с предметом.
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
    /// Возвращает и изменяет флаг, указывающий, находится ли предмет в руке сущности.
    /// </summary>
    public bool InHand { get; set; } = false;
    /// <summary>
    /// Настройка и проверка полей.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    protected virtual void Awake()
    {
        _mainCamera = Camera.main;
        _collider = this.GetComponent<CircleCollider2D>();

        if (_collider == null) throw new ArgumentNullException("UnvisibleItemPickUp: _collider is null");
        if (_targetTag == null) throw new ArgumentNullException("UnvisibleItemPickUp: _targetTag is null");
        if (_targetTag.Length.Equals(0)) throw new ArgumentOutOfRangeException("UnvisibleItemPickUp: Lenght of _targetTag is 0");
        if (Radius <= 0) throw new ArgumentOutOfRangeException("UnvisibleItemPickUp: Radius <= 0");
        if (Icon == null) throw new ArgumentOutOfRangeException("UnvisibleItemPickUp: Icon is null");
        if (_mainCamera == null) throw new ArgumentNullException("UnvisibleItemPickUp: _mainCamera is null");

        _collider.radius = Radius;
        _collider.isTrigger = true;
    }
    /// <summary>
    /// Вызов взаимодействия с предметом при нажатии на кнопку.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && !InHand && other.gameObject.CompareTag(_targetTag))
        {
            if (Input.GetKey(Key)) //Взаимодействие происходит при нажатии на кнопку.
            {
                Interact();
            }
        }
    }
    /// <summary>
    /// Взаимодействие с предметом.
    /// </summary>
    public virtual void Interact()
    {
        PlayerInventory._slots[PlayerInventory._currentSlot].AddItem(this);
    }
    /// <summary>
    /// Активирование предмета на сцене.<br/>
    /// Более безопасный аналог метода SetActive(true).
    /// </summary>
    public virtual void Active()
    {
        this.gameObject.SetActive(true);
    }
    /// <summary>
    /// Деактивирование предмета на сцене.<br/>
    /// Более безопасный аналог метода SetActive(false).
    /// </summary>
    public virtual void Deactive()
    {
        this.gameObject.SetActive(false);
    }
}