using UnityEngine;

public class Talker : ClickedObject
{
    public override string Description { get; } = SettingData.Dialogue.ToString();
    public override void Interact()
    {
        // Логика взаимодействия
        Debug.Log("Talker");
    }
}
