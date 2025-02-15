using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Icon : MonoBehaviour
{
    [SerializeField]
    private Transform window;
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
    public GameObject[] Privioses;
    private bool isActive;
    public bool Active() { return isActive; }
    //активирует выбранный навык
    public void OpenSkill()
    {
        if (Privioses.Length == 0 || Privioses.Any(gm => gm.GetComponentInChildren<Icon>().Active()))
        {
            isActive = true;
            this.GetComponent<Image>().sprite = active;
            foreach (Image img in this.transform.parent.GetComponentsInChildren<Image>())
                img.color = new Color(167, 255, 255, 255);
        }
    }
    private void Start()
    {
        this.GetComponent<Image>().sprite = inactive;

        //Включает окно с описанием способности
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            window.gameObject.SetActive(true);
            Transform panel = window.transform.GetChild(0);
            panel.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
            panel.GetChild(1).GetComponent<TextMeshProUGUI>().text = discription;
            panel.GetChild(2).GetComponent<TextMeshProUGUI>().text = techDiscription;
            panel.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Стоимость: " + price;
            panel.GetChild(4).GetComponent<Button>().onClick.RemoveAllListeners();
            panel.GetChild(4).GetComponent<Button>().onClick.AddListener(OpenSkill);
        });
    }

}
