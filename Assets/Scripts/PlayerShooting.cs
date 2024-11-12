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
    private AbstractGun _gun;
    private void Update()
    {
        _gun = PlayerInventory._slots[PlayerInventory._currSlot].StoredItem?.GetComponent<AbstractGun>();
        if (_gun != null)
        {
            if (Input.GetKey(KeyCode.Mouse0))
                _gun.Shoot();
            else 
                _gun.StopShoot();
        }
    }
}
