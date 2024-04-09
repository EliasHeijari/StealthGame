using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DoorInteractable : NetworkBehaviour, IInteractable
{
    NetworkVariable<bool> isOpen = new NetworkVariable<bool>();

    [SerializeField] Animator animator;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public string GetInteractText()
    {
        if (isOpen.Value) return "Close";
        else return "Open";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

    public void Interact(Transform interactorTransform)
    {
        animator.SetTrigger("OpenClose");
        IsOpenTriggerServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void IsOpenTriggerServerRpc()
    {
        isOpen.Value = !isOpen.Value;
    }
}
