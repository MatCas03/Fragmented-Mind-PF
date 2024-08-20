using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorCam : MonoBehaviour
{
    public float sensitivity = 100f;  // Asegúrate de que esté dentro del rango de 0 a 250
    private float rotacionX = 0f;

    public Transform orientacion;
    public float maxRayDistance = 5f;
    public LayerMask interactableLayer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

        orientacion.Rotate(Vector3.up * mouseX);

        DetectButton();
    }

    void DetectButton()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxRayDistance, interactableLayer))
            {
                ButtonInteractable button = hit.transform.GetComponent<ButtonInteractable>();
                if (button != null)
                {
                    button.OnInteract();
                }
            }
        }
    }
}



