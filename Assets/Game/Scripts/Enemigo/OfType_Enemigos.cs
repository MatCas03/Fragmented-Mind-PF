using UnityEngine;
using TMPro;
using System.Linq;

public class OfType_Enemigos : MonoBehaviour
{
    public TextMeshProUGUI textoCantidadEnemigosCercanos;
    public TextMeshProUGUI textoCantidadEnemigosDistancia;
    private int cantidadEnemigosCercanos;
    private int cantidadEnemigosDistancia;

    void Start()
    {
        ActualizarContadores();
    }

    void Update()
    {       
        int nuevaCantidadEnemigosCercanos = ContarEnemigosCercanos();
        int nuevaCantidadEnemigosDistancia = ContarEnemigosDistancia();

        if (nuevaCantidadEnemigosCercanos != cantidadEnemigosCercanos || nuevaCantidadEnemigosDistancia != cantidadEnemigosDistancia)
        {
            cantidadEnemigosCercanos = nuevaCantidadEnemigosCercanos;
            cantidadEnemigosDistancia = nuevaCantidadEnemigosDistancia;
            ActualizarContadores();
        }
    }

    int ContarEnemigosCercanos()
    {
        //IA2-LINQ - OfType
        var enemigo = GameObject.FindGameObjectsWithTag("Enemigo");
        var enemigosCercanos = enemigo.Select(enemigo => enemigo.GetComponent<Enemigo>()).OfType<Enemigo>();
        return enemigosCercanos.Count();
    }

    int ContarEnemigosDistancia()
    {
        var enemigo = GameObject.FindGameObjectsWithTag("EnemigoDispara");
        var enemigosDistancia = enemigo.Select(enemigo => enemigo.GetComponent<EnemigoDispara>()).OfType<EnemigoDispara>();
        return enemigosDistancia.Count();
    }

    void ActualizarContadores()
    {
        textoCantidadEnemigosCercanos.text = "Enemigos Cercanos: " + cantidadEnemigosCercanos.ToString();
        textoCantidadEnemigosDistancia.text = "Enemigos a Distancia: " + cantidadEnemigosDistancia.ToString();
    }

    public void EnemigoEliminado(bool esCercano)
    {
        if (esCercano)
            cantidadEnemigosCercanos--;
        else
            cantidadEnemigosDistancia--;

        ActualizarContadores();
    }
}