using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

//Переход на новую сцену после прохода в новую комнату
public class SwitchOnDoorOpening : SwitchScene
{
    //В какой позиции игрок должен появиться на следующей сцене
    [SerializeField]
    public Vector3 Position;
    [SerializeField]
    private Quaternion Rotation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Door door = this.transform.parent.GetComponentInChildren<Door>();
        if (door.isRunning || door.IsOpen)
        {
            if (PlayerInitPosition.Instance != null)
            {
                Debug.Log($"Name: {Name}");
                if (SwitchOn == States.ByName)
                    for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                    {
                        var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                        if (scenePath.EndsWith(Name + ".unity"))
                        {
                            PlayerInitPosition.Instance.SavePosition(i, Position, Rotation);
                            break;
                        }
                    }
                else PlayerInitPosition.Instance.SavePosition(Index, Position, Rotation);

                Debug.Log($"Position: {Position}, onScene: {Index}, current scene: {SceneManager.GetActiveScene().buildIndex}");
            }
            Switch();
        }
    }
}