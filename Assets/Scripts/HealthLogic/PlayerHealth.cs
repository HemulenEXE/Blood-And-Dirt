using GunLogic;
using System.Collections;
using UnityEngine;
using System;
using CameraLogic.CameraEffects;
using InventoryLogic;
using SkillLogic;

namespace PlayerLogic
{
    /// <summary>
    /// Класс здоровья игрока. Скрипт навешивается на игрока
    /// </summary>
    public class PlayerHealth : AbstractHealth
    {
        // Перенёс данные в класс PlayerInfo


        [SerializeField]
        private float _frameDuration = 0.5f; //Сколько длится кадр, во время которого игрок не получает урон

        private BloodEffect bloodController;
        private float _updateStateTime = 10f; // Время обновления состояния игрока
        private float _currentTime = 0f;
        private int _hitsToSurvive;

        private void Start()
        {
            //PlayerInfo.ExecuteSkill("StartOfANewLife", this.gameObject);
            //PlayerInfo.ExecuteSkill("MusclesSecondSkeleton", this.gameObject);

            maxHealth = (int)PlayerInfo._fullHealth;
            currentHealth = (int)PlayerInfo._currentHealth;

            bloodController = GetComponent<BloodEffect>();
            StartCoroutine(BleedDamage());
        }

        private void Update()
        {
            if (_currentTime <= 0)
            {
                _currentTime = _updateStateTime;
                _hitsToSurvive = PlayerInfo._hitsToSurvive;
            }


            if (Input.GetKeyDown(KeyCode.Alpha3) && ConsumableCounter._firstAidKitCount > 0)
            {
                if (currentHealth + PlayerInfo._firstAidKitHealth > maxHealth)
                    currentHealth = maxHealth;
                else currentHealth += PlayerInfo._firstAidKitHealth;

                ConsumableCounter._firstAidKitCount--;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && ConsumableCounter._bandageCount > 0)
            {
                Debug.Log("Применены бинты!");
                if (currentHealth + PlayerInfo._bandageHealth > maxHealth)
                    currentHealth = maxHealth;
                else currentHealth += PlayerInfo._bandageHealth;

                ConsumableCounter._bandageCount--;
            }
            isInvulnerable = PlayerInfo._isGod;
        }
        void FixedUpdate()
        {
            // Debug.Log("XP = " + currentHealth);
            bloodController?.SetBloodEffect(CalculateStateDamaged());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Projectile")
            {
                if (_hitsToSurvive > 0) --_hitsToSurvive;
                else
                {
                    var dataBullet = collision.gameObject.GetComponent<ProjectileData>();
                    GetDamage(dataBullet);
                }
            }
        }

        /// <summary>
        /// Реализует получение урона от снаряда
        /// </summary>
        /// <param name="bullet"></param>
        public override void GetDamage(ProjectileData bullet)
        {
            if (!isInvulnerable)
            {
                PlayerInfo._isBleeding = true;
                currentHealth -= (int)bullet.Damage;

                if (currentHealth <= 0)
                {
                    Death();
                    return;
                }

                StartCoroutine(InvulnerabilityFrames(_frameDuration));
            }
        }

        private IEnumerator BleedDamage()
        {
            while (true)
            {
                if (PlayerInfo._isBleeding && !isInvulnerable)
                {
                    currentHealth -= PlayerInfo._bleedingDamage;
                }
                yield return new WaitForSeconds(60f);
            }

        }
        /// <summary>
        /// Высчитывает текущее состояние эффекта крови
        /// </summary>
        /// <returns></returns>
        StateBloodEffect CalculateStateDamaged()
        {
            // print($"Damaged = {Math.Floor((maxHealth - currentHealth) / (maxHealth / 5.0))}");
            return (StateBloodEffect)Math.Floor((maxHealth - currentHealth) / (maxHealth / 5.0));
        }
        public override void Death()
        {
            if (PlayerInfo._hitsToSurvive <= 0) Destroy(this.gameObject);
            else
            {
                --PlayerInfo._hitsToSurvive;
                PlayerInfo.GetSkill<Reincarnation>()?.SpawnBody(this.gameObject);
            }
        }
    }

}

