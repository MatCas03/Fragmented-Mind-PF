using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class Safe : MonoBehaviour
{
    public string correctPassword = "1234";
    private string currentPassword = "";

    public TextMeshPro display;  // Cambiado a TextMeshProUGUI para usar solo uno

    public GameObject targetObject;      // GameObject al que se le asignará la nueva capa
    public string layerName = "Unlocked"; // Nombre de la capa a agregar

    [SerializeField] private AudioClip sonidoCorrect;
    [SerializeField] private AudioClip sonidoError;

    public void AddDigit(string digit)
    {
        if (currentPassword.Length < 4) // Cambiado a 5 para que coincida con la longitud de correctPassword
        {
            currentPassword += digit;
            UpdateDisplay("");
        }
    }

    public void DeleteLastDigit()
    {
        if (currentPassword.Length > 0)
        {
            currentPassword = currentPassword.Substring(0, currentPassword.Length - 1);
            UpdateDisplay("");
        }
    }

    public void CheckPassword()
    {
        if (currentPassword == correctPassword)
        {
            Debug.Log("Password Correct!");
            UpdateDisplay("Password Correct!");
            SoundFXManager.Instance.PlaySoundFXClip(sonidoCorrect, this.transform, .75f);
            AddLayerToGameObject(targetObject, layerName);
        }
        else
        {
            SoundFXManager.Instance.PlaySoundFXClip(sonidoError, this.transform, .75f);
            Debug.Log("Password Incorrect!");
            UpdateDisplay("Password Incorrect!");
        }
        currentPassword = ""; // Reset after checking
    }

    private void UpdateDisplay(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            display.text = currentPassword;
        }
        else
        {
            display.text = message;
        }
    }

    private void AddLayerToGameObject(GameObject obj, string layerName)
    {
        // Asignar la nueva capa al GameObject
        obj.layer = LayerMask.NameToLayer(layerName);
    }

    
}






