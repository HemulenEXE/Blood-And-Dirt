using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuControl : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private GameObject _skillMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseMenu.SetActive(!_pauseMenu.active);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            _skillMenu.SetActive(!_skillMenu.active);
        }
    }
}
