using UnityEngine;

public class Body : ClickedObject
{
    private int _maxHealth = 3;
    private int _currentHealth = 3;
    private int _healthBoost = 10;

    [SerializeField]
    private AudioClip _eating;
    [SerializeField]
    private AudioClip _eatingFinish;
    // Ёти пол€ настроены у префаба

    public override void Interact()
    {
        if (!PlayerData.HasSkill<LiveInNotVain>()) return;

        _currentHealth -= 1;
        if (PlayerData.CurrentHealth + _healthBoost > PlayerData.MaxHealth) PlayerData.CurrentHealth = PlayerData.MaxHealth;
        else PlayerData.CurrentHealth += _healthBoost;

        if (_currentHealth <= 0)
        {
            Destroy(this.gameObject.GetComponent<Collider2D>()); // „тобы с трупом нельз€ было дальше взаимодействовать
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(_eatingFinish);
            Destroy(gameObject, _eatingFinish.length);
        }
        else this.gameObject.GetComponent<AudioSource>().PlayOneShot(_eating);

    }
}
