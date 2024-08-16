using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController controller;
    private Rigidbody2D movementRigidbody;
    private SpriteRenderer spriteRenderer;
    
    private Vector2 movementDirection = Vector2.zero;

    [SerializeField] private float speed = 100f;
    [SerializeField] private AudioClip footprintClip;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        movementRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        controller.OnMoveEvent += Move;
    }

    private void Move(Vector2 direction)
    {
        movementDirection = direction;
        if (direction != Vector2.zero)
        {
            spriteRenderer.flipX = (movementDirection == Vector2.left);
        }
    }

    public void DoorStop()
    {
        StartCoroutine(InteractStop(0.8f));
        controller.EnableInput(true);
    }

    private void FixedUpdate()
    {
        ApplyMovement(movementDirection);
    }

    private void ApplyMovement(Vector2 direction)
    {
        direction = direction * speed * Time.fixedDeltaTime;
        movementRigidbody.velocity = direction;
    }

    private IEnumerator InteractStop(float waitTime)
    {
        float time = 0f;
        controller.EnableInput(false);
        while (time < waitTime)
        {
            movementDirection = Vector2.zero;
            time += Time.deltaTime;
            yield return null;
        }
    }
}
