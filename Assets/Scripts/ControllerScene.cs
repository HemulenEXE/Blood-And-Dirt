using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerLogic;

public class ControllerScene : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject _player;
    [SerializeField] private List<BotController> _enemies;
    [SerializeField] private float alarmDistance = 5;



    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemies = GameObject.FindGameObjectsWithTag("Enemy").Where(x => x.GetComponent<BotController>()).ToList().ConvertAll(e => e.GetComponent<BotController>());
        

    }

    private void OnEnable()
    {
        BotController.DetectedEnemy += RaisingAlarm;
        HealthBot.death += DeathBot;
        PlayerMotion.makeNoise += CheckNose;
    }

    private void OnDisable()
    {
        BotController.DetectedEnemy -= RaisingAlarm;
        HealthBot.death += DeathBot;
        PlayerMotion.makeNoise += CheckNose;
    }
    // Update is called once per frame
    void Update()
    {
        if(_player == null) 
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
        
        if(_enemies != null)
        {
            foreach (var _enemy in _enemies)
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

    private void CheckNose(Transform transform, float radiusNoise)
    {
        if(_enemies != null)
        {
            foreach(var _enemy in _enemies)
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
        _enemies.Remove(bot);
    }
}
