using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Añadir este using para IEnumerator y StartCoroutine
using static ControlLinterna.GestorEventos;

public class ControlLinterna : MonoBehaviour
{
    public Light luzLinterna;
    public GameObject puntoDeLuz;
    public int cantidadPilas = 0;
    public bool linternaEncendida = false;
    public float EnergiaActual = 0f; // Inicializar a 0 para que sea necesario recoger pilas
    public float EnergiaMaxima = 100f;
    public float VelocidadConsumo;

    public TextMeshPro TextoPorcentajeBateria;

    public bool parpadeando = false; // Indica si la linterna está parpadeando

    private Color colorRojo = Color.red;
    private Color colorAmarillo = Color.yellow;
    private Color colorVerde = Color.green;

    public float intensidadInicial;

    void Start()
    {
        GestorEventos.EnEventoLinterna += ManejarEventoLinterna;

        intensidadInicial = luzLinterna.intensity;

        if (TextoPorcentajeBateria != null)
        {
            ActualizarTextoBateria();
        }
    }

    public void ActualizarTextoBateria()
    {
        if (TextoPorcentajeBateria != null)
        {
            float porcentaje = (EnergiaActual / EnergiaMaxima) * 100f;
            TextoPorcentajeBateria.text = $"{porcentaje}";
        }
    }

    void Update()
    {
        if (linternaEncendida && EnergiaActual > 0)
        {
            EnergiaActual -= VelocidadConsumo * Time.deltaTime;
            if (EnergiaActual <= 0)
            {
                EnergiaActual = 0;
                GestorEventos.DispararEventoLinterna(EventoLinterna.TipoEvento.BateriaVacia);
            }
            else if (EnergiaActual < 0.1 && !parpadeando)
            {
                GestorEventos.DispararEventoLinterna(EventoLinterna.TipoEvento.BateriaBaja);
            }
        }

        if(EnergiaActual <= 0)
        {
            EnergiaActual = 0;
        }

        // Actualizar el texto del porcentaje de batería
        float porcentajeBateria = (EnergiaActual / EnergiaMaxima) * 100;
        TextoPorcentajeBateria.text = $"{porcentajeBateria:F1}%";

        // Cambiar el color del texto dependiendo de la energía
        if (EnergiaActual < 20)
        {
            TextoPorcentajeBateria.color = colorRojo;
        }
        else if (EnergiaActual <= EnergiaMaxima * 0.5)
        {
            TextoPorcentajeBateria.color = colorAmarillo;
        }
        else
        {
            TextoPorcentajeBateria.color = colorVerde;
        }
    }

public void EncenderLinterna()
    {
        if (EnergiaActual > 0)
        {
            luzLinterna.intensity = intensidadInicial;
            luzLinterna.enabled = true;
            linternaEncendida = true;
            print("Linterna encendida.");

            if (EnergiaActual < 0.1 && !parpadeando)
            {
                GestorEventos.DispararEventoLinterna(EventoLinterna.TipoEvento.BateriaBaja);
            }
        }
        else
        {
            ApagarLinterna();
            GestorEventos.DispararEventoLinterna(EventoLinterna.TipoEvento.BateriaVacia);
        }
    }

    public void ApagarLinterna()
    {
        // Solo apaga la linterna si no está parpadeando
        if (!parpadeando)
        {
            // Apaga la luz de la linterna y desactiva el rayo de luz
            luzLinterna.enabled = false;
            linternaEncendida = false;
            print("Linterna apagada.");
        }
    }

    public class EventoLinterna
    {
        public enum TipoEvento { BateriaBaja, BateriaVacia }
        public TipoEvento Evento;
    }

    public class GestorEventos : MonoBehaviour
    {
        public delegate void ManejadorEventoLinterna(EventoLinterna.TipoEvento tipo);
        public static event ManejadorEventoLinterna EnEventoLinterna;

        public static void DispararEventoLinterna(EventoLinterna.TipoEvento tipo)
        {
            EnEventoLinterna?.Invoke(tipo);
        }
    }

    void OnDestroy()
    {
        GestorEventos.EnEventoLinterna -= ManejarEventoLinterna;
    }

    void ManejarEventoLinterna(EventoLinterna.TipoEvento tipo)
    {
        switch (tipo)
        {
            case EventoLinterna.TipoEvento.BateriaBaja:
                // Manejar la linterna con batería baja
                if (EnergiaActual > 0)
                {
                    StartCoroutine(ParpadearLinterna());
                }
                break;
            case EventoLinterna.TipoEvento.BateriaVacia:
                // Manejar la linterna sin batería
                StartCoroutine(ParpadearLinternaYApagar());
                break;
            default:
                break;
        }
    }

    private IEnumerator ParpadearLinterna()
    {
        parpadeando = true;

        // Reduce la intensidad de la luz antes de parpadear
        luzLinterna.intensity = Mathf.Lerp(luzLinterna.intensity, 0.5f, 0.5f);

        for (int i = 0; i < 3; i++)
        {
            luzLinterna.enabled = false;
            yield return new WaitForSeconds(0.2f);
            luzLinterna.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        parpadeando = false;
    }

    public IEnumerator ParpadearLinternaYApagar()
    {
        parpadeando = true;

        // Reduce la intensidad de la luz antes de parpadear
        luzLinterna.intensity = Mathf.Lerp(luzLinterna.intensity, 0.5f, 0.5f);

        for (int i = 0; i < 3; i++)
        {
            luzLinterna.enabled = false;
            yield return new WaitForSeconds(0.2f);
            luzLinterna.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        parpadeando = false;

        // Apaga la linterna después de parpadear
        ApagarLinterna();
    }
}












