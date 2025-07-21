using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public EnemyBase Prefab { get; set; }
    public event Action<EnemyBase> OnDie;

    [SerializeField] protected int pointValue = 100;
    [SerializeField] protected LayerMask playerLayer;

    protected Animator anim;
    protected Collider2D col;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        UpdateBehavior();
    }

    protected abstract void UpdateBehavior();

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (((1 << c.gameObject.layer) & playerLayer) != 0
            && Mathf.Abs(c.relativeVelocity.x) > 0.5f)
        {
            c.collider.GetComponent<PlayerController>()
             .Rebound(c.GetContact(0).normal);
        }
    }

    public void Die()
    {
        col.enabled = false;
        anim.SetTrigger("Die");
        OnDie?.Invoke(this);
        GameEventManager.EnemyKilled(pointValue);
        StartCoroutine(ReturnToPoolAfterDelay(0.5f));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.Instance.Release(this);
    }
}
