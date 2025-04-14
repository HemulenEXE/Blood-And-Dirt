using System;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

//Переход на новую сцену после прохода в новую комнату
public class SwitchOnDoorOpening : SwitchScene
{
    private Vector3 position; //Позиция игрока при следующем заходе на эту сцену
    private Door door; //Дверь, после открытия которой должен происходить переход
    private Quaternion rotate; //Поворот игрока при следующем заходе на эту сцену
    private bool saved = false; //Сохранена ли новая позиция
    private void Start()
    {
        door = this.transform.parent.GetComponentInChildren<Door>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Установка позиции и повортоа в зависимости от того к какой дери и с какой стороны подошёл игрок
            Transform player = collision.transform;
            if (door.Side == Door.SideOpen.Down || door.Side == Door.SideOpen.Up) 
            {
                if (door.PlayerSide == Door.ApproachSide.Right)
                {
                    Vector3 rot = player.eulerAngles;
                    rot.z = 0;
                    rotate = Quaternion.Euler(rot);
                    position = new Vector3(player.position.x + 0.5f, player.position.y, player.position.z);
                }
                else
                {
                    Vector3 rot = player.eulerAngles;
                    rot.z = 180;
                    rotate = Quaternion.Euler(rot);
                    position = new Vector3(player.position.x - 0.5f, player.position.y, player.position.z);
                }
            }
            else
            {
                if (door.PlayerSide == Door.ApproachSide.Up)
                {
                    Vector3 rot = player.eulerAngles;
                    rot.z = 90;
                    rotate = Quaternion.Euler(rot);
                    position = new Vector3(player.position.x, player.position.y + 0.5f, player.position.z);
                }
                else
                {
                    Vector3 rot = player.eulerAngles;
                    rot.z = 270;
                    rotate = Quaternion.Euler(rot);
                    position = new Vector3(player.position.x, player.position.y - 0.5f, player.position.z);
                }
            }
            // Само перемещение и запоминание положение игрока
            if (door.isRunning || door.IsOpen)
            {
                if (PlayerInitPosition.Instance != null && !saved)
                {
                    PlayerInitPosition.Instance.SavePosition(SceneManager.GetActiveScene().buildIndex, position, rotate);
                    saved = true;
                }
                Switch(); 
            }
        }
    }
 
}