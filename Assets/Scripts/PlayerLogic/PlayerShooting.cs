using GunLogic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private float _nextAttackTime;

    private void Update()
    {
        IGun _gun = GameObject.FindAnyObjectByType<InventoryAndConsumableCounterUI>().GetItem()?.gameObject?.GetComponent<IGun>();
        if (_gun != null)
        {
            if (Input.GetKey(KeyCode.Mouse0) && _nextAttackTime <= 0)
            {
                _nextAttackTime = _gun.ShotDelay;
                _gun.Shoot();
            }
            if (Input.GetKey(KeyCode.R)) _gun.Recharge();
        }
        _nextAttackTime -= Time.deltaTime;
    }
}