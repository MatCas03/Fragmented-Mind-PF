using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluides : MonoBehaviour
{
    [SerializeField] private float movimientoSuave;
    [SerializeField] private float multiplicador;
 
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplicador;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplicador;

        Quaternion RotacionX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion RotacionY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion rotacion = RotacionX * RotacionY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotacion, movimientoSuave * Time.deltaTime);
    }
}
