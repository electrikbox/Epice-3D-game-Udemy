using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Quality : MonoBehaviour
{
    public Text qualityText;
    public Slider quality;
    public AudioClip lightClick;
    AudioSource aSource;

    public PostProcessVolume ppv;
    DepthOfField depthOfField;
    Bloom bloom;
    



    private void Start()
    {
        ppv.sharedProfile.TryGetSettings<DepthOfField>(out depthOfField);
        ppv.sharedProfile.TryGetSettings<Bloom>(out bloom);

        aSource = GetComponent<AudioSource>();

        if(PlayerPrefs.HasKey("quality"))
        {
            quality.value = PlayerPrefs.GetFloat("quality");
            QualityUpdate(PlayerPrefs.GetFloat("quality"));
        }
        else
        {
            qualityText.text = "Hight";
            quality.value = 2;
        }
    }


    public void QualityUpdate(float value)
    {
        switch(value)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                depthOfField.active = false;
                bloom.active = false;
                qualityText.text = "Low";
                break;

            case 1:
            QualitySettings.SetQualityLevel(1);
                depthOfField.active = false;
                bloom.active = true;
                qualityText.text = "Mid";
                break;
            
            case 2:
                QualitySettings.SetQualityLevel(2);
                depthOfField.active = true;
                bloom.active = true;
                qualityText.text = "High";
                break;
        }

        PlayerPrefs.SetFloat("quality", value);
    }

    public void LightClick(float sliderValue) => aSource.PlayOneShot(lightClick, sliderValue / 20);
}

