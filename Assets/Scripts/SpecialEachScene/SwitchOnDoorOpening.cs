using UnityEngine;

//Переход на новую сцену после прохода в новую комнату
public class SwitchOnDoorOpening : SwitchScene
{
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Door door = this.transform.parent.GetComponentInChildren<Door>();
        if (door.IsOpen)
            Switch();
    }
}
