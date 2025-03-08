using UnityEngine;

public interface IInteractable
{
    public string Name { get; }
    public string Description { get; }
    public void Interact();
}