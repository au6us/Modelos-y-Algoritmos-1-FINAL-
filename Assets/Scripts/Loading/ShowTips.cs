using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ShowTips : MonoBehaviour
{
    public TMP_Text tipsText;
    public string[] tips; 
    public float changeInterval = 3f; // Tiempo en segundos entre cada cambio

    private void Start()
    {
        StartCoroutine(ChangeTipsRoutine()); 
    }

    private IEnumerator ChangeTipsRoutine()
    {
        while (true)
        {
            GenerateTips();
            yield return new WaitForSeconds(changeInterval); // Espera el tiempo antes de cambiar
        }
    }

    private void GenerateTips()
    {
        int tipCount = Random.Range(0, tips.Length);
        tipsText.text = tips[tipCount];
    }
}
