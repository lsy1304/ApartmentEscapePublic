using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerController controller;

    private readonly int isWalking = Animator.StringToHash("isWalking");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        controller.OnStateChangeEvent += ChangeAnimationState;
    }

    private void ChangeAnimationState(PlayerState ps)
    {
        animator.SetBool(isWalking, ps == PlayerState.Walk);
    }
}
