using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Класс, реализующий "интерактивные объекты".<br/>
/// Над такими объектами высвечивается интерактивный текст, но их нельзя взять в инвентарь.
/// </summary>
public class VisibleClickedObject : UnvisibleClickedObject
{
    /// <summary>
    /// Компонент, отвечающий за визуальное представление объекта.
    /// </summary>
    protected Renderer _renderer;
    /// <summary>
    /// Всплывающий над интерактивным объектом текст.
    /// </summary>
    private TextMeshProUGUI _description;
    /// <summary>
    /// Возвращает всплывающий над интерактивным объектом текст.
    /// </summary>
    public TextMeshProUGUI Description { get => _description; }
    /// <summary>
    /// Название шрифта интерактивного текста.
    /// </summary>
    [SerializeField] private string _fontName = "PixelFont";
    /// <summary>
    /// Возвращает название шрифта интерактивного текста.
    /// </summary>
    public string FontName { get => _fontName; }
    /// <summary>
    /// Размер шрифта.
    /// </summary>
    [SerializeField] private float _fontSize = 40f;
    /// <summary>
    /// Возвращает размер шрифта интерактивного текста.
    /// </summary>
    public float FontSize { get => _fontSize; }
    /// <summary>
    /// Вертикальное смещение всплывающего текста.
    /// </summary>
    [SerializeField] private float _offSet = 0.5f;
    /// <summary>
    /// Возвращает вертикальное смещение всплывающего текста.
    /// </summary>
    public float OffSet { get => _offSet; }
    /// <summary>
    /// Настройка и проверка полей.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    protected override void Awake()
    {
        base.Awake();

        _renderer = this.GetComponent<Renderer>();
        TMP_FontAsset loadedFont = Resources.Load<TMP_FontAsset>($"Fonts/{_fontName}"); //Загрузка шрифта.
        Transform position = GameObject.FindGameObjectWithTag("Canvas")?.transform?.Find("InteractiveUI");

        if (_renderer == null) throw new ArgumentNullException("VisibleClickedObject: _renderer is null");
        if (loadedFont == null) throw new ArgumentNullException("VisibleClickedObject: loadedFont is null");
        if (position == null) throw new ArgumentNullException("VisibleClickedObject: Scene hasn't Canvas or InteractiveUI");
        if (FontSize <= 0) throw new ArgumentOutOfRangeException("VisibleClickedObject: _fontSize <= 0");
        if (FontName == null) throw new ArgumentOutOfRangeException("VisibleClickedObject: FontName is null");
        if (FontName.Length.Equals(0)) throw new ArgumentOutOfRangeException("VisibleClickedObject: Length of FontName is 0");

        _description = new GameObject($"TMPUGUI {this.name}").AddComponent<TextMeshProUGUI>();
        Description.gameObject.SetActive(false);
        Description.transform.SetParent(position, false); //Установка связи с канвасом.
        Description.fontSize = _fontSize;
        Description.text = Key.ToString();
        Description.font = loadedFont;
        Description.alignment = TextAlignmentOptions.Center;
    }
    /// <summary>
    /// Появление интерактивного текста.
    /// </summary>
    /// <param name="other"></param>
    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.CompareTag(_targetTag))
        {
            if (Description != null && !Description.gameObject.activeInHierarchy) //Не работает операция ?
            {
                Description.gameObject.SetActive(true);
            }
            Vector3 positionObject = this.transform.position;
            positionObject.y = _renderer.bounds.max.y + OffSet; //Получение верхней границы визуального представления объекта.
            Vector3 positionInWorld = RectTransformUtility.WorldToScreenPoint(_mainCamera, positionObject);
            Description.transform.position = positionInWorld;

            if (Input.GetKey(Key)) //Взаимодействие происходит при нажатии на кнопку.
            {
                Interact();
            }
        }
    }
    /// <summary>
    /// Скрытие интерактивного текста.
    /// </summary>
    /// <param name="other"></param>
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag(_targetTag))
        {
            if (Description != null && Description.gameObject.activeInHierarchy) //Не работает операция ?
            {
                Description.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Взаимодействие с объектом.
    /// </summary>
    public override void Interact()
    {
        Debug.Log("VisibleClickedObject");
    }
}
