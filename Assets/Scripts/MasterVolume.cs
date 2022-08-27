using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    public AudioMixer myAudioMixer;
    public Text volumeText;
    public Slider volume;
    public AudioClip lightClick;
    AudioSource aSource;


    private void Start()
    {
        aSource = GetComponent<AudioSource>();

        if(PlayerPrefs.HasKey("sliderValueVolume"))
        {
            volume.value = PlayerPrefs.GetFloat("sliderValueVolume");
            SetVolume(PlayerPrefs.GetFloat("volumeSliderValue"));
            VolumeTextUpdate(PlayerPrefs.GetFloat("volumeText"));
        }
        else
        {
            volume.value = 15;
            volumeText.text = "70 %";
            SetVolume(15);
        }
    }


    public void SetVolume(float sliderValue)
    {
        myAudioMixer.SetFloat("MasterVolume", (Mathf.Log10(sliderValue / 20) * 80));

        PlayerPrefs.SetFloat("sliderValueVolume", sliderValue);
        PlayerPrefs.SetFloat("volumeSliderValue", sliderValue);
    }

    public void VolumeTextUpdate(float value)
    {
        volumeText.text = Mathf.RoundToInt((value * 10 - 10) / 2) + "%";
        PlayerPrefs.SetFloat("volumeText", value);
    }

    public void LightClick(float sliderValue) => aSource.PlayOneShot(lightClick, sliderValue / 20);
}
