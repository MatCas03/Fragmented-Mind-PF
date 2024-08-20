using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public float waitTime = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Vision"))
        {
            StartCoroutine(HandleSpawnSequence());
        }
    }

    private IEnumerator HandleSpawnSequence()
    {
        // Mover al jugador al primer punto de spawn
        transform.position = spawnPoint1.position;
        // Esperar por el tiempo especificado
        yield return new WaitForSeconds(waitTime);
        // Mover al jugador al segundo punto de spawn
        transform.position = spawnPoint2.position;
    }
}
