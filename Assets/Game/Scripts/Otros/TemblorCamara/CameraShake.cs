using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private GameObject playerCamPos;

    public IEnumerator Shaking()
    {
        Vector3 startPosition = playerCamPos.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float strength = curve.Evaluate(elapsedTime / duration);
            playerCamPos.transform.localPosition = startPosition + Random.insideUnitSphere * strength;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerCamPos.transform.localPosition = startPosition;
    }

}
