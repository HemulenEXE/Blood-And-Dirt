using GunLogic;
using PlayerLogic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerBotScene : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private List<GameObject> _enemiesSolders;
    [SerializeField] private List<BotController> _alliesSolders;
    [SerializeField] private float alarmDistance = 5;
    [SerializeField] private string alliesTag = "Enemy";
    [SerializeField] private string enemiesTag = "Player";
    [SerializeField] private bool _isPlayerEnemy = true;

    [SerializeField] private uint _countSolders;
    [SerializeField] private bool _needToRestoreSolder;
    [SerializeField] private bool _restoreKilledNonPlayer;



    void Awake()
    {
        _enemiesSolders = GameObject.FindGameObjectsWithTag(enemiesTag).ToList();
        _alliesSolders = GameObject.FindGameObjectsWithTag(alliesTag).Where(x => x.GetComponent<BotController>()).ToList().ConvertAll(e => e.GetComponent<BotController>());
        

    }

    private void OnEnable()
    {
        BotController.DetectedEnemy += RaisingAlarm;
        HealthBot.death += DeathBot;
        PlayerMotion.makeNoise += CheckNoise;
        FlameThrower.makeNoiseShooting += CheckNoise;
        ShotGun.makeNoiseShooting += CheckNoise;
        Pistol.makeNoiseShooting += CheckNoise;
        MachineGun.makeNoiseShooting += CheckNoise;
    }

    private void OnDisable()
    {
        BotController.DetectedEnemy -= RaisingAlarm;
        HealthBot.death -= DeathBot;
        PlayerMotion.makeNoise -= CheckNoise;
        FlameThrower.makeNoiseShooting -= CheckNoise;
        ShotGun.makeNoiseShooting -= CheckNoise;
        Pistol.makeNoiseShooting -= CheckNoise;
        MachineGun.makeNoiseShooting -= CheckNoise;
    }
    // Update is called once per frame
    void Update()
    {
        if(_enemiesSolders == null) 
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }

    private void RaisingAlarm(Transform enemy, Transform player)
    {
        
        if(_alliesSolders != null)
        {
            foreach (var _enemy in _alliesSolders)
            {
                if (_enemy.transform.position == enemy.transform.position)
                {
                    continue;
                }
                if (Vector3.Distance(enemy.position, _enemy.transform.position) <= alarmDistance)
                {
                    _enemy.NotifiedOfEnemy(player);
                }
            }
        }
        
    }

    private void CheckNoise(Transform transform, float radiusNoise)
    {
        if(_alliesSolders != null)
        {
            foreach(var _enemy in _alliesSolders)
            {
                if(Vector2.Distance(_enemy.transform.position, transform.position) <= radiusNoise)
                {
                    _enemy.ReactToNoise(transform);
                }
            }
        }
    }

    private void DeathBot(BotController bot)
    {
        _alliesSolders.Remove(bot);
    }
}
