using GunLogic;
using InventoryLogic;
using System;
using TMPro;
using UnityEngine;

namespace PlayerLogic
{
    /// <summary>
    /// Класс, реализующий "механику стрельбы игроком".
    /// </summary>
    public class PlayerShooting : MonoBehaviour
    {
        //Поля.

        /// <summary>
        /// Текущее ружьё.
        /// </summary>
        private IGun _gun;
        private Side _sideplayer;
        private void Awake()
        {
            _sideplayer = GetComponent<Side>();
        }
        /// <summary>
        /// Время следующей аттаки.
        /// </summary>
        private float _nextAttackTime;

        //Встроенные методы.

        private void Update()
        {
            _gun = Inventory.GetInstance.CurrentSlot.StoredItem?.GetComponent<IGun>();
            if (_gun != null)
            {
                if (Input.GetKey(KeyCode.Mouse0) && _nextAttackTime <= 0)
                {
                    _nextAttackTime = _gun.ShotDelay;
                    _gun.Shoot(_sideplayer, IsPlayerShoot:true);
                }
                if (Input.GetKey(KeyCode.R))
                {
                    _gun.Recharge();
                }
                GameObject discription = Inventory.GetInstance.CurrentSlot.transform.GetChild(0).gameObject;
                discription.GetComponent<TextMeshProUGUI>().text = _gun.AmmoTotalCurrent + "\\" + _gun.AmmoTotal;
            }
            _nextAttackTime -= Time.deltaTime;
        }
    }
}