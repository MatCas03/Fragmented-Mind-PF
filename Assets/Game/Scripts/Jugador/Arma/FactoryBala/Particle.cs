using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] private float _tiempoInicial;
    [SerializeField] private float _velocity;

    private static List<Particle> balasClonadas = new List<Particle>();
    //private ParticleSystem ParticleSystem;
    private float _tiempoActual;

    private void Update()
    {
        _tiempoActual -= Time.deltaTime;

        if (_tiempoActual <= 0)
        {
            Factory.Instance.ReturnObjectToPool(this);
        }

        transform.position += transform.forward * _velocity * Time.deltaTime;
    }

    public void Reset()
    {
        _tiempoActual = _tiempoInicial;
        //ParticleSystem?.Play();
    }

    public static void TurnOn(Particle b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Particle b)
    {
        b.gameObject.SetActive(false);
    }

    private void Awake()
    {
        balasClonadas.Add(this);
        //ParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void OnDestroy()
    {
        balasClonadas.Remove(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Factory.Instance.ReturnObjectToPool(this);
    }
}
