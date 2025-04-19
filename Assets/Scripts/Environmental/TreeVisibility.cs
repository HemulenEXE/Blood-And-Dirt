using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;   
using System;

public class TreeVisibility : MonoBehaviour
{
    //наш тайлмап, видимость которого мы меняем(привязка к скрипту автоматическая)
    Tilemap tilemap;

    //цвет, на который меняется тайлмап, если мы сталкиваемся с его триггером
    public Color SettedColor;

    //цвет исходный, если мы выходим из триггера
    Color initialColor;
    
    public float durationTime;
    void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        //фиксация исходного цвета
        initialColor = tilemap.color;
    }

    
    //входим в триггер
    void OnTriggerEnter2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            StartCoroutine(SetTilemapColor(tilemap, SettedColor, durationTime));
        }
    }

    //выходим из триггера
    void OnTriggerExit2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            StartCoroutine(SetTilemapColor(tilemap, initialColor, durationTime));
        }
    }

    
/// <summary>
/// меняет цвет тайловой карты tilemap на новый цвет color за duration секунд
/// </summary>
/// <param name="tilemap"></param> 
/// <param name="color"></param>
/// <param name="duration"></param>
    IEnumerator SetTilemapColor(Tilemap tilemap, Color color, float duration)
    {
        int count = Convert.ToInt32(duration*60);
        float deltaRed = (color.r - tilemap.color.r) / count;
        float deltaGreen = (color.g - tilemap.color.g) / count;
        float deltaBlue = (color.b - tilemap.color.b) / count;
        float deltaAlpha = (color.a - tilemap.color.a) / count;

        Color deltaColor = new Vector4(deltaRed, deltaGreen, deltaBlue, deltaAlpha);

        for (int i = 0; i < count; i++)
        {
            tilemap.color += deltaColor;
            yield return new WaitForSeconds(duration/60);
        }
        StopCoroutine(SetTilemapColor(tilemap, new Color(1, 1, 1, 1), 1f));        
    }
}
