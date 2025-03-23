
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput;

namespace GunLogic
{
    /// <summary>
    /// Тип оружия.
    /// </summary>
    public enum GunType
    {
        /// <summary>
        /// Лёгкое оружие.
        /// </summary>
        Light,
        /// <summary>
        /// Тяжёлое оружие.
        /// </summary>
        Heavy,
        /// <summary>
        /// Огненное оружие.
        /// </summary>
        Firebased,
        /// <summary>
        /// Взрывное оружие.
        /// </summary>
        Explosive,
        /// <summary>
        /// Холодное оружие.
        /// </summary>
        Cold,
        /// <summary>
        /// Токсичное оружие.
        /// </summary>
        Toxic
    }
    /// <summary>
    /// Интерфейс, поддерживающий "ружьё".<br/>
    /// Ружьё может быть как огнестрельным, так и распыляющим.
    /// </summary>
    public interface IGun
    {
        /// <summary>
        /// Возвращает тип оружия.
        /// </summary>
        public GunType Type { get; }
        /// <summary>
        /// Возвращает величину наносимого урона.
        /// </summary>
        public float Damage { get; }
        /// <summary>
        /// Возвращает и изменяет суммарное число снарядов.
        /// </summary>
        public int AmmoTotal { get; }
        /// <summary>
        /// Возвращает задержку между выстрелами.
        /// </summary>
        public float ShotDelay { get; }
        /// <summary>
        /// Возвращает вместимость очереди.
        /// </summary>
        public int AmmoCapacity { get; }
        /// <summary>
        /// Возвращает и изменяет текущее число снарядов в очереди.
        /// </summary>
        public int AmmoTotalCurrent { get; }
        /// <summary>
        /// Возвращает скорость вылета пули.
        /// </summary>
        public float SpeedProjectile { get; }
        /// <summary>
        /// Возвращает и изменяет дистанцию атаки оружия (в основном для ботов)
        /// </summary>
        public float AttackRange { get; set; }
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли перезарядка.
        /// </summary>
        public bool IsRecharging { get; set; }
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли стрельба.
        /// </summary>
        public bool IsShooting { get; set; }
        /// <summary>
        /// Возвращает и изменяет силу шума оружия при выстреле
        /// </summary>
        public float NoiseIntensity { get; set; }

        public bool IsHeld { get; set; }

        /// <summary>
        /// Событие вызова реакции на шум стрельбы
        /// </summary>
        public static event Action<Transform, float> makeNoiseShooting;
        /// <summary>
        /// Выстрел из ружья.
        /// </summary>
        public void Shoot(Side sideShooter, bool IsPlayerShoot = false);
        /// <summary>
        /// Перезарядка ружья.
        /// </summary>
        public void Recharge();
        /// <summary>
        /// Проверяет, пусто ли ружьё.
        /// </summary>
        public bool IsEmpty();
        /// <summary>
        /// Проверяет, находится ли цель в эффективной дальности стрельбы оружия
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <returns></returns>
        public bool IsInRange(Vector3 targetPosition);

    }
}