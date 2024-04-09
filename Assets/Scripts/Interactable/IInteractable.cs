using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public interface IInteractable {

    void Interact(Transform interactorTransform);
    string GetInteractText();
    Transform GetTransform();
    NetworkObject GetNetworkObject();

}