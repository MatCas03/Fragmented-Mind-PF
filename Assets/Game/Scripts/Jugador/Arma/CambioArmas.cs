using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambioArmas : MonoBehaviour
{
    public GameObject[] armas;
    private int armaActualIndex = 0;

    void Update()
    {
        // Obtener el input de la rueda del mouse
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        // Cambiar el índice del arma actual basado en el input de la rueda del mouse
        if (scrollWheelInput > 0f) // Rueda del mouse hacia arriba
        {
            armaActualIndex = (armaActualIndex - 1 + armas.Length) % armas.Length;
        }
        else if (scrollWheelInput < 0f) // Rueda del mouse hacia abajo
        {
            armaActualIndex = (armaActualIndex + 1) % armas.Length;
        }

        // Desactivar todas las armas
        foreach (GameObject arma in armas)
        {
            arma.SetActive(false);
        }

        // Activar el arma actual
        armas[armaActualIndex].SetActive(true);
    }
}
