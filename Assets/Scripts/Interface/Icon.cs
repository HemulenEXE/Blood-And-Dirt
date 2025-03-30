using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            if ((Privioses.Length == 0 || Privioses.Any(gm => gm.GetComponentInChildren<Icon>().Active())) && !PlayerData.HasSkill(skill))
            {
                isActive = true;
                this.GetComponent<Image>().sprite = active;
                Counter.Instance().RemovePoints(price);

                if (skill.Type == SkillType.Added)
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

        if (PlayerData.SkillsStorage.ContainsKey(skillName))
            skill = PlayerData.SkillsStorage[skillName];
        else skill = null; //throw new ArgumentNullException("Skill with such name don't exists!"); //Раскоментировать, когда все навыки будут подвязаны

        if (skill != null)
            price = skill.Cost; 

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
        if (skill != null)
        {
            skill.IsUnlocked = true;
            PlayerData.AddSkill(skill);
            Debug.Log($"{skill.Name} is added!");
        }
    }
    private void ActivateSkill(Skill skill)
    {
        if (skill != null)
        {
            PlayerData.AddSkill(skill);
            skill.Execute(_player);
            Debug.Log($"{skill.Name} is used!");
        }
    }

}
