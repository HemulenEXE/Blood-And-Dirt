using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private float _nextAttackTime;
    private Side _sideplayer;
    private void Awake()
    {
        _sideplayer = GetComponent<Side>();
    }
    private void Update()
    {
        IGun _gun = GameObject.FindAnyObjectByType<InventoryAndConsumableCounterUI>().GetItem()?.gameObject?.GetComponent<IGun>();
        if (_gun != null)
        {
            if (Input.GetKey(KeyCode.Mouse0) && _nextAttackTime <= 0)
            {
                _nextAttackTime = _gun.ShotDelay;
                _gun.Shoot(_sideplayer, IsPlayerShoot: true);
            }
            if (Input.GetKey(KeyCode.R)) _gun.Recharge();
        }
        _nextAttackTime -= Time.deltaTime;
    }
}