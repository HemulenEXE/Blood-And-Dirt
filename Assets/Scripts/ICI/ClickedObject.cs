using UnityEngine;

/// <summary>
/// Класс, реализующий "интерактивные объекты, которые нельзя взять в инвентарь".
/// </summary>
public class ClickedObject : MonoBehaviour, IInteractable
{
    public string Name { get; }
    public string Description { get; protected set; }
    private void Start()
    {
        Description = SettingData.Interact.ToString();
    }

    public virtual void Interact()
    {
        // Логика взаимодействия
        Debug.Log("ClickedObject");
    }
}