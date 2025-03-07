using TMPro;
using UnityEngine;

public class CounterDisplay : MonoBehaviour
{ 
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = ": " + Counter.Instance().Points(); 
    }
}
