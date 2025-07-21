using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hongo : Entity
{
    [SerializeField] float speed;
    [SerializeField] Transform floorController;
    [SerializeField] float distance;
    [SerializeField] bool lookRight;
    private Rigidbody2D rbHongo;

    private void Start()
    {
        rbHongo = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D informacionSuelo = Physics2D.Raycast(floorController.position, Vector2.down, distance, LayerMask.GetMask("Floor"));
        
        rbHongo.velocity = new Vector2(speed, rbHongo.velocity.y);

        if(informacionSuelo == false)
        {
            
            Turn();
        }
    }

    private void Turn()
    {
        lookRight = !lookRight;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        speed *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(floorController.transform.position, floorController.transform.position + Vector3.down * distance);
    }
}
