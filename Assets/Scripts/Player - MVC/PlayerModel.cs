using System;
using System.Collections;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;
    public float JumpForce = 7f;
    public int MaxJumps = 2;
    public float DashSpeed = 12f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 1f;

    [Header("Health Settings")]
    [SerializeField] private int maxLife = 10;
    [SerializeField] private int currentLife = 10;

    public int JumpsLeft { get; private set; }
    public bool CanDash { get; private set; } = true;
    public int Life => currentLife;
    public int MaxLife => maxLife;
    public bool IsRespawning { get; private set; }

    public event Action OnJump;
    public event Action OnDoubleJump;
    public event Action OnFall;
    public event Action OnLand;
    public event Action OnDash;
    public event Action OnDamage;
    public event Action OnDeath;
    public event Action<int> OnLifeChanged;

    private void Awake()
    {
        JumpsLeft = MaxJumps;
        currentLife = maxLife;
    }

    public bool UseJump()
    {
        if (JumpsLeft <= 0) return false;
        JumpsLeft--;
        if (JumpsLeft == MaxJumps - 1) OnJump?.Invoke();
        else OnDoubleJump?.Invoke();
        return true;
    }

    public void Fall() => OnFall?.Invoke();
    public void Land()
    {
        JumpsLeft = MaxJumps;
        OnLand?.Invoke();
    }

    public bool UseDash()
    {
        if (!CanDash) return false;
        CanDash = false;
        OnDash?.Invoke();
        StartCoroutine(DashCooldownRoutine());
        return true;
    }

    private IEnumerator DashCooldownRoutine()
    {
        yield return new WaitForSeconds(DashCooldown);
        CanDash = true;
    }

    public void TakeDamage(int damageAmount)
    {
        if (IsRespawning) return;

        bool willDie = (currentLife - damageAmount) <= 0;

        currentLife = Mathf.Max(currentLife - damageAmount, 0);

        // Disparar evento de vida
        GameEventManager.TriggerPlayerLifeEvent(currentLife, maxLife);
        OnLifeChanged?.Invoke(currentLife);

        if (!willDie)
        {
            OnDamage?.Invoke();
        }

        if (currentLife <= 0)
            OnDeath?.Invoke();
    }

    public void Heal(int amount)
    {
        int newLife = Mathf.Min(currentLife + amount, maxLife);

        if (newLife != currentLife)
        {
            currentLife = newLife;
            GameEventManager.TriggerPlayerLifeEvent(currentLife, maxLife);
            OnLifeChanged?.Invoke(currentLife);
        }
    }

    // Nuevo método para establecer la vida directamente
    public void SetLife(int newLife)
    {
        newLife = Mathf.Clamp(newLife, 0, maxLife);
        if (newLife != currentLife)
        {
            currentLife = newLife;
            GameEventManager.TriggerPlayerLifeEvent(currentLife, maxLife);
            OnLifeChanged?.Invoke(currentLife);
        }
    }

    public PlayerMemento SaveState(Vector3 pos)
    {
        return new PlayerMemento(pos, currentLife);
    }

    public void StartRespawn()
    {
        IsRespawning = true;
    }

    public void EndRespawn()
    {
        IsRespawning = false;
    }
}