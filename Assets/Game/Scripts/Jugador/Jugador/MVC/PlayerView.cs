using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    public Player _p;
    private bool PerdisteVariable = false;

    [Header("Sonido")]
    private AudioClip[] _stepsSounds;
    private AudioClip _dashSound;
    private AudioClip _voiceSound;

    private TextMeshPro _lifeText;
    private ParticleSystem _jumpParticles;
    public JugadorCam _jc;
    public RaycastCam _rc;
    private GameObject _looseScreen;
    private Image _bloodCam;

    public PlayerView(Player user)
    {
        _p = user;
        _stepsSounds = user.StepsSounds;  
        _dashSound = user.DashSound;    
        _lifeText = user.LifeText;
        _jc = user.GetComponentInChildren<JugadorCam>();
        _rc = user.GetComponentInChildren<RaycastCam>();
        _looseScreen = user.LoosePannel;
        _bloodCam = user.BloodCam;
    }


    public void LifeView()
    {
        if (_lifeText != null)
        {
            _lifeText.text = " " + _p.model._currentLife.ToString();
        }

        float alpha = 1.0f - ((float)_p.model._currentLife / _p.StartLife);
        Color newColor = _bloodCam.color;
        newColor.a = alpha;
        _bloodCam.color = newColor;
    }

    public void MovementSound()
    {
        SoundFXManager.Instance.PlayRandomSoundFXClip(_stepsSounds, _p.transform, 1f);
    }

    public void DashSound() 
    {
        SoundFXManager.Instance.PlaySoundFXClip(_dashSound, _p.transform, 1f);
    }

    public void PlayerDead()
    {
        MostrarPanelGameOver();
        //SoundFXManager.Instance.PlaySoundFXClip(_playerDeadSound, _p.transform, 1f);
    }

    public void VoiceSound()
    {
        //_voiceSound.Play();
    }

    private void MostrarPanelGameOver()
    {
        _p.enabled = false;
        _jc.enabled = false;
        _rc.enabled = false;
        _looseScreen.SetActive(true);
        PerdisteVariable = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
