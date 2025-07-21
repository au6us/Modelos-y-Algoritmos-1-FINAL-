using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyBase Prefab { get; set; }
    public event Action<EnemyBase> OnDie;

    [SerializeField] protected int pointValue = 100;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected float deathAnimDuration = 0.5f;

    protected Animator anim;
    protected Collider2D col;
    protected bool isDead = false;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
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
            ContactPoint2D contact = c.GetContact(0);
            Vector2 normal = contact.normal;

            // Si me tocaron de costado (no de arriba)
            if (Mathf.Abs(normal.y) < 0.5f && Mathf.Abs(normal.x) > 0.5f)
            {
                var player = c.collider.GetComponent<PlayerController>();
                if (player != null)
                    player.Rebound(normal);
            }
        }
    }

    /// <summary>
    /// Llamado por StompDetector al pisar al enemigo.
    /// </summary>
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // NO SE QUITA EL COLLIDER
        anim.SetBool("Walk", false);
        anim.SetBool("Die", true);

        OnDie?.Invoke(this);
        GameEventManager.EnemyKilled(pointValue);

        StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(deathAnimDuration);
        isDead = false;
        anim.SetBool("Die", false);
        ResetAfterDeath();
        PoolManager.Instance.Release(this);
    }

    protected virtual void ResetAfterDeath()
    {
        // Nada que hacer si no desactivamos el collider
        // Pero igual podés resetear animaciones acá si querés
    }
}
