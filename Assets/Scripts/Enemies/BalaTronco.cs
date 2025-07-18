using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaTronco : MonoBehaviour
{
    
    public float speed = 2f;

    public float timeLife = 4f;

    public float damage = 1;

    private void Start()
    {
        Destroy(gameObject, timeLife);
    }


    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == 6)
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage((int)damage, transform.position);
                player.Knockback(player.knockBackSpeed * 0.5f); //La bala mandaba a china al player con el knockback, esto es para normalizar el knockback como cualquier enemy
                Destroy(gameObject);

            }
        }

    }
}
