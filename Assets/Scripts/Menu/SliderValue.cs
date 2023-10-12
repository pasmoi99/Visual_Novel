using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textValue;
    public Slider slider;
    public AudioSource music;

    public void OnValueChanged(float newValue)
    {
        int valueInt = (int)Mathf.Round(newValue * 100.0f);
        textValue.text = valueInt.ToString();
    }

    public void OnMusicValueChanged(float newValue)
    {
        music.volume = newValue;
    }
    
       
    
}
