using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject ObjetoMenuPausa;
    public bool Pausa = false;
    [SerializeField] private GameObject MenuSalir;
    [SerializeField] private GameObject VolverMenu;
    [SerializeField] private GameObject OpcionesConfig;
    [SerializeField] private GameObject botonesMain;
    [SerializeField] private GameObject HUDPlayer;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            botonesMain.SetActive(true);
            HUDPlayer.SetActive(false);

            if (Pausa == false)
            {
                ObjetoMenuPausa.SetActive(true);
                Pausa = true;

                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                AudioSource[] sonidos = FindObjectsOfType<AudioSource>();

                for (int i = 0; i < sonidos.Length; i++)
                {
                    sonidos[i].Pause();
                }
            }
            else if (Pausa == true)
            {
                Resumir();
            }
        }
    }

    public void Resumir()
    {
        ObjetoMenuPausa.SetActive(false);
        MenuSalir.SetActive(false);
        VolverMenu.SetActive(false);
        OpcionesConfig.SetActive(false);
        HUDPlayer.SetActive(true);

        Pausa = false;

        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        AudioSource[] sonidos = FindObjectsOfType<AudioSource>();

    } 

    public void IrAlMenu(string NombreMenu)
    {
        SceneManager.LoadScene(NombreMenu);
    }

    public void SalirDeljuego()
    {
       Application.Quit();
    }
}
