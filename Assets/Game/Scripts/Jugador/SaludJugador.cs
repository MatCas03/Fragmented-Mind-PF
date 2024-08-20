using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaludJugador : MonoBehaviour
{
    public int saludInicial = 100;
    public int saludActual;
    
    private bool PerdisteVariable = false;

    private void Update()
    {
        if (PerdisteVariable && Cursor.visible)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        saludActual = saludInicial;

    }
   
}



