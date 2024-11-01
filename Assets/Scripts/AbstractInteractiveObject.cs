using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// ����������� ����� �������������� �������
/// </summary>
public abstract class AbstractInteractiveObject : MonoBehaviour
{
    private CircleCollider2D _collider;
    /// <summary>
    /// �����, ������� ������������� ��� ��������, ����� ����� �������� � ���� ���������� ������
    /// </summary>
    private TextMeshProUGUI _description;
    /// <summary>
    /// ������, ��� ������� �� ������� ���������� �������������� � ��������
    /// </summary>
    [SerializeField] private KeyCode _key = KeyCode.E;
    /// <summary>
    /// �������� ���� ������������� ������ (��� ����������)
    /// </summary>
    [SerializeField] private string _fontType = "PixelFont";
    /// <summary>
    /// ������ ������
    /// </summary>
    [SerializeField] private float _fontSize = 40.0f;
    /// <summary>
    /// ������ ���� �������������� _collider
    /// </summary>
    [SerializeField] private float _distance = 5.0f;
    protected virtual void Start()
    {
        //��������� ���� ��������������
        _collider = this.GetComponent<CircleCollider2D>();
        if (_collider == null) throw new ArgumentNullException("AbstractInteractiveObject: _collider is null");
        _collider.radius = _distance;
        _collider.isTrigger = true;

        //��������� ������������� ������
        TMP_FontAsset loadedFont = Resources.Load<TMP_FontAsset>($"Fonts/{_fontType}"); //�������� ������ �� ����� Resources/Fonts
        if (loadedFont == null) throw new ArgumentNullException("AbstractInteractiveObject: loadedFont is null");

        //��������� �������� �������
        _description = new GameObject("TextMeshProUGUI").AddComponent<TextMeshProUGUI>();
        _description.gameObject.SetActive(false);
        _description.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform.Find("InteractiveUI"), false); //��������� ����� � ��������
        _description.fontSize = _fontSize;
        _description.text = $"Press {_key}";
        _description.font = loadedFont;
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player")) //������ ����� ����� ����������������� � �������������� ���������
        {
            _description.gameObject?.SetActive(true);
        }
    }
    /// <summary>
    /// ������������ ������������ � ��������, ������� ��� Player.
    /// ���� ����� ��������� � ���� �������������� � ������ �������� � ����� �� ������ _key, �� ���������� �������������� � ���� ��������
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player")) //������ ����� ����� ����������������� � �������������� ���������
        {
            if (Input.GetKey(_key)) //�������������� ���������� ��� ������� �� ������
            {
                Interact();
            }
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player")) //������ ����� ����� ����������������� � �������������� ���������
        {
            _description.gameObject?.SetActive(false);
        }
    }
    /// <summary>
    /// �������������� � ��������
    /// </summary>
    public abstract void Interact();
}
