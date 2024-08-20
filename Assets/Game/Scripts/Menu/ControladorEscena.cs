using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class ControladorEscena : MonoBehaviour
{
    [SerializeField] private Slider BarraDeCarga;
    [SerializeField] private GameObject PanelDeCarga;

    [SerializeField] private AudioClip sonidoPatada;
    [SerializeField] private AudioClip sonidoBoton;
    [SerializeField] private AudioClip sonidoBotonSalir;
    [SerializeField] private AudioClip musicMenu;

    private void Awake()
    {
        SoundFXManager.Instance.PlaySoundFXClip(musicMenu, this.transform, 0.35f);
    }

    public void SceneLoad(int sceneIndex)
    {
        SoundFXManager.Instance.PlaySoundFXClip(sonidoPatada, this.transform, .25f);
        PanelDeCarga.SetActive(true);
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!asyncOperation.isDone)
        {
            print(asyncOperation.progress);
            BarraDeCarga.value = asyncOperation.progress / 0.9f;
            yield return null;
            
        }
    }

    public void ClickSound()
    {
        SoundFXManager.Instance.PlaySoundFXClip(sonidoBoton, this.transform, .25f);
    }

    public void SalirSound()
    {
        SoundFXManager.Instance.PlaySoundFXClip(sonidoBotonSalir, this.transform, .25f);
    }
}
