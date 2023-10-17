using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneCapture : MonoBehaviour
{
    AudioSource audioSource;
    UiManager uiManager;

    // Default Mic
    public string defaultDevice;
    public List<string> selectedDevices;
    public string lastDevice;

    // Block for audioSource.GetOutputData()
    public static float[] samples = new float[128];


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        uiManager = FindObjectOfType<UiManager>();
        uiManager.microphoneDropdown.ClearOptions();
        /**
         * if there are microphones,
         * select the default mic,
         * set the audioSource.clip to the default mic, looped, for 1 second, at the sampleRate
         * set loop to true
         */
        if (Microphone.devices.Length > 0)
        {
            for (int i = 0; i < Microphone.devices.Length; i++) 
            {
                selectedDevices.Add(Microphone.devices[i].ToString());
            }
            uiManager.microphoneDropdown.AddOptions(selectedDevices);
            uiManager.fillImage.color = uiManager.colors[0];

            defaultDevice = Microphone.devices[0].ToString();
            audioSource.clip = Microphone.Start(selectedDevices[0], true, 1, AudioSettings.outputSampleRate);
            lastDevice = selectedDevices[0];
            Debug.Log(uiManager.microphoneDropdown.value.ToString() + " / " + uiManager.microphoneDropdown.captionText.text);
            audioSource.loop = true;

            /**
             * While the position of the mic in the recording is greater than 0,
             * play the clip (that should be the mic)
             */
            while (!(Microphone.GetPosition(defaultDevice) > 0))
            {
                audioSource.Play();
            }
        }
    }


    void Update()
    {
        getOutputData();
        ChangeColorSlider();
    }

    /**
     * Load the block samples with data from the audioSource output
     * Average the values across the size of the block.
     * vals is the volume of the mic, used to control block height
     * Block height represents candle flame getting larger
     */
    void getOutputData()
    {
        audioSource.GetOutputData(samples, 0);
        float vals = 0.0f;

        for (int i = 0; i < 128; i++)
        {
            vals += Mathf.Abs(samples[i]);
        }
        //vals /= 128.0f;
        uiManager.microphoneSliderDetect.value = vals * 100f;
        //Debug.Log(vals);
        //gameObject.transform.localScale = new Vector3(1.0f, 1.0f + (vals * 10.0f), 1.0f);
    }

    private void ChangeColorSlider()
    {
        if(uiManager.microphoneSliderDetect.value >= 50)
            uiManager.fillImage.color = Color.Lerp(uiManager.colors[1], uiManager.colors[0], Time.deltaTime);

        if (uiManager.microphoneSliderDetect.value < 50)
            uiManager.fillImage.color = Color.Lerp(uiManager.colors[0], uiManager.colors[1], Time.deltaTime);
    }

    public void OnDropDownChange()
    {
        StopCaptureVoice();

        if(audioSource.clip != null)
            audioSource.clip.UnloadAudioData();
        
        audioSource.clip = Microphone.Start(selectedDevices[uiManager.microphoneDropdown.value], true, 1, AudioSettings.outputSampleRate);
        Debug.Log(uiManager.microphoneDropdown.value.ToString() + " / " + uiManager.microphoneDropdown.captionText.text);
        audioSource.loop = true;

        while(Microphone.GetPosition(null)< 0) 
        {
        }

        audioSource.Play();
    }

    public void StopCaptureVoice()
    {
        lastDevice = selectedDevices[uiManager.microphoneDropdown.value];

        if (Microphone.IsRecording(null) == false)
            return;

        Microphone.End(null);
        audioSource.Stop();

    }
}