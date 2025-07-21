using UnityEngine;

public class Murcielago : EnemyBase
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed;
    [SerializeField] private float maxDistance;
    private Vector3 startPos;
    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        startPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void UpdateBehavior()
    {
        float d = Vector2.Distance(transform.position, player.position);
        if (d < maxDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                                                     player.position,
                                                     speed * Time.deltaTime);
            anim.Play("Bat_Fly");
            sr.flipX = player.position.x < transform.position.x;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position,
                                                     startPos,
                                                     speed * Time.deltaTime);
            anim.Play("Bat_Idle");
            sr.flipX = startPos.x < transform.position.x;
        }
    }
}
