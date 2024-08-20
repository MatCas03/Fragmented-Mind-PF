using System.Collections.Generic;
using UnityEngine;

public class ParpadeoLuz : MonoBehaviour
{
    public List<GameObject> objectsToBlink; // Lista de GameObjects que parpadearán y se activarán/desactivarán
    private Renderer objectRenderer;
    private Light objectLight;
    private Renderer[] childRenderers;
    private Light[] childLights;

    public float blinkInterval = 0.5f; // Intervalo de parpadeo en segundos
    private bool isEmissionOn = false;
    private float timer;

    void Start()
    {
        // Obtener el Renderer del objeto actual
        objectRenderer = GetComponent<Renderer>();
        // Obtener el Light del objeto actual si existe
        objectLight = GetComponent<Light>();

        // Obtener todos los Renderers de los hijos
        childRenderers = GetComponentsInChildren<Renderer>();
        // Obtener todos los Lights de los hijos
        childLights = GetComponentsInChildren<Light>();

        // Inicialmente apagar la emisión
        SetEmission(false);

        // Inicializar el temporizador
        timer = blinkInterval;

        // Verificar y activar/desactivar los objetos asignados inicialmente
        foreach (GameObject obj in objectsToBlink)
        {
            if (obj != null)
            {
                obj.SetActive(false); // Inicialmente desactivado
            }
        }
    }

    void Update()
    {
        // Contador para el parpadeo
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ToggleEmission(); // Cambiar estado de la emisión
            timer = blinkInterval; // Reiniciar el temporizador
        }
    }

    // Método para cambiar el estado de la emisión
    private void ToggleEmission()
    {
        isEmissionOn = !isEmissionOn;
        SetEmission(isEmissionOn);

        // Si se asignaron objetos específicos para parpadear y activar/desactivar
        foreach (GameObject obj in objectsToBlink)
        {
            if (obj != null)
            {
                obj.SetActive(isEmissionOn); // Activar/desactivar según el estado de parpadeo
            }
        }
    }

    // Método privado para activar o desactivar la emisión y las luces
    private void SetEmission(bool isActive)
    {
        // Cambiar la emisión del objeto actual
        if (objectRenderer != null)
        {
            Material material = objectRenderer.material;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", isActive ? Color.white : Color.black);
        }

        // Cambiar la luz del objeto actual si existe
        if (objectLight != null)
        {
            objectLight.enabled = isActive;
        }

        // Cambiar la emisión y las luces de los hijos
        foreach (Renderer childRenderer in childRenderers)
        {
            Material childMaterial = childRenderer.material;
            childMaterial.EnableKeyword("_EMISSION");
            childMaterial.SetColor("_EmissionColor", isActive ? Color.white : Color.black);
        }

        foreach (Light childLight in childLights)
        {
            childLight.enabled = isActive;
        }
    }
}