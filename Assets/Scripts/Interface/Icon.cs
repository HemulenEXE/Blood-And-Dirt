using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//Иконка дерева прокачик (Её активация и управление всплывающим окном с описанием)
public class Icon : MonoBehaviour
{
    [SerializeField]
    private Transform window; //всплывающее окно с описанием
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
            if (Privioses.Length == 0 || Privioses.Any(gm => gm.GetComponentInChildren<Icon>().Active()))
            {
                isActive = true;
                this.GetComponent<Image>().sprite = active;
                Counter.Instance().RemovePoints(price);
                foreach (Image img in this.transform.parent.GetComponentsInChildren<Image>())
                    img.color = new Color(167, 255, 255, 255);
            }
        }
    }
    private void Start()
    {
        this.GetComponent<Image>().sprite = inactive;

        bool flag = false;

        //Включает окно с описанием способности (выключает, сели уже включено)
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!flag)
            {
                window.gameObject.SetActive(true);
                Transform panel = window.transform.GetChild(0);
                panel.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
                panel.GetChild(1).GetComponent<TextMeshProUGUI>().text = discription;
                panel.GetChild(2).GetComponent<TextMeshProUGUI>().text = techDiscription;
                panel.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Стоимость: " + price;
                panel.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
                panel.GetChild(4).GetComponent<Button>().onClick.AddListener(OpenSkill);
                flag = true;
            }
            else
            {
                window.gameObject.SetActive(false);
                flag = false;
            }
        });

    }

}
