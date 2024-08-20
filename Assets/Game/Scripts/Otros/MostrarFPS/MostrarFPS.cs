using UnityEngine;
using TMPro;


public class MostrarFPS : MonoBehaviour
{

    [Header("FPS")]
    public TextMeshPro textoFPS;
    public int maxFPS = 90;
    private float tiempoInicio;
    private int framesPasados = 0;

    void Start()
    {
        Application.targetFrameRate = maxFPS;
        tiempoInicio = Time.time;
    }

    void Update()
    {
        framesPasados++;

        if (Time.time - tiempoInicio >= 1f)
        {
            float fps = framesPasados / (Time.time - tiempoInicio);
            textoFPS.text = "FPS: " + Mathf.Round(fps);

            tiempoInicio = Time.time;
            framesPasados = 0;
        }
    }
}
