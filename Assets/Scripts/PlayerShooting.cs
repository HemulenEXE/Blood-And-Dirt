using UnityEngine;

//TODO.

/// <summary>
/// Скрипт, реализующий выстрел из оружия игроком.
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    /// <summary>
    /// Оружие в руке игрока.
    /// </summary>
    private GameObject _gun;
    private void Update()
    {
        _gun = PlayerInventory._slots[PlayerInventory._currSlot].StoredItem;
        if (_gun != null)
        {
            var shotgun = _gun.GetComponent<ShotGun>();
            if (shotgun != null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                shotgun.Shoot();
            }
            var flameThrower = _gun.GetComponent<FlameThrower>();
            if (flameThrower != null)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0)) flameThrower.StartFiring();
                if (Input.GetKeyUp(KeyCode.Mouse0)) flameThrower.StopFiring();
            }
        }
        //ДА, НАПИСАНО УЖАСНО, ПОТОМ ИСПРАВЛЮ!!! ЭТО ДЛЯ ДЕМОНСТРАЦИИ РАБОТЫ КОДА!!!
    }
}
