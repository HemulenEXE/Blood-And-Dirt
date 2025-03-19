using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//������ �������� �� ����� ����� ����� ��������� ������� (���������, ��� ������ ��������, ���� ��������� ��� ��������� �������, �� ������� �� ��������� EXIT)
//���������� �� ���������, ����� ��������� � ������� ��������� ������� �� ������ �����
public class SwitchAfterDialogue : SwitchScene
{
    [SerializeField] TextAsset FileName;
    private GameObject DialogueWindow;
    private Dialogue _dialogue;
    private ShowDialogueDubl _director;
    private void Start()
    {
        _director = GetComponent<ShowDialogueDubl>();
    }
    void Update()
    {
        if (_director.FileName == FileName)
        {
            DialogueWindow = _director.DialogueWindow.gameObject;
            _dialogue = _director.GetDialogue();
        }

        if (!DialogueWindow.activeSelf && (_dialogue.GetCurentNodeIndex() == _dialogue.Nodes.Length - 1))
            Switch();   
    }
}
