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

        // Получаем объект SerializedObject
        SerializedObject serializedObject = new SerializedObject(e);

        // Получаем итератор по всем свойствам SerializedObject
        SerializedProperty property = serializedObject.GetIterator();

        // Переходим к первому свойству (пропускаем Script)
        property.NextVisible(true);

        while (property.NextVisible(false))
        {
            if (property.name != "SwitchOn" && property.name != "Name" && property.name != "Index")
                EditorGUILayout.PropertyField(property, true); // true включает вложенные свойства
        }

        // Применяем изменения, если были внесены
        serializedObject.ApplyModifiedProperties();
    
        // Обновляем значения в скрипте
        if (GUI.changed)
        {
            EditorUtility.SetDirty(e);
        }
    }
}
#endif