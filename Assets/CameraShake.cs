using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    public static UnityEvent<float,float> OnCameraShake;
    public CinemachineVirtualCamera virutalCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private float duration;
    void Start()
    {
        OnCameraShake = new UnityEvent<float,float>();
        cinemachineBasicMultiChannelPerlin = virutalCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        OnCameraShake.AddListener(StartCameraShake);
        duration = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (duration > 0)
        {
            CamShake();
            duration -= Time.deltaTime;
        }
    }

    private void CamShake()
    {
    }

    public void StartCameraShake(float duration,float ampl)
    {
        StartCoroutine(StartShakeCoroutine(duration, ampl));
    }
    IEnumerator StartShakeCoroutine(float duration,float amplitude)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
        yield return new WaitForSeconds(duration);
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;


    }

}
