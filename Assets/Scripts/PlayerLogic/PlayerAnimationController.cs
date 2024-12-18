using GunLogic;
using InventoryLogic;
using PlayerLogic;
using UnityEngine;

namespace PlayerLogic
{
    /// <summary>
    /// Класс, реализующий "управление анимациями игрока".
    /// </summary>
    public class PlayerAnimationController : MonoBehaviour
    {
        /// <summary>
        /// Компонент, отвечающий за анимацю игрока.
        /// </summary>
        private Animator _animator;
        private void Awake()
        {
            _animator = this.transform.GetChild(0).GetComponent<Animator>();
        }
        private void Update()
        {
            _animator.SetBool("IsMoving", this.GetComponent<PlayerMotion>().IsMoving);
            var currentItem = Inventory.GetInstance.CurrentSlot.StoredItem;
            if (currentItem?.GetComponent<ShotGun>() != null)
            {
                _animator.SetBool("ShotGun", true);
            }
            else _animator.SetBool("ShotGun", false);
            if (currentItem?.GetComponent<MachineGun>() != null)
            {
                _animator.SetBool("MachineGun", true);
            }
            else _animator.SetBool("MachineGun", false);
            if (currentItem?.GetComponent<Pistol>() != null)
            {
                _animator.SetBool("Pistol", true);
            }
            else _animator.SetBool("Pistol", false);
            if (currentItem?.GetComponent<Knife>() != null)
            {
                _animator.SetBool("Knife", true);
            }
            else _animator.SetBool("Knife", false);
        }
    }
}
