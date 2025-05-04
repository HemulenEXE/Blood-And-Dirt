using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UIElements;
using System;

enum StateDifficultyWave { easyFirstWave, easySecondWave, easyThirdWave, mediumFirstWave, mediumSecondWave, mediumThirdWave, hardFirstWave, hardSecondWave, hardThirdWave };
enum Product {pistol, shotgun, machineGun, launchGun, bandage, firstKit, granade, flask };

public class ControllerArena : MonoBehaviour
{    
    [SerializeField] private BonusArena[] bonusArena;
    [SerializeField] private GameObject controllerBeliever;
    [SerializeField] private GameObject controllerFalcons;
    [SerializeField] private int updateTimeBonusAndWave = 60;
    [SerializeField] private GameObject spawnSellerPlace;
    [SerializeField] private GameObject[] products;
    [SerializeField] private GameObject stathonarMachineGun;



    public static event Action<Transform, string> playHorn;

    private StateDifficultyWave currentWaveStay = StateDifficultyWave.easySecondWave;
    private BalancePlayer balance;
    private bool startTrigger = false;
    private bool isAlreadyUpgradeFalcons = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!startTrigger && collision.gameObject.tag == "Player")
        {
            startTrigger = true;
            InvokeRepeating("UpDifficultyArena", 0, updateTimeBonusAndWave);
            InvokeRepeating("UpdateBonus", 0, updateTimeBonusAndWave);
            controllerBeliever.SetActive(true);
            controllerFalcons.SetActive(true);
            InvokeRepeating("PlayHorn", 60, updateTimeBonusAndWave);
        }
    }

    void OnDisable()
    {
        CancelInvoke();
        ShowDialogueWithLogging.OnAnswerChosen -= ReactToAnswer;
    }

    void Start()
    {
        balance = BalancePlayer.Instance;
    }

    private void OnEnable()
    {
        ShowDialogueWithLogging.OnAnswerChosen += ReactToAnswer;
    }

    // Update is called once per frame
    void Update()
    {
        if(startTrigger) 
        {

        }
    }

    private void UpDifficultyArena()
    {
        switch(currentWaveStay) 
        {
            case StateDifficultyWave.easyFirstWave:
                controllerBeliever.GetComponent<Barraks>().SetMaxStrengthWave(5);
                break;
            case StateDifficultyWave.easySecondWave:
                controllerBeliever.GetComponent<Barraks>().SetMaxStrengthWave(10);
                break;
            case StateDifficultyWave.easyThirdWave:
                controllerBeliever.GetComponent<Barraks>().SetMaxStrengthWave(15);
                break;
            case StateDifficultyWave.mediumFirstWave:
                controllerBeliever.GetComponent<LevelArmyEasy>().enabled = false;
                controllerBeliever.GetComponent<LevelArmyMedium>().enabled = true;
                controllerBeliever.GetComponent<LevelArmyMedium>().Init();
                controllerBeliever.GetComponent<Barraks>().SetArmy(controllerBeliever.GetComponent<LevelArmyMedium>());
                controllerBeliever.GetComponent<Barraks>().SetMaxStrengthWave(10);
                break;
            case StateDifficultyWave.mediumSecondWave:
                controllerBeliever.GetComponent<Barraks>().SetMaxStrengthWave(15);
                break;
            case StateDifficultyWave.mediumThirdWave:
                controllerBeliever.GetComponent<Barraks>().SetMaxStrengthWave(20);
                break;
            case StateDifficultyWave.hardFirstWave:
                controllerBeliever.GetComponent<LevelArmyMedium>().enabled = false;
                controllerBeliever.GetComponent<LevelArmyHard>().enabled = true;
                controllerBeliever.GetComponent<LevelArmyHard>().Init();
                controllerBeliever.GetComponent<Barraks>().SetArmy(controllerBeliever.GetComponent<LevelArmyHard>());
                controllerBeliever.GetComponent<Barraks>().SetMaxStrengthWave(15);
                break;
            case StateDifficultyWave.hardSecondWave:
                controllerBeliever.GetComponent<Barraks>().SetMaxStrengthWave(20);
                break;
            case StateDifficultyWave.hardThirdWave:
                controllerBeliever.GetComponent<Barraks>().SetMaxStrengthWave(25);
                break;
        }

        balance.GiveSallary(20);
        currentWaveStay++;
    }

    private void UpdateBonus()
    {
        foreach(var bonus in bonusArena)
        {
            bonus.UpdateCurrentBonus();
        }
        PlayerData.Score++;
    }

    private void ReactToAnswer(ShowDialogueWithLogging directoDialogue, Dialogue.Answer answer)
    {
        Debug.Log("React!");
        int? cost = IsPurchase(answer.text);
        if (cost != null)
        {
            Debug.Log("Its purchase!");
            if (BalancePlayer.Instance.IsCanBuy((int)cost))
            {
                Debug.Log("money hav");
                if (answer.text.Contains("Пистолет")) Instantiate(products[(int)Product.pistol], spawnSellerPlace.transform);
                else if (answer.text.Contains("Дробовик")) Instantiate(products[(int)Product.shotgun], spawnSellerPlace.transform);
                else if (answer.text.Contains("Автомат")) Instantiate(products[(int)Product.machineGun], spawnSellerPlace.transform);
                else if (answer.text.Contains("Гранатомет")) Instantiate(products[(int)Product.launchGun], spawnSellerPlace.transform);
                else if (answer.text.Contains("Стационарный пулемет")) stathonarMachineGun.SetActive(true);
                else if (answer.text.Contains("Граната")) Instantiate(products[(int)Product.granade], spawnSellerPlace.transform);
                else if (answer.text.Contains("Аптечка")) Instantiate(products[(int)Product.firstKit], spawnSellerPlace.transform);
                else if (answer.text.Contains("Бинты")) Instantiate(products[(int)Product.bandage], spawnSellerPlace.transform);
                else if (answer.text.Contains("Колба")) Instantiate(products[(int)Product.flask], spawnSellerPlace.transform);
                else if (answer.text.Contains("Мне нужно побольше помощников"))
                {
                    controllerFalcons.GetComponent<Barraks>().AddMaxStrengthWave(2);
                }
                else if (answer.text.Contains("Они должны быть лучше вооружены"))
                {
                    if(!isAlreadyUpgradeFalcons)
                    {
                        controllerFalcons.GetComponent<LevelArmyEasy>().enabled = false;
                        var buf = controllerFalcons.GetComponent<LevelArmyMedium>();
                        buf.enabled = true;
                        buf.Init();
                        controllerFalcons.GetComponent<Barraks>().SetArmy(buf);
                    }
                }
                BalancePlayer.Instance.BuyObject((int)cost);
            }
            else
            {
                answer.toNode = 7; // Magic Number, yea
            }
            
        }
    }

    int? IsPurchase(string text)
    {
        int? result = null;
        Match match = Regex.Match(text, @"\((\d+)\s*очков\)");
        if(match.Success)
        {
            result = int.Parse(match.Groups[1].Value);
        }

        return result;
    }

    void PlayHorn()
    {
        playHorn?.Invoke(transform, "battle_horn_1-6931");
    }
}
