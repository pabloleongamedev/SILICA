using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [Header("UI References")]
    public Slider masterVolumeSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Cargar valores guardados
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        ApplySound();
    }

    public void ApplySound()
    {
        // Volumen maestro
        AudioListener.volume = masterVolumeSlider.value;

        // Aquí puedes conectar con tus AudioMixers si los tienes
        // Ejemplo:
        // audioMixer.SetFloat("MusicVol", Mathf.Log10(musicSlider.value) * 20);
        // audioMixer.SetFloat("SFXVol", Mathf.Log10(sfxSlider.value) * 20);

        // Guardar
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }
}
