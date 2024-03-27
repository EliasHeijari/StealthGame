using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
