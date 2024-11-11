using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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
}
