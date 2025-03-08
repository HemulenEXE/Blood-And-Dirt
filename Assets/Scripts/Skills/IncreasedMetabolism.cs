using System.Collections;
using UnityEngine;

public class IncreasedMetabolism : Skill
{
    private float _timeWithoutDamage = 5f;
    private float _damageTimer = 0f;
    private bool _isStartedTimer;

    public IncreasedMetabolism()
    {
        Name = "IncreasedMetabolism";
        IsUnlocked = false;
        Type = SkillType.Activated;
    }

    public override void Execute(GameObject point)
    {
        if (!PlayerData.HasSkill<DropByDrop>()) { /* Внедрение дебафа */ }
        if (PlayerData.IsBleeding && !_isStartedTimer)
        {
            _isStartedTimer = true;
            point.GetComponent<PlayerHealth>().StartCoroutine(StopBleedingCoroutine(point));
        }
    }

    private IEnumerator StopBleedingCoroutine(GameObject player)
    {
        while (PlayerData.IsBleeding)
        {
            yield return new WaitForSeconds(1f);

            if (_damageTimer >= _timeWithoutDamage) StopBleeding(player);
            else _damageTimer += 1f;
        }
    }

    public void RebootTimer()
    {
        _damageTimer = 0f;
    }

    private void StopBleeding(GameObject player)
    {
        PlayerData.IsBleeding = false;
        _isStartedTimer = false;
        Debug.Log(PlayerData.IsBleeding);
    }
}
