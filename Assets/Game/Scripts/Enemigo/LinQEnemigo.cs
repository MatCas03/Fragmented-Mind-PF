using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class LinQEnemigo : MonoBehaviour
{
    private List<Transform> enemigos;
    public Transform jugador;
    public TextMeshProUGUI TextoEstadisticas;
    public float distanciaMinimaMostrarEstadisticas = 10f;

    void Start()
    {
        //IA2-LINQ - Select - Tolist
        enemigos = GameObject.FindGameObjectsWithTag("Enemigo")
                               .Select(e => e.transform)
                               .ToList();
        enemigos.AddRange(GameObject.FindGameObjectsWithTag("EnemigoDispara")
                                    .Where(e => e.GetComponent<EnemigoDispara>() != null)
                                    .Select(e => e.transform));
    }

    void Update()
    {
        //IA2-LINQ - Where - Tolist
        enemigos = enemigos.Where(e => e != null).ToList();

        // Verifica si hay enemigos dentro del rango de distancia mínima
        //IA2-LINQ - Any
        bool hayEnemigosCercanos = enemigos.Any(e => e != null && Vector3.Distance(e.position, jugador.position) <= distanciaMinimaMostrarEstadisticas);

        if (hayEnemigosCercanos)
        {
            // Filtra los enemigos dentro del rango de distancia mínima
            var enemigosCercanos = enemigos.Where(e => e != null && Vector3.Distance(e.position, jugador.position) <= distanciaMinimaMostrarEstadisticas);

            // Encuentra el enemigo más cercano dentro del rango
            //IA2-LINQ - OrderBy - FirstOrDefault
            Transform enemigoMasCercano = enemigosCercanos.OrderBy(e => Vector3.Distance(e.position, jugador.position)).FirstOrDefault();

            string estadisticas = "";

            float distanciaAlJugador = Vector3.Distance(enemigoMasCercano.position, jugador.position);

            string tipoEnemigo;
            int vidaEnemigo;
            if (enemigoMasCercano.GetComponent<Enemigo>() != null)
            {
                tipoEnemigo = "Enemigo de cercanía";
                //vidaEnemigo = enemigoMasCercano.GetComponent<Enemigo>().vida;
            }
            else if (enemigoMasCercano.GetComponent<EnemigoDispara>() != null)
            {
                tipoEnemigo = "Enemigo de disparo";
                //vidaEnemigo = enemigoMasCercano.GetComponent<Enemigo>().vida;
            }
            else
            {
                tipoEnemigo = "Tipo desconocido";
                vidaEnemigo = 0;
            }

            //estadisticas += $"Tipo de enemigo: {tipoEnemigo}, Distancia al jugador: {distanciaAlJugador}, Vida: {vidaEnemigo}\n";

            TextoEstadisticas.text = estadisticas;
        }
        else
        {
            TextoEstadisticas.text = "No hay enemigos cercanos.";
        }
    }
}













