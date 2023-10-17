using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    public GameObject audioBar; // Prefab de la barra de audio
    public int barCount = 64; // Cantidad de barras
    public float spacing = 0.1f; // Espacio entre las barras
    public float minHeight = 0.1f; // Altura mínima de la barra
    public float maxHeight = 3.0f; // Altura máxima de la barra
    public float sensitivity = 100.0f; // Sensibilidad de detección de audio

    private GameObject[] audioBars;
    private float[] audioLevels;

    [Header("Microphone")]
    public AudioClip m_AudioClip;
    public bool _useMicrophone;
    public string _selectedDevice;
    private AudioSource m_AudioSource;
    [Header("Audio mixers")]
    public AudioMixerGroup _mixerGroupMaster;
    public AudioMixerGroup _mixerGroupMicrophone;

    void Start()
    {
        audioBars = new GameObject[barCount];
        audioLevels = new float[barCount];

        for (int i = 0; i < barCount; i++)
        {
            float xPos = i * spacing;
            audioBars[i] = Instantiate(audioBar, new Vector3(xPos, 0, 0), Quaternion.identity);
        }
    }

    void Update()
    {
        float[] spectrumData = new float[256];
        AudioListener.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);

        for (int i = 0; i < barCount; i++)
        {
            float audioLevel = Mathf.Clamp01(spectrumData[i] * sensitivity);
            //audioLevels[i] = Mathf.Lerp(audioLevels[i], audioLevel, Time.deltaTime * 5);

            Vector3 scale = audioBars[i].transform.localScale;
            scale.y = Mathf.Lerp(minHeight, maxHeight, audioLevel);
            audioBars[i].transform.localScale = scale;
        }
    }

    /*  float AudioInputLevel()
    {
        float level = 0;
        foreach (var device in Microphone.devices)
        {
            var audioData = new float[256];
            Microphone.
            foreach (var sample in audioData)
            {
                level += Mathf.Abs(sample);
            }
        }
        return level / 256;
    }*/
}
