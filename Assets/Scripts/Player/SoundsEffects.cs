using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsEffects : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip doubleJump;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


}
