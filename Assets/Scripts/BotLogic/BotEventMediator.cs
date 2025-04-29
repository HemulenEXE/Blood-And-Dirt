using GunLogic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotEventMediator : MonoBehaviour
{
    public static BotEventMediator Instance { get; private set; }
    private List<BotController> allBot;
    private List<BotController> believesBot;
    private List<BotController> falconsBot;
    private List<CivilianController> civilianPeople;
    
    /// <summary>
    /// Как далеко поднимается тревога солдатами
    /// </summary>
    [SerializeField] private float alarmDistance = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        
        believesBot = GameObject.FindGameObjectsWithTag("EnemyBelievers").ToList().ConvertAll(e => e.GetComponent<BotController>());
        falconsBot = GameObject.FindGameObjectsWithTag("EnemyFalcons").ToList().ConvertAll(e => e.GetComponent<BotController>());
        civilianPeople = GameObject.FindGameObjectsWithTag("CivilianPeople").ToList().ConvertAll(e => e.GetComponent<CivilianController>());
        allBot = believesBot.Union(falconsBot).ToList();
    }
    private void OnEnable()
    {
        BotController.DetectedEnemy += RaisingAlarm;
        HealthBot.death += DeathBot;
        PlayerMotion.makeNoise += CheckNoise;
        //FlameThrower.makeNoiseShooting += CheckNoise;
        ShotGun.makeNoiseShooting += CheckNoise;
        Pistol.makeNoiseShooting += CheckNoise;
        MachineGun.makeNoiseShooting += CheckNoise;
        GrenadeLauncher.makeNoiseShooting += CheckNoise;
        ShotGun.makeNoiseShooting += CivilianUpAlarm;
        Pistol.makeNoiseShooting += CivilianUpAlarm;
        MachineGun.makeNoiseShooting += CivilianUpAlarm;
        GrenadeLauncher.makeNoiseShooting += CivilianUpAlarm;
    }

    private void OnDisable()
    {
        BotController.DetectedEnemy -= RaisingAlarm;
        HealthBot.death -= DeathBot;
        PlayerMotion.makeNoise -= CheckNoise;
        //FlameThrower.makeNoiseShooting -= CheckNoise;
        ShotGun.makeNoiseShooting -= CheckNoise;
        Pistol.makeNoiseShooting -= CheckNoise;
        MachineGun.makeNoiseShooting -= CheckNoise;
        GrenadeLauncher.makeNoiseShooting -= CheckNoise;
        ShotGun.makeNoiseShooting -= CivilianUpAlarm;
        Pistol.makeNoiseShooting -= CivilianUpAlarm;
        MachineGun.makeNoiseShooting -= CivilianUpAlarm;
        GrenadeLauncher.makeNoiseShooting -= CivilianUpAlarm;
    }

    private void CivilianUpAlarm(Transform transform, float radiusNoise)
    {
        if (civilianPeople != null)
        {
            foreach (var people in civilianPeople)
            {
                if (people == null)
                {
                    civilianPeople.Remove(people);
                    continue;
                }
                if (Vector2.Distance(people.transform.position, transform.position) <= radiusNoise)
                {
                    people?.TriggerFlee();
                }
            }
        }
    }

    private void RaisingAlarm(Transform solder, Transform detechedEnemy)
    {
        string tag = solder.tag;
        var buf = tag == "EnemyFalcons" ? falconsBot : tag == "EnemyBelievers" ? believesBot : allBot;

        if (buf != null)
        {
            foreach (var _enemy in buf)
            {
                if (_enemy.transform.position == solder.transform.position)
                {
                    continue;
                }
                if (Vector3.Distance(solder.position, _enemy.transform.position) <= alarmDistance)
                {
                    _enemy.NotifiedOfEnemy(detechedEnemy);
                }
            }
        }

    }

    private void CheckNoise(Transform transform, float radiusNoise)
    {
        if (allBot != null)
        {
            foreach (var _enemy in allBot)
            {
                if(_enemy == null)
                {
                    allBot.Remove(_enemy);
                    continue;
                }
                if (Vector2.Distance(_enemy.transform.position, transform.position) <= radiusNoise)
                {
                    _enemy?.ReactToNoise(transform);
                }
            }
        }
    }

    private void DeathBot(BotController bot)
    {
        allBot.Remove(bot);
        if(believesBot.Contains(bot))
        {
            believesBot.Remove(bot);
        }
        else if(falconsBot.Contains(bot))
        {
            falconsBot.Remove(bot);
        }
    }

    public int CountBot()
    {
        return allBot.Count;
    }

    public int CountBotSide(EnemySides sides)
    {
        if(sides == EnemySides.falcons)
        {
            return falconsBot.Count;
        }
        else if(sides == EnemySides.believers)
        {
            return believesBot.Count;
        }
        else { return 0; }
    }

    public void AddBot(EnemySides sides, List<BotController> bots)
    {
        if (sides == EnemySides.falcons)
        {
            falconsBot.AddRange(bots);
        }
        else if (sides == EnemySides.believers)
        {
            believesBot.AddRange(bots);
        }
        allBot.AddRange(bots);
    }
}
