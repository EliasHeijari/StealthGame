using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(FollowTransform))]
public class Item : MonoBehaviour, IInteractable
{
    public UnityEvent OnSoundMade;

    public string GetInteractText()
    {
        return "Pick Up";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        interactorTransform.GetComponent<ItemHandler>().SetItem(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnSoundMade.Invoke();
    }
}
