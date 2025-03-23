using GunLogic;
using System.Collections;
using UnityEngine;
using System;
using CameraLogic.CameraEffects;
using InventoryLogic;
using SkillLogic;
using Grenades;

namespace PlayerLogic
{
    /// <summary>
    /// Класс здоровья игрока. Скрипт навешивается на игрока
    /// </summary>
    public class PlayerHealth : AbstractHealth
    {
        private float _frameDuration = 0.5f; // Сколько длится кадр, во время которого игрок не получает урон

        private BloodEffect bloodController;

        private float _updateStateTimeBleeding = 10f;
        private float _currentTimeBleeding = 0f;

        private int _currentHitsToSurvive;
        private float _updateStateTime = 10f; // Время обновления состояния игрока
        private float _currentTimeHits = 0f;

        private void Start()
        {
            // PlayerInfo.ExecuteSkill("StartOfANewLife", this.gameObject);
            // PlayerInfo.ExecuteSkill("MusclesSecondSkeleton", this.gameObject);

            bloodController = GetComponent<BloodEffect>();
            StartCoroutine(BleedDamage());
        }

        private void Update()
        {
            if (_currentTimeHits <= 0)
            {
                _currentTimeHits = _updateStateTime;
                _currentHitsToSurvive = PlayerInfo._hitsToSurvive; // Обновление количества пропускаемых ударов
            }
            _currentTimeHits -= Time.deltaTime;

            if (_currentTimeBleeding <= 0)
            {
                _currentTimeBleeding = _updateStateTimeBleeding;
                PlayerInfo._isBleeding = false; // Обновление состояния кровотечения
            }
            _currentTimeBleeding -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Alpha3) && ConsumableCounter._firstAidKitCount > 0)
            {
                if (PlayerInfo._currentHealth + PlayerInfo._firstAidKitHealth > PlayerInfo._fullHealth)
                    PlayerInfo._currentHealth = PlayerInfo._fullHealth;
                else PlayerInfo._currentHealth += PlayerInfo._firstAidKitHealth;

                ConsumableCounter._firstAidKitCount--;
            }

            if (Input.GetKeyDown(KeyCode.Alpha4) && ConsumableCounter._bandageCount > 0)
            {
                if (PlayerInfo._currentHealth + PlayerInfo._bandageHealth > PlayerInfo._fullHealth)
                    PlayerInfo._currentHealth = PlayerInfo._fullHealth;
                else PlayerInfo._currentHealth += PlayerInfo._bandageHealth;

                ConsumableCounter._bandageCount--;
            }
        }
        void FixedUpdate()
        {
            bloodController?.SetBloodEffect(CalculateStateDamaged());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Projectile")
            {
                if (_currentHitsToSurvive > 0)
                {
                    --_currentHitsToSurvive;
                    _currentTimeHits = _updateStateTime;
                }
                else
                {
                    var dataBullet = collision.gameObject.GetComponent<ProjectileData>();
                    GetDamage(dataBullet);
                }
            }
        }

        public override void GetDamage(ProjectileData bullet)
        {
            GetDamage((int)bullet.Damage);
        }

        public override void GetDamage(SimpleGrenade grenade)
        {
            GetDamage((int)grenade.DamageExplosion);
        }

        protected override void GetDamage(int damage)
        {
            if (!PlayerInfo._isGod)
            {
                PlayerInfo._isBleeding = true;
                PlayerInfo._currentHealth -= damage;
                _currentTimeBleeding = _updateStateTimeBleeding;

                if (PlayerInfo._currentHealth <= 0)
                {
                    var temp = PlayerInfo.GetSkill<Reincarnation>();
                    if (temp != null && PlayerInfo._bodyCount > 0)
                    {
                        temp.SpawnBody(this.gameObject); // Спавн трупа
                        PlayerInfo._fullHealth /= 2;
                        PlayerInfo._currentHealth = PlayerInfo._fullHealth;
                        return;
                    }
                    else
                    {
                        Death();
                        return;
                    }
                }
                StartCoroutine(InvulnerabilityFrames(_frameDuration));
            }
        }

        private IEnumerator BleedDamage()
        {
            while (true)
            {
                if (PlayerInfo._isBleeding && !PlayerInfo._isGod)
                {
                    PlayerInfo._currentHealth -= PlayerInfo._bleedingDamage;
                }
                yield return new WaitForSeconds(60f); // Что за 60f?
            }
        }
        /// <summary>
        /// Высчитывает текущее состояние эффекта крови
        /// </summary>
        /// <returns></returns>
        StateBloodEffect CalculateStateDamaged()
        {
            // print($"Damaged = {Math.Floor((maxHealth - currentHealth) / (maxHealth / 5.0))}");
            return (StateBloodEffect)Math.Floor((PlayerInfo._fullHealth - PlayerInfo._currentHealth) / (PlayerInfo._fullHealth / 5.0));
        }
    }
}

