using UnityEngine;

public class Murcielago : EnemyBase
{
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;

    private Vector3 startPos;
    private SpriteRenderer sr;
    private Transform playerTarget; // Referencia dinámica

    protected override void Awake()
    {
        base.Awake();
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void UpdateBehavior()
    {
        if (playerTarget == null)
        {
            FindPlayer();
            return; // Sale del frame si no encontró jugador
        }

        float d = Vector2.Distance(transform.position, playerTarget.position);

        if (d < maxDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                                                     playerTarget.position,
                                                     speed * Time.deltaTime);
            anim.Play("Bat_Fly");
            sr.flipX = playerTarget.position.x > transform.position.x;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position,
                                                     startPos,
                                                     speed * Time.deltaTime);
            anim.Play("Bat_Idle");
            sr.flipX = startPos.x > transform.position.x;
        }
    }

    private void FindPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(
            transform.position,
            maxDistance,
            playerLayer
        );

        if (playerCollider != null)
        {
            playerTarget = playerCollider.transform;
        }
    }
}