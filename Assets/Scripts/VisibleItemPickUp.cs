using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Класс, реализующий "интерактивные объекты, над которыми высвечивается текст и которые можно взять в инвентарь".
/// </summary>
public class VisibleItemPickUp : UnvisibleItemPickUp
{
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
            if (value.Equals(null)) throw new ArgumentNullException("VisibleItemPickUp: Collider is null");
            _renderer = value;
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
            _description = value;
        }
    }
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
            if (value.Equals(null)) throw new ArgumentNullException("VisibleItemPickUp: FontName is null");
            if (value.Length.Equals(0)) throw new ArgumentOutOfRangeException("VisibleItemPickUp: Length of FontName is 0");
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
            if (value <= 0) throw new ArgumentOutOfRangeException("VisibleItemPickUp: FontSize <= 0");
            _fontSize = value;
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
    protected override void Awake()
    {
        base.Awake();

        //Проверка полей, настраиваемых в редакторе Unity.
        if (_fontSize <= 0) throw new ArgumentOutOfRangeException("VisibleClickedObject: _fontSize <= 0");
        if (_fontName.Equals(0)) throw new ArgumentOutOfRangeException("VisibleClickedObject: Length of _fontType is 0");
        if (_fontName.Length.Equals(0)) throw new ArgumentOutOfRangeException("VisibleClickedObject: Length of _fontName is 0");

        //Установка значений ненастраиваемых полей.

        TMP_FontAsset loadedFont = Resources.Load<TMP_FontAsset>($"Fonts/{_fontName}"); //Загрузка шрифта из папки Resources.
        if (loadedFont == null) throw new ArgumentNullException("VisibleClickedObject: loadedFont is null");
        Description = new GameObject($"TMPUGUI {this.name}").AddComponent<TextMeshProUGUI>();
        Description.gameObject.SetActive(false);
        Transform position = GameObject.FindGameObjectWithTag("Canvas")?.transform?.Find("InteractiveUI");
        if (position == null) throw new ArgumentNullException("VisibleClickedObject: Scene hasn't Canvas or InteractiveUI");
        Description.transform.SetParent(position, false); //Установка связи с канвасом.
        Description.fontSize = _fontSize;
        Description.text = Key.ToString();
        Description.font = loadedFont;
        Description.alignment = TextAlignmentOptions.Center;

        Renderer = this.GetComponent<Renderer>();
    }
    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && !InHand && other.gameObject.CompareTag(_targetTag))
        {
            if (Description != null)
            {
                Description.gameObject.SetActive(true);
            }
            Vector3 positionObject = this.transform.position;
            positionObject.y = Renderer.bounds.max.y + OffSet; //Получение верхней границы визуального представления объекта.
            Vector3 positionInWorld = RectTransformUtility.WorldToScreenPoint(_mainCamera, positionObject);
            Description.transform.position = positionInWorld;

            if (Input.GetKey(Key)) //Взаимодействие происходит при нажатии на кнопку.
            {
                Interact();
            }
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if ((other != null && other.gameObject.CompareTag(_targetTag)))
        {
            if (Description != null)
            {
                Description.gameObject.SetActive(false);
            }
        }
    }
}
