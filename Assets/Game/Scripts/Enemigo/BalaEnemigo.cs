using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaEnemigo : MonoBehaviour
{
    private float daño;

    public void SetDaño(int nuevoDaño)
    {
        daño = nuevoDaño;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("a");
            player.model.TakeDamage(daño);
            Destroy(gameObject); // Destruimos la bala después de impactar al jugador
        }
    }
}
