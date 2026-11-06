using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float attackRange = 1f;

    [SerializeField]
    private float attackDamage = 20f;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private float attackCooldown = 0.5f;
    [SerializeField]
    private float attackPointDistance = 0.7f;

    [SerializeField]
    private LineRenderer attackCircle;

    [SerializeField]
    private int circleSegments = 60;

    private float nextAttackTime;

    private Vector2 lastMoveDirection = Vector2.right;
    [SerializeField] private Animator animator;

    private void Start()
    {
        DrawAttackCircle();
    }

    private void DrawAttackCircle()
    {
        if (attackCircle == null)
            return;

        attackCircle.useWorldSpace = false;
        attackCircle.loop = true;
        attackCircle.positionCount = circleSegments;
        attackCircle.startWidth = 0.03f;
        attackCircle.endWidth = 0.03f;

        for (int i = 0; i < circleSegments; i++)
        {
            float angle = i * Mathf.PI * 2f / circleSegments;

            float x = Mathf.Cos(angle) * attackRange;
            float y = Mathf.Sin(angle) * attackRange;

            attackCircle.SetPosition(
                i,
                new Vector3(x, y, 0)
            );
        }
    }

    private void Update()
    {
        UpdateAttackDirection();

        if (Input.GetKeyDown(KeyCode.Space) &&
            Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void Attack()
{
if (animator != null)
{
animator.SetFloat("DirectionX", lastMoveDirection.x);
animator.SetFloat("DirectionY", lastMoveDirection.y);
animator.SetBool("IsAttacking", true);
animator.SetTrigger("Attack");
}

Collider2D[] hits =
Physics2D.OverlapCircleAll(
attackPoint.position,
attackRange,
enemyLayer
);

foreach (Collider2D hit in hits)
{
EnemyHealth enemyHealth =
hit.GetComponent<EnemyHealth>();

if (enemyHealth != null)
{
enemyHealth.TakeDamage(attackDamage);
}
}
}

    private void UpdateAttackDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 moveDirection = new Vector2(
            horizontal,
            vertical
        );

        if (moveDirection != Vector2.zero)
        {
            lastMoveDirection = moveDirection.normalized;
        }

        attackPoint.localPosition =
            (Vector3)(lastMoveDirection * attackPointDistance);
    }

    private void EndAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsAttacking", false);
        }
    }
}

