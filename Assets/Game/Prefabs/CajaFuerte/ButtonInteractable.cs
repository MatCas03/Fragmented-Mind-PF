using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractable : MonoBehaviour
{
    public Safe safe;
    public string buttonValue;

    [SerializeField] private AudioClip sonidoTecla;

    public void OnInteract()
    {
        if (buttonValue == "Clear")
        {
            SoundFXManager.Instance.PlaySoundFXClip(sonidoTecla, safe.transform, .75f);
            safe.DeleteLastDigit();
        }
        else if (buttonValue == "Enter")
        {
            safe.CheckPassword();
        }
        else
        {
            SoundFXManager.Instance.PlaySoundFXClip(sonidoTecla, safe.transform, .75f);
            safe.AddDigit(buttonValue);
        }
    }
}


