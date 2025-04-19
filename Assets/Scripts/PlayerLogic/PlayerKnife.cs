using System;
using UnityEngine;

public class PlayerKnife : MonoBehaviour
{
    private Knife _knife; // Нож в руке
    private float _nextAttackTime;

    private InventoryAndConsumableCounterUI _inventoryAndConsumableCounterUI;

    private void Start()
    {
        _inventoryAndConsumableCounterUI = GameObject.FindAnyObjectByType<InventoryAndConsumableCounterUI>();

        if (_inventoryAndConsumableCounterUI == null) throw new ArgumentNullException("PlayerKnife: _inventoryAndConsumableCounterUI is null");

    }
    private void Update()
    {
        _knife = _inventoryAndConsumableCounterUI.GetItem()?.GetComponent<Knife>();
        if (_knife != null)
        {
            if (Input.GetKey(KeyCode.Mouse0) && _nextAttackTime <= 0)
            {
                //if (!PlayerInfo._isFighting && PlayerInfo.HasSkill<AnyPrice>())
                //    _knife.InstantKill();
                _knife.DealDamage();
                _nextAttackTime = _knife.AttackDelay;
            }
        }
        _nextAttackTime -= Time.deltaTime;
    }
}
