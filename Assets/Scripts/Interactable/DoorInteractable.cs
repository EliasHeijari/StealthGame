using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    bool isOpen = false;

    [SerializeField] Animator animator;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public string GetInteractText()
    {
        if (isOpen) return "Close";
        else return "Open";
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact(Transform interactorTransform)
    {
        animator.SetTrigger("OpenClose");
        isOpen = !isOpen;
    }
}
