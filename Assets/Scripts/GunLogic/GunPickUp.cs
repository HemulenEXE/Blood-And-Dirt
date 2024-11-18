using System;

/// <summary>
/// Скрипт, реализующий "оружия, которые игрок может взять в свой инвентарь".
/// </summary>
public class GunPickUp : VisibleItemPickUp
{
    /// <summary>
    /// Выбранное оружие.
    /// </summary>
    protected IGun _gun;
    /// <summary>
    /// Настройка и проверка полей.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    protected override void Awake()
    {
        base.Awake();
        _gun = this.GetComponent<IGun>();
        if (_gun == null) throw new ArgumentNullException("GunPickUp: _gun is null");
    }
    /// <summary>
    /// Деактивирование предмета на сцене.<br/>
    /// Более безопасный аналог метода SetActive(false).
    /// </summary>
    public override void Deactive()
    {
        _gun.IsRecharging = false;
        _gun.IsShooting = false;
        base.Deactive();
    }
}
