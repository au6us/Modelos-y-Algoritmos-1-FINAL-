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

    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void UpdateBehavior()
    {
        if (isDead || enemyRb == null) return;

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

        enemyRb.velocity = new Vector2(direction * speed, enemyRb.velocity.y);
        anim.SetBool("Walk", true);
        sr.flipX = direction > 0;
    }

    public override void ResetState()
    {
        base.ResetState();
        direction = 1;
        if (sr != null) sr.flipX = false;

        // Resetear posición y rotación
        transform.rotation = Quaternion.identity;
    }
}