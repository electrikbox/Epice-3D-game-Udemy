using UnityEngine;
using UnityEngine.Audio;


public class VolumeSetAtLaunch : MonoBehaviour
{
    public AudioMixer myAudioMixer;


    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("volumeSliderValue"));
    }


    public void SetVolume(float sliderValue)
    {
        myAudioMixer.SetFloat("MasterVolume", (Mathf.Log10(sliderValue / 20) * 80));
    }
}
