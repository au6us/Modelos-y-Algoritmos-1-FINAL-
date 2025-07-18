using System;
using System.Collections;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    // Parámetros ajustables
    public float MoveSpeed = 5f;
    public float JumpForce = 7f;
    public int MaxJumps = 2;
    public float DashSpeed = 12f;
    public float DashDuration = 0.2f;
    public float DashCooldown = 1f;
    public int MaxLife = 3;

    // Estado interno
    public int JumpsLeft { get; private set; }
    public bool CanDash { get; private set; } = true;
    public int Life { get; private set; }

    // Eventos
    public event Action OnJump;
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
        OnJump?.Invoke();
        return true;
    }

    public void Land()
    {
        JumpsLeft = MaxJumps;
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
}