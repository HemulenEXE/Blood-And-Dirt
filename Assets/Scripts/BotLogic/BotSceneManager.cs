using GunLogic;
using PlayerLogic;
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

    private Barraks barraks;
    private Side sideController;



    void Awake()
    {
        barraks = GetComponent<Barraks>();
        sideController = new Side(sides, _isPlayerEnemy);
    }

    
    // Update is called once per frame
    void Update()
    {

        if (_needToRestoreSolder && _countSoldersMin >= BotEventMediator.Instance.CountBotSide(sides))
        {
            var newSolder = barraks.SpawnSolders(sideController);
            BotEventMediator.Instance.AddBot(sides,newSolder);
        }
    }



    private void RestartScene()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
