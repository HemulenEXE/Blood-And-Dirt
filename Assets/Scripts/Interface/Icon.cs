using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
using SkillLogic;
using PlayerLogic;
using static UnityEditor.Experimental.GraphView.GraphView;

//Иконка дерева прокачик (Её активация и управление всплывающим окном с описанием)
public class Icon : MonoBehaviour
{
    [SerializeField]
    private Animator window; //всплывающее окно с описанием 
    [SerializeField]
    private Sprite inactive;
    [SerializeField]
    private Sprite active;
    [SerializeField]
    private string title;
    [SerializeField]
    private string discription;
    [SerializeField]
    private string techDiscription;
    [SerializeField]
    private string skillName;

    private GameObject _player; //для обновления PlayerInfo
    private Skill skill; //активируемый навык
    private int price;
    public GameObject[] Privioses; //Предшествующие навыки
    private bool isActive;
    public bool Active() { return isActive; }
    //активирует выбранный навык
    public void OpenSkill()
    {
        if (Counter.Instance().Points() >= price) //Активирует, если достаточно монет
        {
            // Активирует, если активирован один из предшествующих навыков, или если их нет
            if ((Privioses.Length == 0 || Privioses.Any(gm => gm.GetComponentInChildren<Icon>().Active())) && !PlayerInfo.HasSkill(skill))
            {
                isActive = true;
                this.GetComponent<Image>().sprite = active;
                Counter.Instance().RemovePoints(price);

                if (skill._type == SkillType.Added)
                    AddSkill(skill);
                else ActivateSkill(skill);

                foreach (Image img in this.transform.parent.GetComponentsInChildren<Image>())
                    img.color = new Color(167, 255, 255, 255);
            }
        }
    }
    private void Start()
    {
        this.GetComponent<Image>().sprite = inactive;

        _player = GameObject.FindWithTag("Player");

        if (SkillStorage._skills.ContainsKey(skillName))
            skill = SkillStorage._skills[skillName];
        else throw new ArgumentNullException("Skill with such name don't exists!");

        //Раскоментировать, когда будет добавлено поле со стоимостью!!!!
        //price = skill._price; 

        //Включает окно с описанием способности (выключает, еcли уже включено)
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!window.GetBool("isOpen"))
            {
                Debug.Log($"Открытие, isOpen = {window.GetBool("isOpen")}");
                window.SetBool("isOpen", true);
                Transform panel = window.transform.GetChild(0);
                panel.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
                panel.GetChild(1).GetComponent<TextMeshProUGUI>().text = discription;
                panel.GetChild(2).GetComponent<TextMeshProUGUI>().text = techDiscription;
                panel.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Стоимость: " + price;
                panel.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
                panel.GetChild(4).GetComponent<Button>().onClick.AddListener(OpenSkill);
            }
            else
            {
                Debug.Log($"Закрытие, isOpen = {window.GetBool("isOpen")}");
                window.SetBool("isOpen", false);
            }
        });
    }
    private void AddSkill(Skill skill)
    {
        skill._isUnlocked = true;
        PlayerInfo.AddSkill(skill);
        Debug.Log($"{skill._name} is added!");
    }
    private void ActivateSkill(Skill skill)
    {
        PlayerInfo.AddSkill(skill);
        skill.Execute(_player);
        Debug.Log($"{skill._name} is used!");     
    }

}
