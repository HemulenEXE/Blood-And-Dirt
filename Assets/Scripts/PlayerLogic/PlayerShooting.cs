using TMPro;
using UnityEngine;
using GunLogic;

namespace PlayerLogic
{
    /// <summary>
    /// Класс, реализующий "стрельбу игроком".
    /// </summary>
    public class PlayerShooting : MonoBehaviour
    {
        /// <summary>
        /// Текущее выбранное ружьё.
        /// </summary>
        private IGun _gun;
        private void Update()
        {
            _gun = PlayerInventory._slots[PlayerInventory._currentSlot]?.StoredItem?.GetComponent<IGun>();
            if (_gun != null)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    _gun.Shoot();
                    //Изменение показателя кол-ва потронов над ячейкой инвентаря
                    GameObject discription = PlayerInventory._slots[PlayerInventory._currentSlot].transform.GetChild(0).gameObject;
                    discription.GetComponent<TextMeshProUGUI>().text = _gun.AmmoTotalCurrent + "\\" + _gun.AmmoTotal;
                }
                else _gun.StopShoot();
                if (Input.GetKey(KeyCode.R))
                {
                    _gun.Recharge();
                    GameObject discription = PlayerInventory._slots[PlayerInventory._currentSlot].transform.GetChild(0).gameObject;
                    discription.GetComponent<TextMeshProUGUI>().text = _gun.AmmoTotalCurrent + "\\" + _gun.AmmoTotal;
                }
            }
        }
    }

}