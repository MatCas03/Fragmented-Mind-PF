using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalaEnemigo : MonoBehaviour
{
    private float da�o;

    public void SetDa�o(int nuevoDa�o)
    {
        da�o = nuevoDa�o;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("a");
            player.model.TakeDamage(da�o);
            Destroy(gameObject); // Destruimos la bala despu�s de impactar al jugador
        }
    }
}
