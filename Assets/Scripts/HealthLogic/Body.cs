using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 10;

    [SerializeField]
    private float _currentHealth = 10;

    public AudioClip _eatingProcess;
    public AudioClip _eatingFinish;

    public void GetDamage(float damage)
    {
        this.GetComponent<AudioSource>().PlayOneShot(_eatingProcess);
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            this.GetComponent<AudioSource>().PlayOneShot(_eatingFinish);
            Destroy(gameObject, _eatingFinish.length);
        }
    }
}
