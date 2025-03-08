using SkillLogic;
using UnityEngine;

public class LiveInNotVain : Skill
{
    private float _searchRadius = 1f;

    public LiveInNotVain()
    {
        Name = "LiveInNotVain";
        IsUnlocked = false;
        Type = SkillType.Added;
    }

    public override void Execute(GameObject point)
    {
        var hitColliders = Physics.OverlapSphere(point.transform.position, _searchRadius);
        foreach (var x in hitColliders)
        {
            x.gameObject.GetComponent<Body>().GetDamage(1);
            Debug.Log($"{x.name} is eated");
        }

    }
}
