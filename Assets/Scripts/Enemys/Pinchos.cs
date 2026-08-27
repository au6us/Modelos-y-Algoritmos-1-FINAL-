using UnityEngine;

// Reactivado: la versión vieja llamaba a TakeDamage(vida, posición), una firma que
// ya no existe en PlayerModel. Queda alineado al mismo patrón que usan Trofeo y
// PowerUpPickup — Tag "Player" en vez de un número de layer fijo.
[RequireComponent(typeof(Collider2D))]
public class Pinchos : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        var model = collision.GetComponent<PlayerModel>();
        if (model != null) model.TakeDamage(damage);
    }
}
