using System.Collections;
using UnityEngine;

public class ActivarFisicasPared : MonoBehaviour
{
    private Rigidbody rb;
    private Coroutine gravedadCoroutine; // Almacena la referencia a la corutina de gravedad activa

    [SerializeField] private GameObject balaExplosiva;
    [SerializeField] private float duracionGravedad; // Duración de la gravedad activada en segundos
    [SerializeField] private float duracionMasa;

    public CameraShake temblorCamara;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 100f;
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        BalaExplosiva balaExplosiva = other.GetComponent<BalaExplosiva>();
        if (balaExplosiva != null)
        {
            if (gravedadCoroutine != null)
            {
                StopCoroutine(gravedadCoroutine);
            }
            gravedadCoroutine = StartCoroutine(ActivarGravedadTemporalmente());

            //StartCoroutine(temblorCamara.Shaking());
        }
    }

    private IEnumerator ActivarGravedadTemporalmente()
    {
        rb.useGravity = true;
        rb.mass = 0.1f;
        rb.constraints = RigidbodyConstraints.None;

        yield return new WaitForSeconds(duracionMasa);
        rb.mass = 100f;

        yield return new WaitForSeconds(duracionGravedad);

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    }
}




