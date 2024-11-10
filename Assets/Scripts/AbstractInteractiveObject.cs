using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Абстрактный класс интерактивного объекта
/// </summary>
public abstract class AbstractInteractiveObject : MonoBehaviour
{
    /// <summary>
    /// Компонент, отвечающий за визуальное представление интерактивного объекта.
    /// </summary>
    private Renderer _renderer;
    /// <summary>
    /// Коллайдер объекта.
    /// </summary>
    private CircleCollider2D _collider;
    /// <summary>
    /// Всплывающий над интерактивным объектом текст.
    /// </summary>
    private TextMeshProUGUI _description;
    /// <summary>
    /// Главная камера.
    /// </summary>
    private Camera _mainCamera;
    /// <summary>
    /// Кнопка взаимодействия игрока с объектом.
    /// </summary>
    [SerializeField] private KeyCode _key = KeyCode.E;
    /// <summary>
    /// Название используемого шрифта (без расширения).
    /// </summary>
    [SerializeField] private string _fontType = "PixelFont";
    /// <summary>
    /// Размер шрифта.
    /// </summary>
    [SerializeField] private float _fontSize = 40.0f;
    /// <summary>
    /// Радиус поля взаимодейтсвия.
    /// </summary>
    [SerializeField] private float _distance = 5.0f;
    /// <summary>
    /// Смещение по вертикали положения всплывающего текста относительно интерактивного объекта.
    /// </summary>
    [SerializeField] private float _offSet = 0.5f;
    protected virtual void Start()
    {
        if (_fontSize < 0) throw new ArgumentOutOfRangeException("AbstractInteractiveObject: _fontSize < 0");
        if (_distance < 0) throw new ArgumentOutOfRangeException("AbstractInteractiveObject: _distance < 0");
        if (_fontType.Length == 0) throw new ArgumentOutOfRangeException("AbstractInteractiveObject: Length of _fontType is 0");

        //Настройка поля взаимодействия с интерактивным объектом.
        _collider = this.GetComponent<CircleCollider2D>();
        if (_collider == null) throw new ArgumentNullException("AbstractInteractiveObject: _collider is null");
        _collider.radius = _distance;
        _collider.isTrigger = true;

        //Настройка описания объекта.
        TMP_FontAsset loadedFont = Resources.Load<TMP_FontAsset>($"Fonts/{_fontType}"); //Загрузка шрифта из папки Resources.
        if (loadedFont == null) throw new ArgumentNullException("AbstractInteractiveObject: loadedFont is null");
        _description = new GameObject("TMPUGUI").AddComponent<TextMeshProUGUI>();
        _description.gameObject.SetActive(false);
        Transform position = GameObject.FindGameObjectWithTag("Canvas")?.transform?.Find("InteractiveUI");
        if (position == null) throw new ArgumentNullException("AbstractInteractiveObject: Scene hasn't Canvas or InteractiveUI");
        _description.transform.SetParent(position, false); //Установка связи с канвасом.
        _description.fontSize = _fontSize;
        _description.text = _key.ToString();
        _description.font = loadedFont;
        _description.alignment = TextAlignmentOptions.Center;

        //Найстройка главной камеры.
        _mainCamera = Camera.main;
        if (_mainCamera == null) throw new ArgumentNullException("AbstractInteractiveObject: _mainCamera is null");

        //Настройка визуального представления интерактивного объекта.
        _renderer = this.GetComponent<Renderer>();
        if (_renderer == null) throw new ArgumentNullException("AbstractInteractiveObject: _renderer is null");
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player")) //Только игрок может взаимодействовать с интерактивным объектом.
        {
            if (_description != null)
            {
                _description.gameObject.SetActive(true);
                Vector3 positionObject = this.transform.position;
                positionObject.y = _renderer.bounds.max.y + _offSet; //Получение верхней границы визуального представления объекта.
                Vector3 positionInWorld = RectTransformUtility.WorldToScreenPoint(_mainCamera, positionObject);
                _description.transform.position = positionInWorld;
            }
        }
    }
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player")) //Только игрок может взаимодействовать с интерактивными объектами.
        {
            if (Input.GetKey(_key)) //Взаимодействие происходит при нажатии на кнопку.
            {
                Interact();
            }
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player")) //Только игрок может взаимодействовать с интерактивными объектами.
        {
            if (_description != null)
            {
                _description.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Взаимодействие с интерактивным объектом.
    /// </summary>
    public abstract void Interact();
}
