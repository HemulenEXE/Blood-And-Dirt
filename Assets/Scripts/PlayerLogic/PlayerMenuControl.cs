using UnityEngine;

public class PlayerMenuControl : MonoBehaviour
{
    [SerializeField]
    private GameObject _skillMenu;

    [SerializeField]
    private GameObject _inventoryMenu;

    // Start is called before the first frame update
    void Start()
    {
        //_skillMenu = GameObject.Find("LevelsUp");
        //_skillMenu.SetActive(false);
        _inventoryMenu = GameObject.Find("Inventory&ConsumableCounter");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _inventoryMenu.SetActive(!_inventoryMenu.active);
            //_skillMenu.SetActive(!_skillMenu.active);
        }
    }
}
