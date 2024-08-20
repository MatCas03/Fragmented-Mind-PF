using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [field: SerializeField] public float StartLife { get; private set; }
    [field: SerializeField] public float Speed { get; set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float DashForce { get; private set; }
    [field: SerializeField] public int DashDuration { get; private set; }
    [field: SerializeField] public float Gravity { get; private set; }
    [field: SerializeField] public TextMeshPro LifeText { get; private set; }
    [field: SerializeField] public Image BloodCam { get; private set; }
    [field: SerializeField] public GameObject LoosePannel { get; private set; }
    [field: SerializeField] public AudioClip JumpSound { get; private set; }
    [field: SerializeField] public AudioClip[] StepsSounds { get; private set; }
    [field: SerializeField] public AudioClip DashSound { get; private set; }
    [field: SerializeField] public AudioClip VoiceSound { get; private set; }
    [field: SerializeField] public CameraShake CameraShake { get; private set; }

    public PlayerModel model;
    public PlayerView view;
    private PlayerController _controller;
    
    public Transform verifyFloor;
    public LayerMask floorLayer;

    [SerializeField] private MenuPausa menuPausa;
    [SerializeField] private string sceneReset;
    [SerializeField] private string sceneLoad;
    [SerializeField] private ListaDeEnemigos listaDeEnemigosTUTORIAL;
    [SerializeField] private ListaDeEnemigos listaDeEnemigosLEVEL1;
    [SerializeField] private ListaDeEnemigos listaDeEnemigosLEVEL2;
    [SerializeField] private ListaDeEnemigos listaDeEnemigosLEVEL3;

    private float originalVoiceVolume;
    private float originalStepsVolume;
    private float originalDashVolume;

    private void Awake()
    {
        model = new PlayerModel(this);
        view = new PlayerView(this);
        _controller = new PlayerController(model);

        model.OnMovement += view.MovementSound;
        model.OnLifeChange += view.LifeView;
        model.OnDash += view.DashSound;
        model.OnDead += view.PlayerDead;
        model.OnDead += DisableComponent;
    }

    void Update()
    {
        if (menuPausa.Pausa == false)
        {
            model.CheckGround();
            model.Gravity();
            _controller.UpdateKeys();
            _controller.Keys();
        }
    }

    void DisableComponent()
    {
        enabled = false;
    }

    public void ReiniciarEscena()
    {
        SceneManager.LoadScene(sceneReset);
    }

    private Vector3 lastCheckpointPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            lastCheckpointPosition = other.transform.position;
            Debug.Log("¡Checkpoint alcanzado! Posición guardada: " + lastCheckpointPosition);
        }
        else if (other.CompareTag("Meta"))
        {    
            SceneManager.LoadScene(sceneLoad);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ReiniciarCheckpoint()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Restaurar posición del último checkpoint
        transform.position = lastCheckpointPosition;

        // Restaurar la salud al valor inicial
        model._currentLife = model._startLife;

        // Volver a habilitar el componente para permitir que el jugador vuelva a controlarlo
        enabled = true;

        // Actualizar la visualización de la salud
        view.LifeView();

        view._p.enabled = true;
        view._jc.enabled = true;
        view._rc.enabled = true;

        // Activar y reposicionar enemigos
        if (listaDeEnemigosTUTORIAL != null)
        {
            listaDeEnemigosTUTORIAL.ActivarEnemigos();
        }

        if (listaDeEnemigosLEVEL1 != null)
        {
            listaDeEnemigosLEVEL1.ActivarEnemigos();
        }

        if (listaDeEnemigosLEVEL2 != null)
        {
            listaDeEnemigosLEVEL2.ActivarEnemigos();
        }

        if (listaDeEnemigosLEVEL3 != null)
        {
            listaDeEnemigosLEVEL3.ActivarEnemigos();
        }
    }
}


