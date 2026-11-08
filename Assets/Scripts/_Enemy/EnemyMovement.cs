using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Transform target;
    private bool canMove;
    public float MoveSpeed => moveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetMovementEnabled(bool value)
    {
        canMove = value;

        if (!canMove)
            rb.linearVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (!canMove || target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (Vector2)target.position - rb.position;

        if (direction.sqrMagnitude < 0.0001f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = direction.normalized * moveSpeed;
    }

    public void Stop()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
    }
}
