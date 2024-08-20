using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemigoDispara : Enemigo
{
    private float tiempoUltimoDisparo;

    public int dañoPorDisparo;
    public float tiempoEntreDisparo = 3f;
    public GameObject objetoDisparo;
    public Transform puntoDisparo;
    public float tiempoDeVidaBala = 3f;
    public float velocidadBala;
    public float distanciaMinimaDisparo;

    private void Update()
    {
        EnemyBehaviour(); 

        CheckPlayer();
    }

    private void CheckPlayer()
    {
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= distanciaMinimaDisparo && Vector3.Angle(transform.forward, jugador.position - transform.position) < amplitudCampoVision / 2f)
        {
            // Asegurarse de que el enemigo solo mire horizontalmente hacia el jugador
            Vector3 lookAtPosition = jugador.position;
            lookAtPosition.y = transform.position.y; // Mantener la misma altura

            if (!isDead) 
            {
                transform.LookAt(lookAtPosition);
            }
            
            ani.SetTrigger("Throw");
        }
    }


    public void ThrowObject()
    {
        Vector3 direccionAlJugador = (jugador.position - puntoDisparo.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(puntoDisparo.position, direccionAlJugador, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (objetoDisparo != null && puntoDisparo != null)
                {
                    GameObject bala = Instantiate(objetoDisparo, puntoDisparo.position, Quaternion.identity);
                    Rigidbody balaRigidbody = bala.GetComponent<Rigidbody>();
                    if (balaRigidbody != null)
                    {
                        balaRigidbody.velocity = direccionAlJugador * velocidadBala;
                    }

                    BalaEnemigo balaEnemigo = bala.AddComponent<BalaEnemigo>();
                    if (balaEnemigo != null)
                    {
                        balaEnemigo.SetDaño(dañoPorDisparo);
                    }

                    Destroy(bala, tiempoDeVidaBala);
                }
                tiempoUltimoDisparo = Time.time;
            }
        }
    }
}
