using GunLogic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotSceneManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private EnemySides sides;
    [SerializeField] private bool _isPlayerEnemy = true;
    [SerializeField] private uint _countSoldersMin;
    [SerializeField] private bool _needToRestoreSolder;
    [SerializeField] private bool _restoreKilledNonPlayer;
    [SerializeField] private bool _analogState = false;
    [SerializeField] private float repeatTime = 30f;
    [SerializeField] private float _stoppingDistance = 0;

    private Barraks barraks;
    private Side sideController;



    void Awake()
    {
        barraks = GetComponent<Barraks>();
        sideController = new Side(sides, _isPlayerEnemy);
        
    }

    private void Start()
    {
        if (_analogState)
        {
            InvokeRepeating("SummonBots", 60, repeatTime);
        }
    }
    void OnDisable()
    {
        CancelInvoke(); 
    }


    // Update is called once per frame
    void Update()
    {
        
        if (!_analogState && _needToRestoreSolder && _countSoldersMin > BotEventMediator.Instance.CountBotSide(sides))
        {
            SummonBots();
        }
    }

    private void SummonBots()
    {
        var newSolder = barraks.SpawnSolders(sideController);
        BotEventMediator.Instance.AddBot(sides, newSolder);
    }



    private void RestartScene()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
