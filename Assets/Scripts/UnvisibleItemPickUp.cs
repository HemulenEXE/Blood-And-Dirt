using System;
using UnityEngine;

/// <summary>
/// Класс, реализующий "интерактивные объекты, над которыми не высвечивается текст и которые можно взять в инвентарь".
/// </summary>
public class UnvisibleItemPickUp : MonoBehaviour, IInteractiveObject
{
    /// <summary>
    /// Главная камера.
    /// </summary>
    public static Camera _mainCamera;
    /// <summary>
    /// Тег сущности, которая может взаимодействовать с данным объектом.
    /// </summary>
    public static string _targetTag = "Player";
    /// <summary>
    /// Коллайдер объекта.
    /// </summary>
    private CircleCollider2D _collider;
    /// <summary>
    /// Возвращает и изменяет коллайдер объекта.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    protected CircleCollider2D Collider
    {
        get => _collider;
        set
        {
            if (value.Equals(null)) throw new ArgumentNullException("UnvisibleItemPickUp: Collider is null");
            _collider = value;
        }
    }
    /// <summary>
    /// Инвентарная иконка предмета.
    /// </summary>
    [SerializeField] protected Sprite _icon;
    /// <summary>
    /// Возвращает и изменяет инвентарную иконку предмета.
    /// </summary>
    public Sprite Icon
    {
        get => _icon; protected set
        {
            if (value.Equals(null)) throw new ArgumentNullException("VisibleItemPickUp: Icon is null");
            _icon = value;
        }
    }
    /// <summary>
    /// Кнопка взаимодействия с объектом.
    /// </summary>
    [SerializeField] private KeyCode _key = KeyCode.E;
    /// <summary>
    /// Возвращает и изменяет кнопку взаимодействия с объектом.
    /// </summary>
    public KeyCode Key { get => _key; set => _key = value; }
    /// <summary>
    /// Радиус поля взаимодейтсвия.
    /// </summary>
    [SerializeField] private float _radius = 5.0f;
    /// <summary>
    /// Возвращает и изменяет радиус поля взаимодействия.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public float Radius
    {
        get => _radius;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException("UnvisibleItemPickUp: Distance <= 0");
            _radius = value;
        }
    }
    /// <summary>
    /// Флаг, указывающий, находится ли предмет в руке сущности.
    /// </summary>
    private bool _inHand;
    /// <summary>
    /// Возвращает флаг, указывающий, находится ли предмет в руке сущности.
    /// </summary>
    public bool InHand { get => _inHand; set => _inHand = value; }
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    protected virtual void Awake()
    {
        //Проверка полей, настраиваемых в редакторе Unity.
        if (_radius <= 0) throw new ArgumentOutOfRangeException("UnvisibleClickedObject: _radius <= 0");

        //Установка значений ненастраиваемых полей.
        Collider = this.GetComponent<CircleCollider2D>();
        _collider.radius = Radius;
        _collider.isTrigger = true;

        _mainCamera = Camera.main;
        if (_mainCamera == null) throw new ArgumentNullException("UnvisibleClickedObject: _mainCamera is null");
    }
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