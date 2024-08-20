using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DynamiteSetActive : MonoBehaviour
{
    [SerializeField] private GameObject _dynamite;
    [SerializeField] private TextMeshProUGUI _dynamiteText;
    [SerializeField] private RaycastCam _raycastCam;

    private void Start()
    {
        _dynamite.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            _dynamite.SetActive(true);
        }
    }

    private void Update()
    {
        _dynamiteText.text = _raycastCam.contadorPiezasBomba.ToString();
    }
}
