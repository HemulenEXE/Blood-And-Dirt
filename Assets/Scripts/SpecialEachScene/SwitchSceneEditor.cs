#if UNITY_EDITOR //Позволяет перенести работу со скриптом в редактор юнити
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwitchScene), true)]

///Класс Editor позволяет создать свой итерфейс для редактирования объекта (ChengeScene) в инспекторе юнити
public class ChangeSceneEditor : Editor
{
    /// <summary>
    /// Определяет, как будет отображатся поле инспектора для объекта
    /// </summary>
    public override void OnInspectorGUI()
    {
        SwitchScene e = (SwitchScene)target;

        e.SwitchOn = (SwitchScene.States)EditorGUILayout.EnumPopup("SwitchOn: ", e.SwitchOn);

        if (e.SwitchOn == SwitchScene.States.ByIndex)
        {
            e.Index = EditorGUILayout.IntField("Index: ", e.Index);
        }

        if (e.SwitchOn == SwitchScene.States.ByName)
        {
            e.Name = EditorGUILayout.TextField("Scene Name:", e.Name);
        }

        // Обновляем значения в скрипте
        if (GUI.changed)
        {
            EditorUtility.SetDirty(e);
        }
    }
}
#endif