using UnityEngine;

/// <summary>
/// Вспомогательный скрипт для проверки раюоты главного меню и менеджера сцен
/// </summary>
public class SceneController2 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ScenesManager.Instance.OnMainMenu();
    }
}
