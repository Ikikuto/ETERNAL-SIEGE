using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
    private Vector2 direction;
    private Vector2 lastDirection = Vector2.down;

private void UpdateAnimation()
    {
        if (animator == null) return;

        if (direction.sqrMagnitude > 0.01f)
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                    {
                        lastDirection = direction.x > 0
                        ? Vector2.right
                        : Vector2.left;
                    }
                else
                    {
                        lastDirection = direction.y > 0
                        ? Vector2.up
                        : Vector2.down;
                    }
            }

        animator.SetFloat("DirectionX", lastDirection.x);
        animator.SetFloat("DirectionY", lastDirection.y);
        animator.SetFloat("Speed", direction.magnitude); 
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new(horizontal, vertical);
        if (direction.sqrMagnitude > 1f)
            {
                direction.Normalize();
            }
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Vector2 targetPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }
}
