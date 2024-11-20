using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerScene : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject _player;
    [SerializeField] private List<BotController> _enemies;
    [SerializeField] private float alarmDistance = 5;



    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList().ConvertAll(e => e.GetComponent<BotController>());
        

    }

    private void OnEnable()
    {
        BotController.DetectedEnemy += RaisingAlarm;
    }

    private void OnDisable()
    {
        BotController.DetectedEnemy -= RaisingAlarm;
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
                if (_enemy == enemy)
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
}
