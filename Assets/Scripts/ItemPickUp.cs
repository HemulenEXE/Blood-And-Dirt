using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : AbstractInteractiveObject
{
    /// <summary>
    /// Взаимодействие с объектом
    /// </summary>
    public override void Interact()
    {
        Debug.Log("Interact!");
    }
}
