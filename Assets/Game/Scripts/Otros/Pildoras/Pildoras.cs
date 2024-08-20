using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Pildoras : MonoBehaviour
{
    public enum TipoPildora { Roja, Naranja, Azul };
    public TipoPildora tipoPildora;
    public int aumentoVida;
    public float aumentoVelocidad;
    public int aumentoDanio;
    [SerializeField] private TextMeshProUGUI tiempoRestanteTexto;
    public float duracionBonificacion = 2f;
    private GunSystem gs;
    private bool recogida = false;

    private float velocidadInicial;
    private int danioInicial;

    private Renderer rend;

    public TextMeshProUGUI pildorasText;
    private List<string> nombresPildorasRecogidas = new List<string>();

    public Image imagenPildoras;
    public List<Sprite> spritesPildoras;

    public Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gs = FindObjectOfType<GunSystem>();
        rend = GetComponent<Renderer>();
    }

    private void Start()
    {
        tiempoRestanteTexto.gameObject.SetActive(false);
        pildorasText.gameObject.SetActive(false);

        if (player != null)
            velocidadInicial = player.Speed;

        if (gs != null)
            danioInicial = gs.damage;
    }

    public void RecogerPildora()
    {
        if (!recogida)
        {
            IniciarBonificacion();
            recogida = true;

            // Agregar el nombre de la píldora recogida a la lista
            string nombrePildora = gameObject.name;
            nombresPildorasRecogidas.Add(nombrePildora);
            imagenPildoras.gameObject.SetActive(true);

            //IA2-LINQ - Zip
            var imagenPildora = nombresPildorasRecogidas.Zip(spritesPildoras, (nombre, sprite) => new { Nombre = nombre, Sprite = sprite })
                //IA2-LINQ - FirstOrDefault
                .FirstOrDefault(pair => pair.Nombre == nombrePildora);

            if (imagenPildora != null)
            {
                // Asigna la imagen de la píldora correspondiente
                imagenPildoras.sprite = imagenPildora.Sprite;
            }
        }
    }

    private void IniciarBonificacion()
    {
        StartCoroutine(ActivarBonificacion());
        rend.enabled = false; // Oculta la píldora cuando se recoge
    }

    private IEnumerator ActivarBonificacion()
    {
        float tiempoInicio = Time.time;
        tiempoRestanteTexto.gameObject.SetActive(true);
        pildorasText.gameObject.SetActive(true);

        switch (tipoPildora)
        {
            case TipoPildora.Roja:
                AumentarVidaMaxima();
                break;
            case TipoPildora.Naranja:
                AumentarVelocidad();
                break;
            case TipoPildora.Azul:
                AumentarDanio();
                break;
        }

        while (Time.time < tiempoInicio + duracionBonificacion)
        {
            float tiempoRestante = (tiempoInicio + duracionBonificacion) - Time.time;
            tiempoRestanteTexto.text = $"Tiempo restante: {tiempoRestante:F1}s";
            yield return null;
        }

        DetenerBonificacion();
        gameObject.SetActive(false);
    }

    private void DetenerBonificacion()
    {
        tiempoRestanteTexto.gameObject.SetActive(false);
        pildorasText.gameObject.SetActive(false);
        // Desactivar la imagen de la píldora al finalizar la bonificación
        imagenPildoras.gameObject.SetActive(false);
        switch (tipoPildora)
        {
            case TipoPildora.Naranja:
                RevertirAumentoVelocidad();
                break;
            case TipoPildora.Azul:
                RevertirAumentoDanio();
                break;
        }
    }

    private void AumentarVidaMaxima()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.model._currentLife += aumentoVida;
            if (player.model._currentLife > player.model._startLife)
            {
                player.model._currentLife = player.model._startLife;
            }
            player.view.LifeView();
        }
    }

    private void AumentarVelocidad()
    {
        if (player != null)
        {
            player.Speed *= (1 + aumentoVelocidad);
            player.model.UpdateSpeed(player.Speed);  // Actualiza la velocidad en PlayerModel
        }
    }

    private void AumentarDanio()
    {
        if (gs != null)
        {
            gs.damage *= aumentoDanio;
        }
    }

    private void RevertirAumentoDanio()
    {
        if (gs != null)
        {
            gs.damage = danioInicial;
        }
    }

    private void RevertirAumentoVelocidad()
    {
        if (player != null)
        {
            player.Speed = velocidadInicial;
            player.model.UpdateSpeed(player.Speed);  // Revertir la velocidad en PlayerModel
        }
    }

    public void ActualizarHUD()
    {
        // Obtener una lista plana de todos los nombres de píldoras recogidas
        //IA2-LINQ - Selectmany
        string textoPildoras = string.Join("\n", nombresPildorasRecogidas.SelectMany(nombre => new[] { nombre }));

        pildorasText.text = "Pildora agarrada:\n" + textoPildoras;
    }
}




