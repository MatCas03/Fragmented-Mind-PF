using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Objetivo : MonoBehaviour
{
    [SerializeField] private TextMeshPro _objetivoTEXT;

    private void Start()
    {
        _objetivoTEXT.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            _objetivoTEXT.enabled = true;
        }
    }
}
