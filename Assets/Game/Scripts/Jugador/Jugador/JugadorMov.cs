using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JugadorMov : MonoBehaviour
{
    public CharacterController controller;

    [Header("Movimiento")]
    public float velocidadJugador = 10f;

    [Header("Salto")]
    public float gravedad = -9.8f;
    public float fuerzaSalto = 8f;

    [Header("Sonido")]
    public AudioSource sonidoSalto;
    public AudioSource sonidoPasos;

    public Transform verificarSuelo;
    public float sueloDis = 0.3f;
    public LayerMask sueloMask;

    Vector3 velocidadGravedad;
    Vector3 velocidadMovimiento;

    [Header("Dash")]
    public bool dash = false;
    public float velocidadDash = 20f;
    public float duracionDash = 0.5f;
    public Vector3 escalaInicialModelo = Vector3.one;
    public Vector3 escalaReducidaDash = new Vector3(0.5f, 0.5f, 0.5f);

    bool suelo;

    //[Header("Contador de enemigos")]
    //public TextMeshProUGUI contadorEnemigosText;
    //public LayerMask capaEnemigo;
    //private (int, List<GameObject>) enemigosEliminadosTupla; //IA2-LINQ - Tuplas


    //void Start()
    //{
    //    enemigosEliminadosTupla = (0, new List<GameObject>());
    //    ActualizarContadorEnemigos();
    //}

    void Update()
    {
        suelo = Physics.CheckSphere(verificarSuelo.position, sueloDis, sueloMask);

        if (suelo && velocidadGravedad.y < 0)
        {
            velocidadGravedad.y = -2f;
        }

        MoverJugador();
    }

    private void MoverJugador()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        velocidadMovimiento = transform.right * x + transform.forward * z;
        velocidadMovimiento *= velocidadJugador;

        if (controller.velocity.magnitude > 0 && suelo)
        {
            if (!sonidoPasos.isPlaying)
            {
                sonidoPasos.Play();
            }
        }
        else
        {
            sonidoPasos.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Space) && suelo)
        {
            velocidadGravedad.y = Mathf.Sqrt(fuerzaSalto * -2 * gravedad);

            if (sonidoSalto != null)
                sonidoSalto.Play();
        }

        velocidadGravedad.y += gravedad * Time.deltaTime;

        controller.Move((velocidadMovimiento + velocidadGravedad) * Time.deltaTime);

        if (!dash)
        {
            controller.Move((velocidadMovimiento + velocidadGravedad) * Time.deltaTime);
        }
    }

    //public void AgregarEnemigoEliminado(GameObject enemigo)
    //{
    //    var listaEnemigos = enemigosEliminadosTupla.Item2;
    //    listaEnemigos.Add(enemigo);
    //    enemigosEliminadosTupla = (listaEnemigos.Count, listaEnemigos);
    //  
    //    int enemigosEliminados = enemigosEliminadosTupla.Item1;
    //    enemigosEliminadosTupla = (enemigosEliminados, listaEnemigos);
    //
    //    ActualizarContadorEnemigos();
    //}

    //private void ActualizarContadorEnemigos()
    //{
    //    contadorEnemigosText.text = "Enemigos Eliminados: " + enemigosEliminadosTupla.Item1.ToString();
    //}

    public void RestablecerVelocidad()
    {
        velocidadJugador = 3f;
    }
}
