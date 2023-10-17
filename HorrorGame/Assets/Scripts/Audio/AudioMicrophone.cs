using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum _channel { Stereo, Left, Right, }
[RequireComponent(typeof(AudioSource))]
public class AudioMicrophone : MonoBehaviour
{
    private AudioSource m_AudioSource;
    [Header("Audio mixers")]
    public AudioMixerGroup _mixerGroupMaster;
    public AudioMixerGroup _mixerGroupMicrophone;

    [Header("Microphone")]
    public AudioClip m_AudioClip;
    public bool _useMicrophone;
    public string _selectedDevice;

    //stereo channels
    [Header("Channels")]
    public _channel channel = new _channel();

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();

        if(_useMicrophone)
        {
            if(Microphone.devices.Length > 0)
            {
                _selectedDevice = Microphone.devices[0].ToString();
                m_AudioSource.outputAudioMixerGroup = _mixerGroupMicrophone;
                m_AudioSource.clip = Microphone.Start(_selectedDevice, true, 10, AudioSettings.outputSampleRate);
            }
            else
                _useMicrophone = false;
        }
        else
        {
            m_AudioSource.outputAudioMixerGroup = _mixerGroupMaster;
            m_AudioSource.clip = m_AudioClip;
        }

        m_AudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
