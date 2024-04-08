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
    Rigidbody rb;

    public bool isRigidbodyEnable { get { return !rb.isKinematic; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public string GetInteractText()
    {
        return "Pick Up";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void DisableRigidbody()
    {
        rb.isKinematic = true;
    }

    public void EnableRigidbody()
    {
        rb.isKinematic = false;
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
