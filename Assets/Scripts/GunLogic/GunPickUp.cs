using System;

public class GunPickUp : VisibleItemPickUp
{
    protected IGun _gun;
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
