using UnityEngine;

public class Talker : ClickedObject
{
    private void Start()
    {
        Description = SettingData.Dialogue.ToString();
    }
    public override void Interact()
    {
        // Логика взаимодействия
        Debug.Log("Talker");
    }
}
