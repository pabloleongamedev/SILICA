using UnityEngine;

public class GameSettings
{
    private static GameSettings instance;
    public static GameSettings Instance {
        get {
            if (instance == null) instance = new GameSettings();
            return instance;
        }
    }

    // Valores actuales
    public float Brightness { get; set; }
    public float MusicVolume { get; set; }
    public float EffectsVolume { get; set; }
    public float MasterVolume { get; set; }

    // Valores por defecto
    public const float DefaultBrightness = 0.75f;
    public const float DefaultMusic = 0.5f;
    public const float DefaultEffects = 0.5f;
    public const float DefaultMaster = 1f;

    private GameSettings() {
        Load();
    }

    public void Save() {
        PlayerPrefs.SetFloat("Brightness", Brightness);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("EffectsVolume", EffectsVolume);
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.Save();
    }

    public void Load() {
        Brightness = PlayerPrefs.GetFloat("Brightness", DefaultBrightness);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", DefaultMusic);
        EffectsVolume = PlayerPrefs.GetFloat("EffectsVolume", DefaultEffects);
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", DefaultMaster);
    }

    public void ResetToDefaults() {
        Brightness = DefaultBrightness;
        MusicVolume = DefaultMusic;
        EffectsVolume = DefaultEffects;
        MasterVolume = DefaultMaster;
    }
}
