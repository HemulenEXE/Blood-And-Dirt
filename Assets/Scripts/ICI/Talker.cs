using UnityEngine;

public class Talker : ClickedObject
{
    private void Start()
    {
        Description = SettingData.Dialogue.ToString();
    }
    public override void Interact()
    {
        // ������ ��������������
        Debug.Log("Talker");
    }
}
