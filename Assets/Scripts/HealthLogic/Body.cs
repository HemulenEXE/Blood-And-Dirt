using UnityEngine;

public class Body : ClickedObject
{
    public int _maxHealth = 3;
    public int _currentHealth = 3;
    public int _healthBoost = 10;

    [SerializeField]
    private AudioClip _eating;
    [SerializeField]
    private AudioClip _eatingFinish;
    [SerializeField]
    private float _timeSecondsLife = 10; // Время жизни тела (нужно, чтобы не перегружать сцены трупами)
    // Эти поля настроены у префаба

    public override void Interact()
    {
        if (!PlayerData.HasSkill<LiveInNotVain>()) return;

        _currentHealth -= 1;
        if (PlayerData.CurrentHealth + _healthBoost > PlayerData.MaxHealth) PlayerData.CurrentHealth = PlayerData.MaxHealth;
        else PlayerData.CurrentHealth += _healthBoost;

        if (_currentHealth <= 0)
        {
            Destroy(this.gameObject.GetComponent<Collider2D>()); // Чтобы с трупом нельзя было дальше взаимодействовать
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(_eatingFinish);
            Destroy(gameObject, _eatingFinish.length);
        }
        else this.gameObject.GetComponent<AudioSource>().PlayOneShot(_eating);

    }

    public void Start()
    {
        Destroy(gameObject, _timeSecondsLife); 
    }
}
