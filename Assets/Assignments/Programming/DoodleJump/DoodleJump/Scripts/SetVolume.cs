using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// All just using the same NameSpace this project.
namespace doodleJump
{
    // To be added to the settings folder in the "Music Manager".
    public class SetVolume : MonoBehaviour
    {
        // Gets the audio mixer to edit.
        public AudioMixer mixer;        // <--- !!! TO BE SET IN INSPECTOR!!!
        // Gets the slider to change the volume.
        public Slider slider;           // <--- !!! TO BE SET IN INSPECTOR!!!

        // Called on Start before any updates.
        void Start()
        {
            // Sets the slider to the player prefs "Music volume" saved.
            slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        }

        // Set in inspector by the Audio Volume Slider. The slider in inspector should be ("Min Value = 0.0001", "Max Value = 1", )
        public void SetLevel(float sliderValue)
        {
            // On a slider change, change the audio = to the new value.
            mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);  // Log 10 means the audio will change at an even pace.
            // Save the new "Music Volume" in player prefs to the new slider number.
            PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        }
    }
}