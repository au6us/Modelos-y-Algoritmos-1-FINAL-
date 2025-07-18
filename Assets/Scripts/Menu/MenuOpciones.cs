using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public void ChangeVolumen(float volumen)
    {
        audioMixer.SetFloat("Volumen", volumen); 
    }
}
