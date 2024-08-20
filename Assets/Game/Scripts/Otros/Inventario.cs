using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Inventario : MonoBehaviour
{
    public GameObject inventarioUI; // El panel del inventario
    public Image[] imagenesInventario; // Arreglo de im�genes en el inventario (tama�o 3: arriba, izquierda, derecha)
    public JugadorCam jugadorCamScript; // Referencia al script JugadorCam

    public bool inventarioAbierto = false;

    public TextMeshProUGUI pildoraRoja;
    public TextMeshProUGUI pildoraAzul;
    public TextMeshProUGUI pildoraNaranja;

    private int indiceSeleccionado = 0; // �ndice del objeto actualmente seleccionado
    private Vector3[] escalasOriginales; // Para almacenar las escalas originales de las im�genes

    private float moveThreshold = 1f; // Sensibilidad del movimiento del mouse para cambiar de selecci�n

    public Player player; // Referencia al jugador
    public GunSystem gs; // Referencia al sistema de armas

    public Pildoras pildoraScriptAzul;
    public Pildoras pildoraScriptRoja;
    public Pildoras pildoraScriptNaranja;

    public TextMeshProUGUI textoPildora; // Para mostrar el nombre de la p�ldora
    public Image imagenPildora; // Para mostrar la imagen de la p�ldora

    public Sprite[] spritesPildoras;

    public float duracionPildoraRoja = 10f;
    public float duracionPildoraAzul = 10f;
    public float duracionPildoraNaranja = 10f;

    private float tiempoRestantePildoraRoja;
    private float tiempoRestantePildoraAzul;
    private float tiempoRestantePildoraNaranja;

    public TextMeshProUGUI textoTiempoPildora;

    public RaycastCam raycastCamScript;

    void Start()
    {
        Time.timeScale = 1;

        // Inicializar el arreglo de escalas originales
        escalasOriginales = new Vector3[imagenesInventario.Length];
        for (int i = 0; i < imagenesInventario.Length; i++)
        {
            escalasOriginales[i] = imagenesInventario[i].transform.localScale;
        }

        // Inicializa el inventario con el primer elemento seleccionado
        ActualizarSeleccion();
    }

    void Update()
    {
        // Detecta si se presiona la tecla Tab para abrir/cerrar el inventario
        if (Input.GetKey(KeyCode.Tab))
        {
            if (!inventarioAbierto)
            {
                AbrirInventario();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (inventarioAbierto)
            {
                CerrarInventario();
            }
        }

        if (inventarioAbierto)
        {
            // Obtener el movimiento del mouse
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            // Calcular el �ndice seleccionado basado en la direcci�n del movimiento del mouse
            if (mouseDelta.x > moveThreshold)
            {
                CambiarSeleccion(1); // Mover a la derecha
            }
            else if (mouseDelta.x < -moveThreshold)
            {
                CambiarSeleccion(2); // Mover a la izquierda
            }
            else if (mouseDelta.y > moveThreshold)
            {
                CambiarSeleccion(0); // Mover arriba
            }

            // Seleccionar imagen con clic izquierdo del mouse
            if (Input.GetMouseButtonDown(0))
            {
                SeleccionarImagen(indiceSeleccionado);
            }
        }
    }

    // M�todo para abrir el inventario
    void AbrirInventario()
    {
        inventarioUI.SetActive(true);
        inventarioAbierto = true;
        Time.timeScale = 0.25f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        jugadorCamScript.enabled = false; // Desactivar el script JugadorCam
        ActualizarSeleccion();
    }

    void CerrarInventario()
    {
        inventarioUI.SetActive(false);
        inventarioAbierto = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        jugadorCamScript.enabled = true; // Activar el script Jugador cam
    }

    // M�todo para cambiar la selecci�n y actualizar la visualizaci�n
    void CambiarSeleccion(int nuevoIndice)
    {
        // Restablecer la escala del objeto actualmente seleccionado
        imagenesInventario[indiceSeleccionado].transform.localScale = escalasOriginales[indiceSeleccionado];

        // Actualizar el �ndice seleccionado
        indiceSeleccionado = nuevoIndice;

        // Actualizar la selecci�n visualmente
        ActualizarSeleccion();
    }

    // M�todo para actualizar la visualizaci�n del objeto seleccionado
    void ActualizarSeleccion()
    {
        // Restablecer la escala de todas las im�genes
        for (int i = 0; i < imagenesInventario.Length; i++)
        {
            imagenesInventario[i].transform.localScale = escalasOriginales[i];
        }

        // Agrandar la imagen seleccionada
        imagenesInventario[indiceSeleccionado].transform.localScale = escalasOriginales[indiceSeleccionado] * 1.2f;
    }

    void SeleccionarImagen(int index)
    {
        Debug.Log("Has seleccionado la imagen en el �ndice: " + index);

        // Verificar si hay p�ldoras disponibles del tipo seleccionado
        if (raycastCamScript.contadorPildoras[(Pildoras.TipoPildora)index] <= 0)
        {
            Debug.LogWarning("No tienes p�ldoras de este tipo para usar.");
            return;
        }

        // Mostrar el texto e imagen de la p�ldora seleccionada
        string nombrePildora = "";
        Sprite spritePildora = null;

        // Asignar el nombre y el Sprite correspondiente seg�n el tipo de p�ldora seleccionado
        switch ((Pildoras.TipoPildora)index)
        {
            case Pildoras.TipoPildora.Roja:
                nombrePildora = "P�ldora Azul";
                spritePildora = spritesPildoras[0]; // Obtener el Sprite de la p�ldora Roja desde la lista
                break;
            case Pildoras.TipoPildora.Azul:
                nombrePildora = "P�ldora Roja";
                spritePildora = spritesPildoras[1]; // Obtener el Sprite de la p�ldora Azul desde la lista
                break;
            case Pildoras.TipoPildora.Naranja:
                nombrePildora = "P�ldora Naranja";
                spritePildora = spritesPildoras[2]; // Obtener el Sprite de la p�ldora Naranja desde la lista
                break;
            default:
                Debug.LogWarning("�ndice de p�ldora fuera de rango: " + index);
                return;
        }

        // Mostrar el nombre de la p�ldora en el texto
        textoPildora.text = "Has recogido: " + nombrePildora;

        // Asignar el Sprite obtenido al componente Image que muestra la imagen de la p�ldora
        imagenPildora.sprite = spritePildora;

        // Mostrar elementos visuales de la p�ldora seleccionada
        textoPildora.gameObject.SetActive(true);
        imagenPildora.gameObject.SetActive(true);

        // Descontar una p�ldora del tipo seleccionado
        raycastCamScript.contadorPildoras[(Pildoras.TipoPildora)index]--;

        // Actualizar el contador en el inventario
        switch ((Pildoras.TipoPildora)index)
        {
            case Pildoras.TipoPildora.Roja:
                pildoraRoja.text = raycastCamScript.contadorPildoras[Pildoras.TipoPildora.Roja].ToString();
                break;
            case Pildoras.TipoPildora.Azul:
                pildoraAzul.text = raycastCamScript.contadorPildoras[Pildoras.TipoPildora.Azul].ToString();
                break;
            case Pildoras.TipoPildora.Naranja:
                pildoraNaranja.text = raycastCamScript.contadorPildoras[Pildoras.TipoPildora.Naranja].ToString();
                break;
            default:
                break;
        }

        // Ejecutar efectos de la p�ldora seleccionada
        switch ((Pildoras.TipoPildora)index)
        {
            case Pildoras.TipoPildora.Roja:
                AumentarDanio();
                tiempoRestantePildoraRoja = duracionPildoraRoja;
                StartCoroutine(ContarDuracionPildoraRoja());
                break;
            case Pildoras.TipoPildora.Azul:
                AumentarVidaMaxima();
                tiempoRestantePildoraAzul = duracionPildoraAzul;
                StartCoroutine(ContarDuracionPildoraAzul());
                break;
            case Pildoras.TipoPildora.Naranja:
                AumentarVelocidad();
                tiempoRestantePildoraNaranja = duracionPildoraNaranja;
                StartCoroutine(ContarDuracionPildoraNaranja());
                break;
            default:
                Debug.LogWarning("�ndice de p�ldora fuera de rango: " + index);
                return;
        }
    }



    // Funci�n para aumentar la vida m�xima del jugador
    void AumentarDanio()
    {
        if (gs != null)
        {
            gs.damage *= 2;
        }
    }

    void AumentarVelocidad()
    {
        if (player != null)
        {
            player.Speed *= (1 + pildoraScriptNaranja.aumentoVelocidad);
            player.model.UpdateSpeed(player.Speed);  // Actualiza la velocidad en PlayerModel
        }
    }

    void AumentarVidaMaxima()
    {
        if (player != null)
        {
            float vidaMaximaDeseada = 100f; // L�mite m�ximo de vida que deseamos
            player.model._currentLife += 100;

            // Asegurar que la vida no supere el l�mite m�ximo
            if (player.model._currentLife > vidaMaximaDeseada)
            {
                player.model._currentLife = vidaMaximaDeseada;
            }

            player.view.LifeView();
        }
    }

    IEnumerator ContarDuracionPildoraRoja()
    {
        while (tiempoRestantePildoraRoja > 0)
        {
            textoTiempoPildora.gameObject.SetActive(true);
            textoTiempoPildora.text = "Tiempo restante: " + tiempoRestantePildoraRoja.ToString("F0") + "s";
            yield return new WaitForSeconds(1f);
            tiempoRestantePildoraRoja -= 1f;
        }
        textoTiempoPildora.text = "";
        textoPildora.gameObject.SetActive(false);
        imagenPildora.gameObject.SetActive(false);
        // Desactivar efectos de la p�ldora Roja al finalizar el tiempo
        if (gs != null)
        {
            gs.damage /= 2;
        }
    }

    IEnumerator ContarDuracionPildoraNaranja()
    {
        while (tiempoRestantePildoraNaranja > 0)
        {
            textoTiempoPildora.gameObject.SetActive(true);
            textoTiempoPildora.text = "Tiempo restante: " + tiempoRestantePildoraNaranja.ToString("F0") + "s";
            yield return new WaitForSeconds(1f);
            tiempoRestantePildoraNaranja -= 1f;
        }
        textoTiempoPildora.text = "";
        textoPildora.gameObject.SetActive(false);
        imagenPildora.gameObject.SetActive(false);
        // Desactivar efectos de la p�ldora Naranja al finalizar el tiempo
        if (player != null)
        {
            player.Speed /= (1 + pildoraScriptNaranja.aumentoVelocidad);
            player.model.UpdateSpeed(player.Speed);  // Actualiza la velocidad en PlayerModel
        }
    }

    IEnumerator ContarDuracionPildoraAzul()
    {
        while (tiempoRestantePildoraAzul > 0)
        {
            textoTiempoPildora.gameObject.SetActive(true);
            textoTiempoPildora.text = "Tiempo restante: " + tiempoRestantePildoraAzul.ToString("F0") + "s";
            yield return new WaitForSeconds(1f);
            tiempoRestantePildoraAzul -= 1f;
        }
        textoTiempoPildora.text = "";
        textoPildora.gameObject.SetActive(false);
        imagenPildora.gameObject.SetActive(false);
        // Desactivar efectos de la p�ldora Azul al finalizar el tiempo

    }


}



