using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;   
using System;

public class TreeVisibility : MonoBehaviour
{
    // Start is called before the first frame update
    Tilemap tilemap;
    public float durationTime;
    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    

    void OnTriggerEnter2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            StartCoroutine(SetTilemapColor(tilemap, new Color(1, 1, 1, 0.4f), durationTime));
        }
    }

    void OnTriggerExit2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {

            StartCoroutine(SetTilemapColor(tilemap, new Color(1, 1, 1, 1), durationTime));
        }
    }

    
/// <summary>
/// меняет цвет тайловой карты на новый цвет за заданное число секунд
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
