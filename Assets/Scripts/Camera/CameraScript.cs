using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    private CinemachineVirtualCamera _cam1;
    private CinemachineVirtualCamera _cam2;

    public float shakeAmplitude = 1f;
    public float shakeDuration = 1f;

    private float _amp = 0f;
    private float _dur = 0f;

    void Awake()
    {
        _cam1 = transform.Find("Main").gameObject.GetComponent<CinemachineVirtualCamera>();
        _cam2 = transform.Find("Boss").gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (_dur > 0f)
        {
            _dur -= Time.deltaTime;
        }
        else if (_amp > 0f)
        {
            _amp -= 10f * Time.deltaTime;
        }


        if (_cam1 != null)
        {
            CinemachineBasicMultiChannelPerlin p = _cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (_amp > 0f)
            {
                p.m_AmplitudeGain = _amp;
            }
            else
            {
                p.m_AmplitudeGain = 0f;
            }
        }

        if (_cam2 != null)
        {
            CinemachineBasicMultiChannelPerlin p = _cam2.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (_amp > 0f)
            {
                p.m_AmplitudeGain = _amp;
            }
            else
            {
                p.m_AmplitudeGain = 0f;
            }
        }
    }

    public void Shake()
    {
        _amp = shakeAmplitude;
        _dur = shakeDuration;
    }

    public void Shake(float amplitude, float duration)
    {
        _amp = amplitude;
        _dur = duration;
    }
}
