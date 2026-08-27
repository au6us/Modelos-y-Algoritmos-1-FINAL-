using System.Collections;
using UnityEngine;

// Boost: sube velocidad/salto/dash por un tiempo fijo y después devuelve los stats originales.
public class BoostPowerUpStrategy : IPowerUpStrategy
{
    public IEnumerator Activate(PlayerModel model, PowerUpManager.Entry entry)
    {
        entry.originalMoveSpeed = model.MoveSpeed;
        entry.originalMaxJumps = model.MaxJumps;
        entry.originalDashCooldown = model.DashCooldown;

        model.MoveSpeed *= entry.moveSpeedMultiplier;
        model.MaxJumps += entry.extraJumps;
        model.DashCooldown *= entry.dashCooldownMultiplier;

        yield return new WaitForSeconds(entry.duration);

        model.MoveSpeed = entry.originalMoveSpeed;
        model.MaxJumps = entry.originalMaxJumps;
        model.DashCooldown = entry.originalDashCooldown;
    }
}

// Shield: dura hasta el timer O hasta que el escudo se rompa antes (el jugador recibió un golpe).
public class ShieldPowerUpStrategy : IPowerUpStrategy
{
    public IEnumerator Activate(PlayerModel model, PowerUpManager.Entry entry)
    {
        model.GrantShield();

        float timer = entry.duration;
        while (timer > 0 && model.HasShield)
        {
            timer -= Time.deltaTime;

            // Parpadeo final (últimos 2 seg)
            if (timer < 2f && entry.effectObject != null)
                entry.effectObject.SetActive((Time.time * 10) % 2 > 1);

            yield return null;
        }

        // Si se acabó el tiempo y todavía tiene escudo, lo rompemos
        if (model.HasShield) model.BreakShield();
    }
}

// CameraZoom: todavía no hay un controlador de cámara que reaccione a esto — se mantiene
// el mismo comportamiento que tenía antes del refactor (solo feedback + espera).
public class CameraZoomPowerUpStrategy : IPowerUpStrategy
{
    public IEnumerator Activate(PlayerModel model, PowerUpManager.Entry entry)
    {
        yield return new WaitForSeconds(entry.duration);
    }
}
