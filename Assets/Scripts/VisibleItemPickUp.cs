using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleItemPickUp : UnvisibleItemPickUp
{
    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && !InHand && other.gameObject.CompareTag(_targetTag))
        {
            Description.gameObject.SetActive(true);
            Vector3 positionObject = this.transform.position;
            positionObject.y = Renderer.bounds.max.y + OffSet; //Получение верхней границы визуального представления объекта.
            Vector3 positionInWorld = RectTransformUtility.WorldToScreenPoint(_mainCamera, positionObject);
            Description.transform.position = positionInWorld;

            if (Input.GetKey(Key)) //Взаимодействие происходит при нажатии на кнопку.
            {
                Interact();
            }
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if ((other != null && other.gameObject.CompareTag(_targetTag)))
        {
            Description.gameObject.SetActive(false);
        }
    }
}
