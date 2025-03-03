using GunLogic;
using InventoryLogic;
using UnityEngine;
using System;
using SkillLogic;

namespace PlayerLogic
{
    public class PlayerKnife : MonoBehaviour
    {
        /// <summary>
        /// Взятый нож.
        /// </summary>
        private Knife _knife;
        /// <summary>
        /// Компонент, управляющий аудио.
        /// </summary>
        private AudioSource _audioControl;
        /// <summary>
        /// Время следующей аттаки.
        /// </summary>
        private float _nextAttackTime;

        //Встроенные методы.

        /// <summary>
        /// Настройка и проверка полейю
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private void Awake()
        {

            _audioControl = this.GetComponent<AudioSource>();
            if (_audioControl == null) throw new ArgumentNullException("PlayerKnife: _audioControl is null");
        }
        private void Update()
        {
            _knife = Inventory.GetInstance.CurrentSlot.StoredItem?.GetComponent<Knife>();
            if (_knife != null)
            {
                if (Input.GetKey(KeyCode.Mouse0) && _nextAttackTime <= 0)
                {
                    _audioControl.PlayOneShot(_knife.AttackSound);
                    //if (!PlayerInfo._isFighting && PlayerInfo.HasSkill<AnyPrice>())
                    //    _knife.InstantKill();
                    _knife.DealDamage();
                    _nextAttackTime = _knife.AttackDelay;
                }
            }
            _nextAttackTime -= Time.deltaTime;
        }
    }
}
