using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Helper
{ 
    public static T DeepCopy<T>(T originalObject) where T : new()
    {
        T resultObject = new T();
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); // Получаем все поля исходного объекта

        foreach (FieldInfo field in fields)
        {
            object fieldValue = field.GetValue(originalObject); // Получаем значение поля из оригинального объекта

            if (fieldValue != null && !field.FieldType.IsValueType && field.FieldType != typeof(string)) // Если значение является объектом (и не null), то создаем его копию
            {
                fieldValue = DeepCopy(fieldValue); // Рекурсивно создаем копию вложенного объекта
            }
            field.SetValue(resultObject, fieldValue); // Устанавливаем значение поля в новый объект
        }

        return resultObject;
    }


    public static GameObject CopyTransformInGameObject(Transform transform)
    {
        GameObject result = new GameObject();
        result.transform.position = transform.position;
        result.transform.rotation = transform.rotation;
        result.transform.localScale = transform.localScale;

        return result;
    }

    public static int SetMask(string[] add, string ignore)
    {
        return LayerMask.GetMask(add) & ~LayerMask.GetMask(ignore);
    }

    public static int SetMask(string[] add, string[] ignore)
    {
        return LayerMask.GetMask(add) & ~LayerMask.GetMask(ignore);
    }

    public static bool IsAgentMoving(NavMeshAgent agent)
    {
        return agent.hasPath && !agent.pathPending && agent.remainingDistance > agent.stoppingDistance && agent.velocity.sqrMagnitude > 0;
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer; // Меняем слой у родителя

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer); // Рекурсивно меняем у дочерних объектов
        }
    }
}
