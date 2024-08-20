using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorAcciones : MonoBehaviour
{
    public List<Disparar> disparadores;

    [SerializeField] private ControlLinterna controlLinterna;
    [SerializeField] private GameObject linterna;

    [SerializeField] private MenuPausa MenuPausa;
    [SerializeField] private RaycastCam _raycast;

    [SerializeField] private AudioClip buttomSound;

    private void Start()
    {
        controlLinterna.TextoPorcentajeBateria.enabled = false;
    }

    void Update()
    {
        Linterna();
    }

    private void Linterna()
    {
        if (linterna.activeSelf && MenuPausa.Pausa == false)
        {
            // Verificar si se presiona la tecla E para prender o apagar la linterna
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!controlLinterna.parpadeando) // Agregar esta condición para evitar reproducir sonido cuando está parpadeando
                {
                    SoundFXManager.Instance.PlaySoundFXClip(buttomSound, linterna.transform, .25f);
                }
                if (controlLinterna.linternaEncendida)
                {
                    // Apagar la linterna si está encendida
                    controlLinterna.ApagarLinterna();
                }
                else
                {
                    // Intentar encender la linterna si hay suficientes pilas
                    if (controlLinterna.cantidadPilas >= 3 || controlLinterna.EnergiaActual > 0)
                    {
                        controlLinterna.EncenderLinterna();
                    }
                    else
                    {
                        print("Busca pilas para encender la linterna.");
                    }
                }
            }

            if (controlLinterna.linternaEncendida == true)
            {
                controlLinterna.EnergiaActual -= Time.deltaTime * controlLinterna.VelocidadConsumo;

                if (controlLinterna.EnergiaActual <= 0)
                {
                    controlLinterna.ApagarLinterna();
                }

            }

            controlLinterna.ActualizarTextoBateria();
        }
    }
}


