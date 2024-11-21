using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Класс, реализующий "интерактивные предметы".<br/>
/// Над такими предметами высвечивается интерактивный текст и их можно взять в инвентарь.
/// </summary>
public class VisibleItemPickUp : UnvisibleItemPickUp
{
    /// <summary>
    /// Компонент, отвечающий за визуальное представление интерактивного предмета.
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
    /// Размер шрифта интерактивного текста.
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
    public float OffSet {  get => _offSet; }
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

        if (position == null) throw new ArgumentNullException("VisibleItemPickUp: Scene hasn't Canvas or InteractiveUI");
        if (loadedFont == null) throw new ArgumentNullException("VisibleItemPickUp: loadedFont is null");
        if (_renderer == null) throw new ArgumentNullException("VisibleItemPickUp: _renderer is null");
        if (FontSize <= 0) throw new ArgumentOutOfRangeException("VisibleItemPickUp: FontSize <= 0");
        if (FontName == null) throw new ArgumentNullException("VisibleItemPickUp: FontName is null");
        if (FontName.Length.Equals(0)) throw new ArgumentOutOfRangeException("VisibleItemPickUp: Length of FontName is 0");

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
        if (other != null && !InHand && other.gameObject.CompareTag(_targetTag))
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
        if ((other != null && other.gameObject.CompareTag(_targetTag)))
        {
            if (Description != null && Description.gameObject.activeInHierarchy) //Не работает операция ?
            {
                Description.gameObject.SetActive(false);
            }
        }
    }
}
