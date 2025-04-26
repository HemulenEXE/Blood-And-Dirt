using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlaceToHide : ClickedObject
{
    private GameObject _player;
    public static event Action<Transform, string> AudioEvent;
    private bool IsInPlaceToHide { get; set; } = false;
    public override void Interact()
    {
        AudioEvent?.Invoke(this.transform, "hide_audio");
        _player.SetActive(false);

        PlayerData.IsMotionless = true;
        IsInPlaceToHide = true;

        Debug.Log("PlaceToHide is used");
    }
    private void Exit()
    {
        _player.SetActive(true);
        PlayerData.IsMotionless = false;
        IsInPlaceToHide = false;

        Debug.Log("PlaceToHide isn't used");
    }

    private void FixedUpdate()
    {
        if (IsInPlaceToHide && Input.GetKey(KeyCode.Q)) Exit();
    }
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").gameObject;
    }

}
