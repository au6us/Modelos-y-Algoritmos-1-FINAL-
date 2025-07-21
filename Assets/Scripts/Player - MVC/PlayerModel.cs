using System;
using System.Collections;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float JumpForce = 7f;
    public int MaxJumps = 2;
    public float DashSpeed = 12f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 1f;
    public int MaxLife = 3;

    public int JumpsLeft { get; private set; }
    public bool CanDash { get; private set; } = true;
    public int Life { get; private set; }

    public event Action OnJump;
    public event Action OnDoubleJump;
    public event Action OnFall;
    public event Action OnLand;
    public event Action OnDash;
    public event Action OnDamage;

    private void Awake()
    {
        JumpsLeft = MaxJumps;
        Life = MaxLife;
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

    public void TakeDamage()
    {
        Life--;
        OnDamage?.Invoke();
    }

    /// <summary>
    /// Cura al jugador, respetando el máximo de vida.
    /// </summary>
    public void Heal(int amount)
    {
        Life = Mathf.Min(Life + amount, MaxLife);
        // Aquí podrías emitir un OnHeal si lo necesitas
    }
}
