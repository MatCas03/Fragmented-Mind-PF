using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
public class Dash : MonoBehaviour
{
    [Header("Referencias")]
    public Transform orientacion;
    public Transform jugadorCam;
    private CharacterController characterController;
    private JugadorMov jugadorMov;

    [Header("Dash")]
    public float fuerzaDash;
    public float duracionDash;
    public float fuerzaParaArriba;

    [Header("Tiempo de espera")]
    public float enfriamientoDash = 5f;

    [Header("Sonido")]
    public AudioSource sonidoDash;

    [Header("Tecla para el dash")]
    public Slider sliderTiempoDash;
    public KeyCode dashKey = KeyCode.LeftShift;

    private bool puedeDashear = true;
    private float tiempoUltimoDash;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        jugadorMov = GetComponent<JugadorMov>();
    }

    private void Update()
    {
        if (!puedeDashear)
        {
            float tiempoTranscurrido = Time.time - tiempoUltimoDash;
            float tiempoRestante = Mathf.Max(0f, enfriamientoDash - tiempoTranscurrido);
            float completado = 1 - (tiempoRestante / enfriamientoDash);
            sliderTiempoDash.value = completado; 

            if (tiempoTranscurrido >= enfriamientoDash)
            {
                puedeDashear = true;
                sliderTiempoDash.value = 1; 
            }
        }

        if (Input.GetKeyDown(dashKey) && puedeDashear && !jugadorMov.dash)
            Deslizar();
    }

    private void Deslizar()
    {
        if (!puedeDashear) return;
        puedeDashear = false;
        tiempoUltimoDash = Time.time;

        jugadorMov.transform.localScale = jugadorMov.escalaReducidaDash;
        jugadorMov.dash = true;

        // Calcula la dirección del dash basada en la orientación actual del jugador
        Vector3 moveDirection = orientacion.forward * Input.GetAxis("Vertical") + orientacion.right * Input.GetAxis("Horizontal");
        moveDirection.Normalize();

        // player view
        if (sonidoDash != null)
            sonidoDash.Play();

        // Aplica el desplazamiento manualmente al CharacterController
        StartCoroutine(DashCoroutine(moveDirection));

        Invoke(nameof(ReiniciarElDash), duracionDash);
    }
    private IEnumerator DashCoroutine(Vector3 moveDirection)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duracionDash)
        {
            characterController.Move(moveDirection * fuerzaDash * Time.deltaTime);
            yield return null;
        }
    }
    private void ReiniciarElDash()
    {
        jugadorMov.dash = false;
        jugadorMov.RestablecerVelocidad();

        jugadorMov.transform.localScale = jugadorMov.escalaInicialModelo;
    }
    
}
