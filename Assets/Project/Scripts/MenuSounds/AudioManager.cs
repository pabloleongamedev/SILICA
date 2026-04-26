using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Singleton sencillo para acceder desde otros scripts
    public static AudioManager Instance { get; private set; }

    [Header("Mixer principal")]
    [SerializeField] private AudioMixer mainMixer;

    // Nombres EXACTOS de los parámetros expuestos en el AudioMixer
    const string MASTER_PARAM = "MasterVol";
    const string MUSIC_PARAM  = "MusicVol";
    const string SFX_PARAM    = "SFXVol";

    [Header("Volúmenes guardados (0–1)")]
    [Range(0.0001f, 1f)] public float masterVolume = 1f;
    [Range(0.0001f, 1f)] public float musicVolume  = 1f;
    [Range(0.0001f, 1f)] public float sfxVolume    = 1f;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Aplicar valores iniciales (por ejemplo, cargados de GameSettings o PlayerPrefs)
        ApplyMasterVolume(masterVolume);
        ApplyMusicVolume(musicVolume);
        ApplySFXVolume(sfxVolume);
    }


    public void SetMasterVolume(float value01)
    {
        masterVolume = Mathf.Clamp(value01, 0.0001f, 1f);
        ApplyMasterVolume(masterVolume);
    }

    public void SetMusicVolume(float value01)
    {
        musicVolume = Mathf.Clamp(value01, 0.0001f, 1f);
        ApplyMusicVolume(musicVolume);
    }

    public void SetSFXVolume(float value01)
    {
        sfxVolume = Mathf.Clamp(value01, 0.0001f, 1f);
        ApplySFXVolume(sfxVolume);
    }

    // ===== Implementación interna =====

    // Convierte un valor lineal 0–1 a decibelios para el AudioMixer
    private void ApplyMasterVolume(float value01)
    {
        float dB = Mathf.Log10(value01) * 20f; // 0.0001 → aprox. -80 dB, 1 → 0 dB[web:92][web:101][web:103]
        mainMixer.SetFloat(MASTER_PARAM, dB);
    }

    private void ApplyMusicVolume(float value01)
    {
        float dB = Mathf.Log10(value01) * 20f;
        mainMixer.SetFloat(MUSIC_PARAM, dB);
    }

    private void ApplySFXVolume(float value01)
    {
        float dB = Mathf.Log10(value01) * 20f;
        mainMixer.SetFloat(SFX_PARAM, dB);
    }
}