using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class RaycastCam : MonoBehaviour
{
    [SerializeField] private float fuerzaPatada;
    [SerializeField] private float distanciaMax;
    [SerializeField] private LayerMask layerPuerta;
    private Rigidbody puertaRb;
    private Rigidbody enemigoRB;
    [SerializeField] private GameObject textoInteract;
    [SerializeField] private GameObject textoKick;
    [SerializeField] private MenuPausa MenuPausa;
    [SerializeField] private GameObject Linterna;
    [SerializeField] private ControlLinterna _linterna;
    [SerializeField] private GameObject CabezaLinterna;
    [SerializeField] private GameObject flashlightICON;
    [SerializeField] private LayerMask layerLinterna;
    [SerializeField] private LayerMask Arma;
    [SerializeField] private LayerMask Pila;
    [SerializeField] private LayerMask Generador;
    [SerializeField] private GameObject[] luces;
    [SerializeField] private PickUpController pickUp;
    [SerializeField] private LayerMask Enemigo;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject legs;
    [SerializeField] private GameObject shotGun;
    [SerializeField] private int cantidadDePiezasBomba;
    [SerializeField] private float DuracionMesaBomba = 6f; // Time required to hold the key down
    private float TiempoMantenido = 0f;
    [SerializeField] private GameObject objetoAsignado; // El objeto que se va a instanciar
    [SerializeField] private Transform puntoDeSpawn; // El punto de spawn asignado
    [SerializeField] private GameObject objetivo;
    [SerializeField] private float cooldownTimeKick; // Tiempo de enfriamiento en segundos
    private float lastKickTime;


    [Header("Sonido")]
    [SerializeField] private AudioClip sonidoPatada;
    [SerializeField] private AudioClip sonidoPatadaViento;
    public AudioClip sonidoAgarrar;
    public AudioClip sonidoAgarrarShotGun;
    public AudioClip sonidoPatada1;
    public AudioClip sonidoPatada2;

    [Header("UI")]
    public Slider progressBar;
    public Slider progressBarBomba;

    private float tiempoPresionandoE;
    private bool sobreGenerador;
    private bool generadorActivado;

    [SerializeField] private RawImage rawImage1;
    [SerializeField] private RawImage rawImage2;
    [SerializeField] private RawImage newRawImage1;
    [SerializeField] private RawImage newRawImage2;
    [SerializeField] private RawImage newRawImage3;
    [SerializeField] private RawImage newRawImage4;

    public Dictionary<Pildoras.TipoPildora, int> contadorPildoras = new Dictionary<Pildoras.TipoPildora, int>();

    public Inventario inventarioScript;

    [SerializeField] private Animator animator;

    public int contadorPiezasBomba = 0; // Contador para las piezas de bomba

    private void Start()
    {
        Time.timeScale = 1;

        legs.SetActive(false);

        newRawImage1.enabled = false;
        newRawImage2.enabled = false;
        newRawImage3.enabled = false;
        newRawImage4.enabled = false;

        contadorPildoras.Add(Pildoras.TipoPildora.Roja, 0);
        contadorPildoras.Add(Pildoras.TipoPildora.Naranja, 0);
        contadorPildoras.Add(Pildoras.TipoPildora.Azul, 0);
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * distanciaMax, Color.red, 0.1f);
        Patada();
        PickUp();
        RecogerPildora();
        RecogerPila();
        EncenderLuces();
        RecogerPiezasBomba();
        MesaBomba();

        if (!sobreGenerador)
        {
            tiempoPresionandoE = 0f;
            progressBar.value = 0f;
            progressBar.gameObject.SetActive(false);
        }
        sobreGenerador = false;
    }

    private void RecogerPildora()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax))
        {
            Pildoras pildora = hit.collider.GetComponent<Pildoras>();
            if (pildora != null && Input.GetKeyDown(KeyCode.E) && MenuPausa.Pausa == false)
            {
                SoundFXManager.Instance.PlaySoundFXClip(sonidoAgarrar, _player.transform, .75f);

                hit.collider.gameObject.SetActive(false);

                contadorPildoras[pildora.tipoPildora]++;
                Debug.Log($"Píldora de tipo {pildora.tipoPildora} recogida. Contador: {contadorPildoras[pildora.tipoPildora]}");

                switch (pildora.tipoPildora)
                {
                    case Pildoras.TipoPildora.Roja:
                        inventarioScript.pildoraRoja.text = contadorPildoras[pildora.tipoPildora].ToString();
                        break;
                    case Pildoras.TipoPildora.Azul:
                        inventarioScript.pildoraAzul.text = contadorPildoras[pildora.tipoPildora].ToString();
                        break;
                    case Pildoras.TipoPildora.Naranja:
                        inventarioScript.pildoraNaranja.text = contadorPildoras[pildora.tipoPildora].ToString();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void RecogerPiezasBomba()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax, LayerMask.GetMask("PiezasBomba")))
        {
            if (hit.collider != null && Input.GetKeyDown(KeyCode.E) && MenuPausa.Pausa == false)
            {
                SoundFXManager.Instance.PlaySoundFXClip(sonidoAgarrar, _player.transform, .75f);

                hit.collider.gameObject.SetActive(false);

                contadorPiezasBomba++;
                Debug.Log($"Pieza de bomba recogida. Contador: {contadorPiezasBomba}");

                // Aquí puedes actualizar la UI o hacer algo con el contador de piezas de bomba
            }
        }
    }

    private void MesaBomba()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax, LayerMask.GetMask("MesaBomba")))
        {
            Debug.Log("MesaBomba detected within range.");

            if (hit.collider != null && MenuPausa.Pausa == false)
            {
                if (contadorPiezasBomba >= cantidadDePiezasBomba) // Check if there are at least 6 bomb pieces
                {
                    if (Input.GetKey(KeyCode.E))
                    {
                        Debug.Log("Key E is being held down.");

                        if (!progressBarBomba.gameObject.activeInHierarchy)
                        {
                            Debug.Log("Activating progress bar.");
                            progressBarBomba.gameObject.SetActive(true);
                        }

                        TiempoMantenido += Time.deltaTime;
                        progressBarBomba.value = Mathf.Clamp01(TiempoMantenido / DuracionMesaBomba);

                        Debug.Log($"Progress: {progressBarBomba.value * 100}%");

                        if (TiempoMantenido >= DuracionMesaBomba)
                        {
                            Debug.Log("Sufficient bomb pieces collected. Setting object active.");
                            objetoAsignado.SetActive(true); // Activate the assigned object

                            contadorPiezasBomba -= cantidadDePiezasBomba; // Deduct pieces used
                            Debug.Log($"Bomb pieces remaining: {contadorPiezasBomba}");

                            TiempoMantenido = 0f;
                            progressBarBomba.gameObject.SetActive(false);
                            objetivo.SetActive(false);
                        }
                    }
                    else
                    {
                        Debug.Log("Key E released before completion.");
                        TiempoMantenido = 0f;
                        progressBarBomba.value = 0f;
                        progressBarBomba.gameObject.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("Not enough bomb pieces collected. Minimum 6 required.");
                    TiempoMantenido = 0f;
                    progressBarBomba.value = 0f;
                    progressBarBomba.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Paused or no valid collider detected.");
                TiempoMantenido = 0f;
                progressBarBomba.value = 0f;
                progressBarBomba.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("MesaBomba not detected within range.");
            TiempoMantenido = 0f;
            progressBarBomba.value = 0f;
            progressBarBomba.gameObject.SetActive(false);
        }
    }



    private void RecogerPila()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax, Pila))
        {
            if (Input.GetKeyDown(KeyCode.E) && MenuPausa.Pausa == false)
            {
                SoundFXManager.Instance.PlaySoundFXClip(sonidoAgarrar, _player.transform, .75f);
                ControlLinterna controlLinterna = FindObjectOfType<ControlLinterna>();
                if (controlLinterna != null)
                {
                    if (controlLinterna.cantidadPilas < 3 || controlLinterna.EnergiaActual < controlLinterna.EnergiaMaxima) 
                    {
                        if (controlLinterna.cantidadPilas < 3) 
                        {
                            controlLinterna.cantidadPilas++;
                            print("¡Recogiste una pila! Ahora tienes " + controlLinterna.cantidadPilas + " pilas.");
                        }
                        
                        controlLinterna.EnergiaActual += controlLinterna.EnergiaMaxima / 3; 
                        if (controlLinterna.EnergiaActual > controlLinterna.EnergiaMaxima)
                        {
                            controlLinterna.EnergiaActual = controlLinterna.EnergiaMaxima;
                        }
                        
                        hit.collider.gameObject.SetActive(false);
                        controlLinterna.ActualizarTextoBateria();
                    }
                    else
                    {
                        print("Ya tienes tres pilas y la energía está al máximo, no puedes recoger más.");
                    }
                }
                else
                {
                    print("El control de linterna no está asignado en el script AgarrarPilas.");
                }
            }
        }
    }

    private void EncenderLuces()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax, Generador))
        {
            if (generadorActivado)
                return;

            sobreGenerador = true;
            if (Input.GetKey(KeyCode.E) && MenuPausa.Pausa == false)
            {
                if (!progressBar.gameObject.activeInHierarchy)
                {
                    progressBar.gameObject.SetActive(true);
                }

                tiempoPresionandoE += Time.deltaTime;
                progressBar.value = tiempoPresionandoE;
                if (tiempoPresionandoE >= 6f)
                {

                    foreach (GameObject luz in luces)
                    {
                        luz.SetActive(true);
                    }
                    print("¡Generador activado y luces encendidas!");
                    generadorActivado = true;
                    progressBar.gameObject.SetActive(false);
                    tiempoPresionandoE = 0f;
                    progressBar.value = 0f;
                }
            }
            else
            {

                tiempoPresionandoE = 0f;
                progressBar.value = 0f;
                progressBar.gameObject.SetActive(false);
            }
        }
    }

    private async void Patada()
    {
        if (Time.time - lastKickTime < cooldownTimeKick) return; // Verifica si el tiempo de enfriamiento ha pasado

        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Q) && !MenuPausa.Pausa)
        {
            lastKickTime = Time.time; // Actualiza el tiempo de la última patada

            legs.SetActive(true);
            animator.SetTrigger("PlayerKick");
            SoundFXManager.Instance.PlaySoundFXClip(sonidoPatadaViento, _player.transform, 1f);

            if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax, layerPuerta))
            {
                puertaRb = hit.collider.GetComponent<Rigidbody>();
                if (puertaRb != null)
                {
                    SoundFXManager.Instance.PlaySoundFXClip(sonidoPatada, _player.transform, .25f);

                    puertaRb.useGravity = true;
                    puertaRb.constraints = RigidbodyConstraints.None;
                    puertaRb.AddForce(-hit.normal * fuerzaPatada, ForceMode.VelocityChange);

                    CancelInvoke("DesactivarGravedad");
                    Invoke("DesactivarGravedad", 5f);
                }
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax, Enemigo))
            {
                enemigoRB = hit.collider.GetComponent<Rigidbody>();
                if (enemigoRB != null)
                {
                    SoundFXManager.Instance.PlaySoundFXClip(sonidoPatada1, _player.transform, .50f);

                    enemigoRB.useGravity = true;
                    enemigoRB.AddForce(-hit.normal * fuerzaPatada, ForceMode.VelocityChange);

                    CancelInvoke("DesactivarGravedad");
                    Invoke("DesactivarGravedad", 5f);
                }
            }
            await Task.Delay(300);
            legs.SetActive(false);
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax))
        {

            // Comprueba si el objeto golpeado tiene la etiqueta "Pickeable" o "Pildoras"
            bool isPickeableOrPildoras = hit.collider.CompareTag("Pickeable") || hit.collider.CompareTag("Pildoras") || hit.collider.CompareTag("Pateable");

            if (hit.collider.CompareTag("Pickeable") || hit.collider.CompareTag("Pildoras"))
            {
                // Configura el estado de los elementos según si el objeto es "Pickeable" o "Pildoras"
                textoInteract.SetActive(isPickeableOrPildoras);
                rawImage1.enabled = !isPickeableOrPildoras;
                rawImage2.enabled = !isPickeableOrPildoras;
                newRawImage1.enabled = isPickeableOrPildoras;
                newRawImage2.enabled = isPickeableOrPildoras;
                newRawImage3.enabled = isPickeableOrPildoras;
                newRawImage4.enabled = isPickeableOrPildoras;
            }
            else if (hit.collider.CompareTag("Pateable"))
            {
                // Configura el estado de los elementos según si el objeto es "Pickeable" o "Pildoras"
                textoKick.SetActive(isPickeableOrPildoras);
                rawImage1.enabled = !isPickeableOrPildoras;
                rawImage2.enabled = !isPickeableOrPildoras;
                newRawImage1.enabled = isPickeableOrPildoras;
                newRawImage2.enabled = isPickeableOrPildoras;
                newRawImage3.enabled = isPickeableOrPildoras;
                newRawImage4.enabled = isPickeableOrPildoras;
            }
            else
            {
                // Si no hay ningún objeto golpeado por el raycast, restablece los estados a los valores por defecto
                textoInteract.SetActive(false);
                textoKick.SetActive(false);
                rawImage1.enabled = true;
                rawImage2.enabled = true;
                newRawImage1.enabled = false;
                newRawImage2.enabled = false;
                newRawImage3.enabled = false;
                newRawImage4.enabled = false;
            }
        }
        else
        {
            // Si no hay ningún objeto golpeado por el raycast, restablece los estados a los valores por defecto
            textoInteract.SetActive(false);
            textoKick.SetActive(false);
            rawImage1.enabled = true;
            rawImage2.enabled = true;
            newRawImage1.enabled = false;
            newRawImage2.enabled = false;
            newRawImage3.enabled = false;
            newRawImage4.enabled = false;
        }
    }

    private void PickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax, layerLinterna))
        {
            if (Input.GetKeyDown(KeyCode.E) && MenuPausa.Pausa == false)
            {
                _linterna.TextoPorcentajeBateria.enabled = true;
                SoundFXManager.Instance.PlaySoundFXClip(sonidoAgarrar, _player.transform, .75f);
                Linterna.SetActive(false);
                CabezaLinterna.SetActive(true);
                flashlightICON.SetActive(true);
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax, Arma))
        {
            Debug.Log("a");
            if (Input.GetKeyDown(KeyCode.E) && MenuPausa.Pausa == false && hit.collider.CompareTag("Pickeable"))
            {
                Debug.Log("b");
                SoundFXManager.Instance.PlaySoundFXClip(sonidoAgarrar, _player.transform, .75f);
                shotGun.SetActive(false);
                SoundFXManager.Instance.PlaySoundFXClip(sonidoAgarrarShotGun, _player.transform, .75f);
                pickUp.PickUp();
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaMax, Arma))
        {
            if (Input.GetKeyDown(KeyCode.E) && MenuPausa.Pausa == false && hit.collider.CompareTag("Pildoras"))
            {
                SoundFXManager.Instance.PlaySoundFXClip(sonidoAgarrar, _player.transform, .75f);
                pickUp.PickUp();
            }
        }
    }

    private void DesactivarGravedad()
    {
        puertaRb.useGravity = false;
        puertaRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }
}





