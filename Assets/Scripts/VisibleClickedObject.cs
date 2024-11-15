using System;
using TMPro;
using UnityEngine;

public class VisibleClickedObject : UnvisibleClickedObject
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag(_targetTag))
        {
            Description.gameObject.SetActive(true);
            Vector3 positionObject = this.transform.position;
            positionObject.y = Renderer.bounds.max.y + OffSet; //Получение верхней границы визуального представления объекта.
            Vector3 positionInWorld = RectTransformUtility.WorldToScreenPoint(_mainCamera, positionObject);
            Description.transform.position = positionInWorld;
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.gameObject.CompareTag(_targetTag))
        {
            if (Description != null)
            {
                Description.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Взаимодействие с объектом.
    /// </summary>
    public override void Interact()
    {
        Debug.Log("Clicked");
    }
}
