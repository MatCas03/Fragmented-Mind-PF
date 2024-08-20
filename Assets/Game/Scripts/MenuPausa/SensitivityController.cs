using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public JugadorCam jugadorCam;

    void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);

        // Inicializar el valor del slider con el valor actual de sensibilidad
        sensitivitySlider.value = jugadorCam.sensitivity;
    }

    void SetSensitivity(float value)
    {
        jugadorCam.sensitivity = value;
    }
}



