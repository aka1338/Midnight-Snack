using Cinemachine;
using System.Collections;
using UnityEngine;

public class ShakeyCam : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private float shakeDuration = 0.3f;
    private float shakeAmplitude = 0.2f;
    private float shakeFrequency = 2.0f;

    private void Start()
    {
        virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
        ShakeCamera(); 
    }

    public void ShakeCamera()
    {
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = shakeAmplitude;
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = shakeFrequency;

        StartCoroutine(StopShaking());
    }

    IEnumerator StopShaking()
    {
        yield return new WaitForSeconds(shakeDuration);
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
    }
}