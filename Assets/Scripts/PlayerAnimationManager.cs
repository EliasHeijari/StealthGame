using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimationManager : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        if (!IsOwner) return;

        animator.SetFloat("Velocity", playerController.vertical);
    }
}
