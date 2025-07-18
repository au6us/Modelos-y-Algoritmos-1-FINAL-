using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float destroyTime = 2f;
    public Vector3 offset = new Vector3 (0, 20, 0);

    void Start()
    {
        Destroy(gameObject, destroyTime);

        transform.position += offset;
    }
}
