
using UnityEngine;
using Gun;
public class PlayerShooting : MonoBehaviour
{
    private IGun _gun;
    private void Update()
    {
        _gun = PlayerInventory._slots[PlayerInventory._currentSlot]?.StoredItem?.GetComponent<IGun>();
        if (_gun != null)
        {
            if (Input.GetKey(KeyCode.Mouse0))
                _gun.Shoot();
            else _gun.StopShoot();
            if (Input.GetKey(KeyCode.R))
                _gun.Recharge();
        }
    }
}
