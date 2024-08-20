using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puño : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Enemigo _enemigo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SaludJugador saludJugador = other.GetComponent<SaludJugador>();
            if (saludJugador != null)
            {
                _player.model.TakeDamage(_enemigo.dañoPorGolpe);
                _enemigo.tiempoUltimoGolpe = Time.time;
            }
        }
    }
}
