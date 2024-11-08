using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class VisionCollider : MonoBehaviour
{
    [SerializeField]
    private float visionRange = 10f;

    private int maskVision;

    private Transform selfTransform;

    private void Awake()
    {
        selfTransform = GetComponent<Transform>();
        maskVision = LayerMask.GetMask("Player", "Default") & ~LayerMask.GetMask("Enemy");
    }

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            Transform positionPlayer = collider.GetComponent<Transform>();

            // Вектор направления от врага к игроку, нормализованный (единичной длины)
            Vector2 directionToPlayer = (positionPlayer.position - transform.position).normalized;

            // Запускаем Raycast от позиции врага в направлении игрока
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, Mathf.Infinity, maskVision);

            if (hit.collider != null)
            {
                // Проверяем, достиг ли луч игрока, не встречая препятствий
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    print("I see u!");
                }
                else
                {
                    print($"{hit.collider.gameObject.name}");
                }

            }
            else
            {
                print("null");
            }
        }
        
    }
}
