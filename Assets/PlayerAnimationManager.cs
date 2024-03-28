using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    private void Update()
    {
        animator.SetFloat("Velocity", playerController.vertical);
    }
}
