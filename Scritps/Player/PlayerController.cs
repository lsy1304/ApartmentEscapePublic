using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Walk
}

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    
    public event Action<Vector2> OnMoveEvent;
    public event Action OnInteractEvent;
    public event Action OnDialogueEvent;
    public event Action OnPauseEvent;
    public event Action<PlayerState> OnStateChangeEvent;

    private Vector2 moveInput;

    private static bool inputEnable = true;
    private bool InputEnable
    {
        get => inputEnable;
        set
        {
            CallMoveEvent(Vector2.zero);
            inputEnable = value;
        }
    }
    
    public static bool IsInteractInterrupted => DialogueManager.Instance.UI.IsDialogueOn || MapManager.Instance.IsOnMoving || UIManager.Instance.IsUIOpen || !inputEnable;

    public PlayerState currentState { get; private set; } = PlayerState.Idle;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        SoundManager.Instance.RegisterPlayerController(this);
    }
    public void CallMoveEvent(Vector2 direction)
    {
        Vector2 directionCorrected = direction;
        
        if (IsInteractInterrupted)
        {
            directionCorrected = Vector2.zero;
        }

        OnMoveEvent?.Invoke(directionCorrected);
        UpdateState(directionCorrected);
    }

    public void CallInteractionEvent()
    {
        OnInteractEvent?.Invoke();
    }

    public void CallDialogueEvent()
    {
        OnDialogueEvent?.Invoke();
    }

    public void CallPauseEvent()
    {
        OnPauseEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInput = Vector2.zero;
        }
        CallMoveEvent(moveInput);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CallInteractionEvent();
        }
    }

    public void OnDialogue(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CallDialogueEvent();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CallPauseEvent();
        }
    }

    public void EnableInput(bool enable)
    {
        InputEnable = enable;
        
        if (enable == false)
        {
            moveInput = Vector2.zero;
            SetState(PlayerState.Idle);
        }
    }

    private void UpdateState(Vector2 direction)
    {
        if (direction != Vector2.zero) SetState(PlayerState.Walk);
        else SetState(PlayerState.Idle);
    }

    public void SetState(PlayerState ps)
    {
        if (currentState == ps) return;
        currentState = ps;
        OnStateChangeEvent?.Invoke(ps);
    }

    private void OnDestroy()
    {
        OnPauseEvent = null;
    }
}
