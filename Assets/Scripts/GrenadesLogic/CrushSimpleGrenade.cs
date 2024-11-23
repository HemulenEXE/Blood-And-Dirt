using UnityEngine;

/// <summary>
/// Фиксирует столкновение гранаты с другими обектами (Collodir2D Ridgitbody2D) и вызывает взрыв. Скрипт навешивается на префаб гранаты
/// </summary>
public class CrushSimpleGrenade : MonoBehaviour
{
    /// <summary>
    /// Когда граната должна взорваться
    /// </summary>
    public float TimeToStop { get; set; }
    private float _timer = 0;
    private Camera _camera;
    private GameObject _player;

    private void Start()
    {
        _camera = Camera.main;
        _player = GameObject.FindGameObjectWithTag("Player"); 
    }

    /// <summary>
    /// Если произошло столкновение с другим объектом
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.1f);
        Crash();
    }

    private void Update()
    {
        if (_timer >= TimeToStop)
        {
            Destroy(gameObject, 0.1f);
            Crash();
        }
        else _timer += Time.deltaTime;
    }
    /// <summary>
    /// Вызывает тряску после уничтожения гранаты 
    /// </summary>
    private void Crash()
    {
        float dist = Vector3.Distance(_player.transform.position, transform.position);

        //Тряска тем больше, чем ближе к игроку упала граната 
        if (dist <= 5) _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.6f);
        else if (dist <= 10) _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.3f);
        else _camera.GetComponent<ShakeEffect>().ShakeCamera(0.5f, 0.08f);
    }
}
