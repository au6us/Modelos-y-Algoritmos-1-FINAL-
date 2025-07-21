using UnityEngine;

public class Hongo : EnemyBase
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    private int direction = 1;

    [Header("Checks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private float wallCheckDistance = 0.1f;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform wallCheckPoint;

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
        // raycast suelo y pared
        bool isGroundAhead = Physics2D.Raycast(
            groundCheckPoint.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );
        bool isWallAhead = Physics2D.Raycast(
            wallCheckPoint.position,
            Vector2.right * direction,
            wallCheckDistance,
            groundLayer
        );

        if (!isGroundAhead || isWallAhead)
            direction *= -1;

        rb.velocity = new Vector2(direction * speed, rb.velocity.y);

        // animación patrulla
        anim.SetBool("Walk", true);
        sr.flipX = direction > 0;
    }
}
