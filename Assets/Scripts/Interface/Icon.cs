using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    [SerializeField]
    private Transform panel;
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

    public Sprite InActive() { return inactive; }
    public Sprite Active() { return active; }
    private void Start()
    {
        this.GetComponent<Image>().sprite = inactive;

        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            panel.gameObject.SetActive(true);
            panel = panel.transform.GetChild(0);
            panel.GetChild(0).GetComponent<TextMeshProUGUI>().text = title;
            panel.GetChild(1).GetComponent<TextMeshProUGUI>().text = discription;
            panel.GetChild(2).GetComponent<TextMeshProUGUI>().text = techDiscription;
        });
    }

}
