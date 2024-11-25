using Guns;
using UnityEngine;

namespace PlayerLogic
{
    public class PlayerKnife : MonoBehaviour
    {
        /// <summary>
        /// Взятое ружьё.
        /// </summary>
        private Knife _knife;
        private void Update()
        {
            _knife = PlayerInventory._slots[PlayerInventory._currentSlot]?.StoredItem?.GetComponent<Knife>();
            if (_knife != null)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    _knife.DealDamage();
                }
            }
        }
    }
}
