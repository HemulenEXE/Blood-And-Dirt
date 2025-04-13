using System;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float _interactionDistance = 1f;
    [SerializeField] private LayerMask _ignoreLayer;

    private InteractiveUI _interactiveUI;

    private void Awake()
    {
        _interactiveUI = GameObject.FindAnyObjectByType<InteractiveUI>();

        if (_interactionDistance < 0) throw new ArgumentOutOfRangeException("PlayerInteract: _interactionDistance < 0");
        if (_interactiveUI == null) throw new ArgumentNullException("PlayerInteract: _interactiveUI is null");
    }
    private void Update()
    {
        Ray2D ray = new Ray2D(this.transform.position, this.transform.right);
        Debug.DrawRay(ray.origin, ray.direction * _interactionDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _interactionDistance, ~_ignoreLayer);

        if (hit.collider != null)
        {
            _interactiveUI.TurnOnText(hit.transform.gameObject);
        }
        else _interactiveUI.TurnOffText();

        var temp = hit.collider?.gameObject?.GetComponent<IInteractable>();

        if (temp is Talker t && Input.GetKeyDown(SettingData.Dialogue))
        {
            t.Interact();
        }

        else if (!(temp is Talker) && temp != null && Input.GetKeyDown(SettingData.Interact))
        {
            temp.Interact();
        }
    }
}