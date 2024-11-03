using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

/// <summary>
/// Абстрактный класс интерактивного объекта
/// </summary>
public abstract class AbstractInteractiveObject : MonoBehaviour
{
    /// <summary>
    /// Компонент, отвечающий за визуальное представление этого объекта
    /// </summary>
    private Renderer _renderer;
    /// <summary>
    /// Коллайдер данного объекта
    /// </summary>
    private CircleCollider2D _collider;
    /// <summary>
    /// Текст, который высвечивается над данным объектом, когда игрок подходит к нему достаточно близко
    /// </summary>
    private TextMeshProUGUI _description;
    /// <summary>
    /// Главная камера
    /// </summary>
    private Camera mainCamera;
    /// <summary>
    /// Кнопка, при нажатии на которую происходит взаимодействие с объектом
    /// </summary>
    [SerializeField] private KeyCode _key = KeyCode.E;
    /// <summary>
    /// Название типа используемого шрифта (без расширения)
    /// </summary>
    [SerializeField] private string _fontType = "PixelFont";
    /// <summary>
    /// Размер шрифта
    /// </summary>
    [SerializeField] private float _fontSize = 40.0f;
    /// <summary>
    /// Радиус поля взаимодейтсвия _collider
    /// </summary>
    [SerializeField] private float _distance = 5.0f;
    /// <summary>
    /// Смещение по вертикали положения интерактивного текста относительно данного объекта
    /// </summary>
    [SerializeField] private float _offSet = 0.5f;
    protected virtual void Start()
    {
        //Настройка поля взаимодействия с интерактивным объектом
        _collider = this.GetComponent<CircleCollider2D>();
        if (_collider == null) throw new ArgumentNullException("AbstractInteractiveObject: _collider is null");
        _collider.radius = _distance;
        _collider.isTrigger = true;

        //Настройка используемого шрифта
        TMP_FontAsset loadedFont = Resources.Load<TMP_FontAsset>($"Fonts/{_fontType}"); //Загрузка шрифта из папки Resources/Fonts
        if (loadedFont == null) throw new ArgumentNullException("AbstractInteractiveObject: loadedFont is null");

        //Настройка описания объекта
        _description = new GameObject("TextMeshProUGUI").AddComponent<TextMeshProUGUI>();
        _description.gameObject.SetActive(false);
        _description.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.Find("InteractiveUI"), false); //Установка связи с канвасом
        _description.fontSize = _fontSize;
        _description.text = _key.ToString();
        _description.font = loadedFont;
        _description.alignment = TextAlignmentOptions.Center;

        //Найстройка главной камеры
        mainCamera = Camera.main;

        //Настройка визуального представления интерактивного объекта
        _renderer = this.GetComponent<Renderer>();
        if (_renderer == null) throw new ArgumentNullException("AbstractInteractiveObject: _renderer is null");
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player")) //Только игрок может взаимодействовать с интерактивными объектами
        {
            if (_description != null)
            {
                _description.gameObject.SetActive(true);
                Vector3 positionObject = this.transform.position;
                positionObject.y = _renderer.bounds.max.y + _offSet; //Получение верхней границы визуала интерактивного объекта
                Vector3 positionInWorld = RectTransformUtility.WorldToScreenPoint(mainCamera, positionObject);
                _description.transform.position = positionInWorld;
            }
        }
    }
    /// <summary>
    /// Обрабатывает столкновение с объектом, имеющем тег Player.
    /// Если игрок находится в поле взаимодействия с данным объектом и нажал на кнопку _key, то происходит взаимодействие с этим объектом
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player")) //Только игрок может взаимодействовать с интерактивными объектами
        {
            if (Input.GetKey(_key)) //Взаимодействие происходит при нажатии на кнопку
            {
                Interact();
            }
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player")) //Только игрок может взаимодействовать с интерактивными объектами
        {
            if (_description != null)
            {
                _description.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Взаимодействие с объектом
    /// </summary>
    public abstract void Interact();
}
