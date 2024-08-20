using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float velocidadMovimiento;
    public float distanciaMinima;
    public float amplitudCampoVision;
    public int vida;
    public int vidaMax;
    public int dañoPorGolpe;
    public float tiempoEntreGolpes = 3f;
    [SerializeField] private int roboDeVida;
    [SerializeField] private Player _player;
    public Transform jugador;

    public float tiempoUltimoGolpe;
    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;

    [SerializeField] protected Animator ani;
    [SerializeField] protected GameObject enemyRagdoll;
    [SerializeField] protected Collider coll;
    [SerializeField] private Collider excludeCollider; // Nuevo campo para el collider que se quiere excluir
    public bool isDead;

    void Start()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
        SetRagdollState(false);
        isDead = false;
    }

    void Update()
    {
        EnemyBehaviour();
    }

    private void Movimiento(Vector3 direccion)
    {
        // Asegurarse de que la dirección esté en el plano horizontal
        direccion.y = 0;

        // Verificar la distancia al jugador
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);
        if (distanciaAlJugador > distanciaMinima)
        {
            // Normalizar y mover el enemigo
            transform.Translate(direccion.normalized * velocidadMovimiento * Time.deltaTime, Space.World);

            // Mirar hacia el jugador horizontalmente
            Vector3 lookAtPosition = jugador.position;
            lookAtPosition.y = transform.position.y; // Mantener la misma altura

            transform.LookAt(lookAtPosition);
        }
    }

    public void EnemyBehaviour()
    {
        if (vida <= 0)
        {
            coll.enabled = false;
            ani.enabled = false;
            SetRagdollState(true);
            isDead = true;
            return;
        }

        if (!isDead)
        {
            Vector3 direccion = jugador.position - transform.position;

            if (Vector3.Angle(transform.forward, direccion) < amplitudCampoVision / 2f)
            {
                ani.SetBool("Walking", true);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direccion.normalized, out hit))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Movimiento(direccion);
                        Ataque();
                    }
                    else
                    {
                        ani.SetBool("Walking", false);
                    }
                }
            }
            else
            {
                ani.SetBool("Walking", false);
            }
        }
    }

    private void Ataque()
    {
        if (Vector3.Distance(transform.position, jugador.position) <= distanciaMinima && Time.time - tiempoUltimoGolpe >= tiempoEntreGolpes)
        {
            ani.SetTrigger("Attack");
            tiempoUltimoGolpe = Time.time;
        }
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bala"))
        {
            RecibirDanio(10);
            Destroy(other.gameObject);
        }
    }

    public void ResetearEnemigo()
    {
        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;
        vida = vidaMax;
        SetRagdollState(false);
        ani.enabled = true;
        coll.enabled = true;
        isDead = false;
        gameObject.SetActive(true);
    }

    private void SetRagdollState(bool state)
    {
        Rigidbody[] rigidbodies = enemyRagdoll.GetComponentsInChildren<Rigidbody>();
        Collider[] colliders = enemyRagdoll.GetComponentsInChildren<Collider>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !state;
        }

        foreach (Collider col in colliders)
        {
            if (col != excludeCollider) // Excluir el Collider asignado en el inspector
            {
                col.enabled = state;
            }
        }
    }
}







