using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparar : MonoBehaviour
{
    public GameObject Gun;
    public GameObject bala;
    public Transform puntoDeAparicion;

    public float fuerzaDisparo;
    public float ratioDisparo = 0.5f;
    public bool esAutomatica = false;

    private float ratioDisparoTiempo = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DispararBala()
    {
        if (Time.time > ratioDisparoTiempo)
        {
            Debug.Log("Disparando bala...");
            //var bala = Factory.Instance.GetObjectFromPool();

            bala.transform.position = puntoDeAparicion.position;
            bala.transform.rotation = puntoDeAparicion.rotation;

            //Fuerza 
            bala.GetComponent<Rigidbody>().AddForce(puntoDeAparicion.forward * (fuerzaDisparo * 1000));
            bala.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ratioDisparoTiempo = Time.time + ratioDisparo;
        }
    }
}

