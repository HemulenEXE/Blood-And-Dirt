using System;
using UnityEngine;

public class HoverDescription : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowDescription()
    {
        this.gameObject.SetActive(true);
    }
    public void HideDescription()
    {
        this.gameObject.SetActive(false);
    }
}
