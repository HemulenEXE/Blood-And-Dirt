using GunLogic;
using InventoryLogic;
using System;
using System.Collections.Generic;
using UnityEngine;
using SkillLogic;

namespace PlayerLogic
{

    public class PlayerMotion : MonoBehaviour
    {
        // Перенёс данные в класс PlayerInfo

        private Camera _mainCamera;
        private Animator _animator;

        public static event Action<Transform, float> makeNoise;

        private Dictionary<float, float> noiseMapping;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _animator = this.transform.GetChild(0).GetComponent<Animator>();

            if (_mainCamera == null) throw new ArgumentNullException("PlayerMotion: _mainCamera is mull");
        }

        private void Update()
        {
            ControlAnimation();
        }

        private void FixedUpdate()
        {
            Move();
            Rotate();
        }   

        private void ControlAnimation()
        {
            _animator.SetBool("IsMoving", PlayerInfo._isWalking);
            var currentItem = Inventory.GetInstance?.CurrentSlot?.StoredItem;
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

        /// <summary>
        /// Передвижение игрока посредством клавиш WASD.
        /// </summary>
        private void Move()
        {
            PlayerInfo._isRunning = Input.GetKey(KeyCode.LeftShift);
            PlayerInfo._isStealing = Input.GetKey(KeyCode.LeftControl);

            if (PlayerInfo.HasSkill<Hatred>() && PlayerInfo._isBleeding) PlayerInfo._isRunning = true;

            // PlayerInfo.ExecuteSkill("Hatred", this.gameObject);
            float speedCurrent = PlayerInfo._isStealing ? PlayerInfo._stealSpeed : (PlayerInfo._isRunning ? PlayerInfo._runSpeed : PlayerInfo._walkSpeed);
            Vector3 movement = Vector2.zero;

            if (Input.GetKey(KeyCode.A))
            {
                movement += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement += Vector3.right;
            }
            if (Input.GetKey(KeyCode.W))
            {
                movement += Vector3.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement += Vector3.down;
            }
            if (movement != Vector3.zero)
            {
                PlayerInfo._isWalking = true;
                PlayerInfo._isStaying = false;
                this.transform.position += movement.normalized * speedCurrent * Time.fixedDeltaTime;
                if (PlayerInfo._isStealing) makeNoise?.Invoke(this.transform, PlayerInfo._stealNoise);
                else if (PlayerInfo._isRunning) makeNoise?.Invoke(this.transform, PlayerInfo._runNoise);
                else if (PlayerInfo._isWalking) makeNoise?.Invoke(this.transform, PlayerInfo._walkNoise);
            }
            else
            {
                PlayerInfo._isStaying = true;
                PlayerInfo._isWalking = false;
            }
        }
        /// <summary>
        /// Поворот игрока за компьтерной мышью.
        /// </summary>
        private void Rotate()
        {
            // Вычисление положения компьютерной мыши в мировом пространстве
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

            Vector3 direction = mousePosition - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Преобразование радиан в градусы - равен 360 / (2 * pi)
            this.transform.rotation = Quaternion.Euler(Vector3.forward* angle);
        }

    }
}
    