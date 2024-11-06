using System.Collections;
using UnityEngine;

/// <summary>
/// Класс огнемёта.
/// </summary>
public class FlameThrower : AbstractSprayingGun
{
    /// <summary>
    /// Начинает распыление из огнемёта.
    /// </summary>
    public override void StartFiring()
    {
        _isSpraying = true;
        _prefabFiredObject.SetActive(true);
        _animatorPrefabFireObject.SetTrigger("StartFire");
        Fire();
    }
    /// <summary>
    /// Длительное распыление из огнемёта.
    /// </summary>
    protected override void Fire()
    {
        _animatorPrefabFireObject.SetTrigger("Fire");
    }
    /// <summary>
    /// Заканчивает распыление из огнемёта.
    /// </summary>
    public override void StopFiring()
    {
        _isSpraying = false;
        _animatorPrefabFireObject.SetTrigger("StopFire");

        StartCoroutine(DeactivateAfterAnimation());
    }
    /// <summary>
    /// Метод, необходимый для предотвращения преждевременного (до завершения конечной анимации) деактивирования _prefabFiredObject
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DeactivateAfterAnimation()
    {
        AnimatorStateInfo stateInfo = _animatorPrefabFireObject.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length * 1.4f); //Знаю, что плохо, но другого выхода не нашёл
        //Деактивация _prefabFiredObject по окончанию анимации
        _prefabFiredObject.SetActive(false);
    }
    /// <summary>
    /// Перезарядка огнемёта.
    /// </summary>
    public override void Recharge()
    {
        if (_volumeFuel > 0 && !_isReloading)
        {
            _isReloading = true;
            StartCoroutine(RechargeCoroutine());
        }
    }
    /// <summary>
    /// Метод, реализующий задержку во время перезарядки.
    /// </summary>
    /// <returns></returns>
    private IEnumerator RechargeCoroutine()
    {
        while (_currentVolumeFuel != _capacityFuel)
        {
            _volumeFuel--;
            _currentVolumeFuel++;
            yield return new WaitForSeconds(_timeRecharging / _capacityFuel);
            
            //Игрок может выстрелить до полной перезарядки.
        }
        _isReloading = false;
    }
}
