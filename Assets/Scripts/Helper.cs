using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Helper
{ 
    public static T DeepCopy<T>(T originalObject) where T : new()
    {
        T resultObject = new T();
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance); // �������� ��� ���� ��������� �������

        foreach (FieldInfo field in fields)
        {
            object fieldValue = field.GetValue(originalObject); // �������� �������� ���� �� ������������� �������

            if (fieldValue != null && !field.FieldType.IsValueType && field.FieldType != typeof(string)) // ���� �������� �������� �������� (� �� null), �� ������� ��� �����
            {
                fieldValue = DeepCopy(fieldValue); // ���������� ������� ����� ���������� �������
            }
            field.SetValue(resultObject, fieldValue); // ������������� �������� ���� � ����� ������
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


    public static void SetLayerRecursive(GameObject gameObject, int layer)
    {
        if (gameObject == null) 
        {
            return;
        }
        gameObject.layer = layer;

        foreach (Transform child in gameObject.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }
}
