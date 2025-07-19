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

    // Eventos
    public event Action OnJump;           // Salto normal
    public event Action OnDoubleJump;     // Doble salto
    public event Action OnFall;           // Inicio de caída
    public event Action OnLand;           // Aterrizaje
    public event Action OnDash;           // Dash
    public event Action OnDamage;         // Daño recibido

    private void Awake()
    {
        JumpsLeft = MaxJumps;
        Life = MaxLife;
    }


    // Intenta un salto. Dispara OnJump o OnDoubleJump según corresponda.
    public bool UseJump()
    {
        if (JumpsLeft <= 0) return false;
        JumpsLeft--;
        if (JumpsLeft == MaxJumps - 1)
            OnJump?.Invoke();        // Primer salto
        else
            OnDoubleJump?.Invoke();  // Segundo salto
        return true;
    }


    // Invocado al caer (por parte del Controller cuando la velocidad vertical es negativa).
    public void Fall()
    {
        OnFall?.Invoke();
    }

    // Recarga saltos y dispara OnLand.
    public void Land()
    {
        JumpsLeft = MaxJumps;
        OnLand?.Invoke();
    }

    // Intenta hacer dash y dispara OnDash.
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

    // El jugador recibe daño.
    public void TakeDamage()
    {
        Life--;
        OnDamage?.Invoke();
    }
}