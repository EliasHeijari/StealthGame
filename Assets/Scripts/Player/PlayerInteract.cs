using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerInteract : NetworkBehaviour {

    [SerializeField] private float interactRadius = 2f;

    private void Update() {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.E)) {
            IInteractable interactable = GetInteractableObject();
            if (interactable != null) {
                InteractionServerRpc(interactable.GetTransform().GetComponent<NetworkObject>());
            }
        }
    }

    [ServerRpc]
    private void InteractionServerRpc(NetworkObjectReference networkObjectReference)
    {
        InteractionClientRpc(networkObjectReference);
    }

    [ClientRpc]
    private void InteractionClientRpc(NetworkObjectReference networkObjectReference)
    {
        networkObjectReference.TryGet(out NetworkObject networkObject);
        IInteractable interactable = networkObject.GetComponent<IInteractable>();
        interactable.Interact(transform);
    }

    public IInteractable GetInteractableObject() {
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRadius);
        foreach (Collider collider in colliderArray) {
            if (collider.TryGetComponent(out IInteractable interactable)) {
                interactableList.Add(interactable);
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList) {
            if (closestInteractable == null) {
                closestInteractable = interactable;
            } else {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < 
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
                    // Closer
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

}