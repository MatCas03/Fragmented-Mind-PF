using UnityEngine;

public class SoundZone : MonoBehaviour
{
    [SerializeField] private float _newVoiceVolume = 1.0f;
    [SerializeField] private float _newStepsVolume = 0.5f;
    [SerializeField] private float _newDashVolume = 0.5f;

    private int _zonasBox = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                if (_zonasBox == 0)
                {                 
                    //player.VoiceSound.volume = _newVoiceVolume;
                    //player.StepsSound.volume = _newStepsVolume;
                    //player.DashSound.volume = _newDashVolume;
                }

                _zonasBox++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                _zonasBox--;

                //if (_zonasBox == 0)
                //{
                //    player.ResetAudioVolumes();
                //}
            }
        }
    }
}
