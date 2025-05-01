using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueWndState : MonoBehaviour
{
    public enum WindowState { NotActive, StartPrint, EndPrint};
    public WindowState currentState = WindowState.NotActive;

    private void Update()
    {
        if (currentState == WindowState.NotActive && gameObject.activeSelf)
            currentState = WindowState.StartPrint;
        else if (currentState == WindowState.EndPrint && gameObject.activeSelf)
            currentState = WindowState.StartPrint;
    }
}
