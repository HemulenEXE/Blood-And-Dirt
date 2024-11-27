
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
        public int AmmoTotal { get; set; }
        /// <summary>
        /// Возвращает вместимость очереди.
        /// </summary>
        public int AmmoCapacity { get; }
        /// <summary>
        /// Возвращает и изменяет текущее число снарядов в очереди.
        /// </summary>
        public int AmmoTotalCurrent { get; set; }
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли перезарядка.
        /// </summary>
        public bool IsRecharging { get; set; }
        /// <summary>
        /// Возврашает и изменяет флаг, указывающий, идёт ли стрельба.
        /// </summary>
        public bool IsShooting { get; set; }
        /// <summary>
        /// Выстрел из ружья.
        /// </summary>
        public void Shoot(int layerMask = 0);
        /// <summary>
        /// Остановка стрельбы из ружья.
        /// </summary>
        public void StopShoot();
        /// <summary>
        /// Перезарядка ружья.
        /// </summary>
        public void Recharge();
        /// <summary>
        /// Проверяет, пусто ли ружьё.
        /// </summary>
        public bool IsEmpty();
    }
}