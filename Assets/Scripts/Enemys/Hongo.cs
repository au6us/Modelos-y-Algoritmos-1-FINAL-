using UnityEngine;

public class Hongo : EnemyBase
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    private int direction = 1;

    [Header("Checks")]
    [Tooltip("Capa de suelo y muros")]
    [SerializeField] private LayerMask groundLayer;             // aquí pones Floor
    [SerializeField] private float groundCheckDistance = 0.2f;   // bajo el pie
    [SerializeField] private float wallCheckDistance = 0.1f;   // frente al torso
    [SerializeField] private Transform groundCheckPoint;        // hijo bajo el pie
    [SerializeField] private Transform wallCheckPoint;          // hijo frente al pecho

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void UpdateBehavior()
    {
        // 1) Compruebo suelo delante
        bool isGroundAhead = Physics2D.Raycast(
            groundCheckPoint.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        // 2) Compruebo muro delante
        bool isWallAhead = Physics2D.Raycast(
            wallCheckPoint.position,
            Vector2.right * direction,
            wallCheckDistance,
            groundLayer
        );

        // Si no hay suelo o hay muro, invierto
        if (!isGroundAhead || isWallAhead)
            direction *= -1;

        // 3) Muevo
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        // 4) Animación y flip
        anim.Play("Hongo_Walk");
        sr.flipX = direction > 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                groundCheckPoint.position,
                groundCheckPoint.position + Vector3.down * groundCheckDistance
            );
        }
        if (wallCheckPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                wallCheckPoint.position,
                wallCheckPoint.position + Vector3.right * direction * wallCheckDistance
            );
        }
    }
}
