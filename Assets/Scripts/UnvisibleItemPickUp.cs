using System;
using TMPro;
using UnityEngine;

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
    /// Компонент, отвечающий за визуальное представление интерактивного объекта.
    /// </summary>
    private Renderer _renderer;
    /// <summary>
    /// Возвращает и изменяет компонент, отвечающий за визуальное представление интерактивного объекта.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    protected Renderer Renderer
    {
        get => _renderer;
        set
        {
            if (value.Equals(null)) throw new ArgumentNullException("ClickedObject: Collider is null");
            _renderer = value;
        }
    }
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
            if (value.Equals(null)) throw new ArgumentNullException("ClickedObject: Collider is null");
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
            if (value.Equals(null)) throw new ArgumentNullException("ClickedObject: Icon is null");
            _icon = value;
        }
    }
    /// <summary>
    /// Всплывающий над интерактивным объектом текст.
    /// </summary>
    private TextMeshProUGUI _description;
    /// <summary>
    /// Возвращает и изменяет всплывающий над интерактивным объектом текст.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public TextMeshProUGUI Description
    {
        get => _description;
        set
        {
            //if (value.text.Equals(null)) throw new ArgumentNullException("ClickedObject: Text of Description is null");
            //if (value.text.Length.Equals(0)) throw new ArgumentOutOfRangeException("ClickedObject: Length of Description is 0");
            _description = value;
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
    /// Название шрифта для интерактивного текста.
    /// </summary>
    [SerializeField] private string _fontName = "PixelFont";
    /// <summary>
    /// Возвращает и изменяет название шрифта для интерактивного текста.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public string FontName
    {
        get => _fontName;
        set
        {
            if (value.Equals(null)) throw new ArgumentNullException("ClickedObject: FontName is null");
            if (value.Length.Equals(0)) throw new ArgumentOutOfRangeException("ClickedObject: Length of FontName is 0");
            _fontName = value;
        }
    }
    /// <summary>
    /// Размер шрифта.
    /// </summary>
    [SerializeField] private float _fontSize = 40f;
    /// <summary>
    /// Возвращает и изменяет размер шрифта.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public float FontSize
    {
        get => _fontSize;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException("ClickedObject: FontSize <= 0");
            _fontSize = value;
        }
    }
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
            if (value <= 0) throw new ArgumentOutOfRangeException("ClickedObject: Distance <= 0");
            _radius = value;
        }
    }
    /// <summary>
    /// Вертикальное смещение всплывающего текста.
    /// </summary>
    [SerializeField] private float _offSet = 0.5f;
    /// <summary>
    /// Возвращает и изменяет вертикальное смещение всплывающего текста.
    /// </summary>
    public float OffSet { get => _offSet; set => _offSet = value; }
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
        if (_fontSize <= 0) throw new ArgumentOutOfRangeException("AbstractInteractiveObject: _fontSize <= 0");
        if (_radius <= 0) throw new ArgumentOutOfRangeException("AbstractInteractiveObject: _radius <= 0");
        if (_fontName.Equals(0)) throw new ArgumentOutOfRangeException("AbstractInteractiveObject: Length of _fontType is 0");
        if (_fontName.Length.Equals(0)) throw new ArgumentOutOfRangeException("AbstractInteractiveObject: Length of _fontName is 0");

        //Установка значений ненастраиваемых полей.
        Collider = this.GetComponent<CircleCollider2D>();
        _collider.radius = Radius;
        _collider.isTrigger = true;

        TMP_FontAsset loadedFont = Resources.Load<TMP_FontAsset>($"Fonts/{_fontName}"); //Загрузка шрифта из папки Resources.
        if (loadedFont == null) throw new ArgumentNullException("AbstractInteractiveObject: loadedFont is null");
        Description = new GameObject($"TMPUGUI {this.name}").AddComponent<TextMeshProUGUI>();
        Description.gameObject.SetActive(false);
        Transform position = GameObject.FindGameObjectWithTag("Canvas")?.transform?.Find("InteractiveUI");
        if (position == null) throw new ArgumentNullException("AbstractInteractiveObject: Scene hasn't Canvas or InteractiveUI");
        Description.transform.SetParent(position, false); //Установка связи с канвасом.
        Description.fontSize = _fontSize;
        Description.text = _key.ToString();
        Description.font = loadedFont;
        Description.alignment = TextAlignmentOptions.Center;

        _mainCamera = Camera.main;
        if (_mainCamera == null) throw new ArgumentNullException("AbstractInteractiveObject: _mainCamera is null");

        Renderer = this.GetComponent<Renderer>();
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
    /// Более безопасный аналог метода SetActive(flag).
    /// </summary>
    public virtual void Deactive()
    {
        this.gameObject.SetActive(false);
    }
}