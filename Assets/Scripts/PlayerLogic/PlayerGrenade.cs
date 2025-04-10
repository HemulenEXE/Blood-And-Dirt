﻿using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Класс, реализующий "управление броска гранаты игроком".
/// </summary>
public class PlayerGrenade : MonoBehaviour
{
    private Camera _camera;
    private ShrapnelGrenade _prefabSimpleGrenade;
    private SmokeGrenade _prefabSmokeGrenade;

    public void ThrowGranade(ShrapnelGrenade grenade)
    {
        StartCoroutine(CoroutineThrowGranade(grenade));
    }
    private IEnumerator CoroutineThrowGranade(ShrapnelGrenade grenade)
    {
        var current_grenade = Instantiate(grenade, this.transform.position, Quaternion.identity);

        Vector2 cursor = Input.mousePosition;
        cursor = _camera.ScreenToWorldPoint(cursor);

        Vector2 startPoint = this.transform.position;
        Vector2 endPoint = new Vector2(cursor.x, cursor.y); //Конечная позиция (Не сам курсор, а точка под ним на уровне игрока)
                                                            //float amplitude = Math.Abs(this.transform.position.y - cursor.y); //Амплитуда броска (Высота на которой находиться курсор по отношению к ироку)
        float elapsedTime = 0f;

        while (elapsedTime < grenade.timeToExplosion)
        {
            float t = elapsedTime / grenade.timeToExplosion; // Нормализуем время (Для того, чтобы отмерить как должны были за этот промежуток измениться координаты)
                                                             //float angle = Mathf.Lerp(0, Mathf.PI, t); // Линейная интерполяция угла от 0 до π (полукруг)

            float x = Mathf.Lerp(startPoint.x, endPoint.x, t);
            //float y = amplitude * Mathf.Sin(angle) + Mathf.Min(startPoint.y, endPoint.y); ; // По y передвигаемся по синусоиде (Минимальная координа по y для корректировки движения)
            float y = Mathf.Lerp(startPoint.y, endPoint.y, t);
            if (current_grenade != null && !current_grenade.IsActivated)
                current_grenade.transform.position = new Vector3(x, y, 0);
            else StopCoroutine(CoroutineThrowGranade(grenade));

            elapsedTime += UnityEngine.Time.deltaTime;

            yield return null; // Ждем следующего кадра
        }
    }

    private void Awake()
    {
        _camera = Camera.main;

        _prefabSimpleGrenade = Resources.Load<GameObject>("Prefabs/Weapons/SimpleGrenade").GetComponent<ShrapnelGrenade>();
        _prefabSmokeGrenade = Resources.Load<GameObject>("Prefabs/Weapons/SmokeGrenade").GetComponent<SmokeGrenade>();

        if (_camera == null) throw new ArgumentNullException("PlayerGrenade: _camera is null");
        if (_prefabSimpleGrenade == null) throw new ArgumentNullException("PlayerGrenade: _prefabSimpleGrenade is null");
        if (_prefabSmokeGrenade == null) throw new ArgumentNullException("PlayerGrenade: _prefabSmokeGrenade is null");
    }
    private void Update()
    {
        if (Input.GetKeyDown(SettingData.SimpleGrenade) && PlayerData.SimpleGrenadeCount > 0)
        {
            ThrowGranade(_prefabSimpleGrenade);
            --PlayerData.SimpleGrenadeCount;
        }
        if (Input.GetKeyDown(SettingData.SmokeGrenade) && PlayerData.SmokeGrenadeCount > 0)
        {
            ThrowGranade(_prefabSmokeGrenade);
            --PlayerData.SmokeGrenadeCount;
        }
    }
}