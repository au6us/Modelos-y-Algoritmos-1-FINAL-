using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyBase Prefab { get; set; }
    public EnemySpawner OriginSpawner { get; set; }
    public event Action<EnemyBase> OnDie;

    [SerializeField] protected int pointValue = 100;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float deathAnimDuration = 0.5f;

    protected Animator anim;
    protected Collider2D col;
    protected Rigidbody2D enemyRb;
    protected bool isDead = false;



    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        enemyRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isDead)
            UpdateBehavior();
    }

    protected abstract void UpdateBehavior();

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (isDead) return;

        if (((1 << c.gameObject.layer) & playerLayer) != 0)
        {
            var player = c.collider.GetComponent<PlayerController>();
            if (player == null) return;

            ContactPoint2D contact = c.GetContact(0);
            Vector2 normal = contact.normal;

            // El jugador viene desde arriba (stomp) → NO hacemos nada
            if (normal.y > 0.5f) return;

            // Calcular dirección basada en posición relativa
            Vector2 knockbackDirection = CalculateKnockbackDirection(player.transform.position);

            player.Rebound(knockbackDirection);
            player.TakeDamage();
        }
    }

    // Nuevo método para calcular dirección de knockback
    private Vector2 CalculateKnockbackDirection(Vector2 playerPosition)
    {
        // Calcular diferencia de posiciones
        float relativePositionX = playerPosition.x - transform.position.x;

        // Determinar dirección horizontal (misma dirección que el punto de contacto)
        float horizontalDirection = Mathf.Sign(relativePositionX);

        // Crear vector con fuerte componente horizontal y pequeño vertical
        return new Vector2(horizontalDirection, 0.2f).normalized;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Deshabilitar física y colisiones
        if (col != null) col.enabled = false;
        if (enemyRb != null) enemyRb.simulated = false;

        anim.SetBool("Walk", false);
        anim.SetBool("Die", true);

        OnDie?.Invoke(this);
        GameEventManager.EnemyKilled(pointValue);

        StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(deathAnimDuration);

        // Notificar al spawner para que respawnee
        if (OriginSpawner != null)
        {
            OriginSpawner.ScheduleRespawn();
        }

        // Devolver al pool
        PoolManager.Instance.Release(this);
    }

    public virtual void ResetState()
    {
        isDead = false;

        // Resetear animaciones
        anim.SetBool("Die", false);
        anim.SetBool("Walk", true);

        // Reactivar componentes
        if (col != null) col.enabled = true;
        if (enemyRb != null)
        {
            enemyRb.simulated = true;
            enemyRb.velocity = Vector2.zero;
            enemyRb.angularVelocity = 0f;
        }

        gameObject.SetActive(true);
    }
}