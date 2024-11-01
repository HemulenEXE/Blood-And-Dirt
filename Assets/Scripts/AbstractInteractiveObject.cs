using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// Абстрактный класс интерактивного объекта
/// </summary>
public abstract class AbstractInteractiveObject : MonoBehaviour
{
    private CircleCollider2D _collider;
    /// <summary>
    /// Текст, который высвечивается над объектом, когда игрок подходит к нему достаточно близко
    /// </summary>
    private TextMeshProUGUI _description;
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
    protected virtual void Start()
    {
        //Настройка поля взаимодействия
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
        _description.text = $"Press {_key}";
        _description.font = loadedFont;
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player")) //Только игрок может взаимодействовать с интерактивными объектами
        {
            _description.gameObject?.SetActive(true);
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
            _description.gameObject?.SetActive(false);
        }
    }
    /// <summary>
    /// Взаимодействие с объектом
    /// </summary>
    public abstract void Interact();
}
