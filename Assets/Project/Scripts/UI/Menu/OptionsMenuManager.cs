using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    [Header("Panel de Opciones")]
    [SerializeField] private GameObject optionsPanel;

    [Header("Botones")]
    [SerializeField] private Button applyButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button closeButton;

    [Header("Sliders")]
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Slider masterVolumeSlider;

    void Start()
    {
        // Inicializar sliders con valores guardados
        brightnessSlider.value = GameSettings.Instance.Brightness;
        musicSlider.value = GameSettings.Instance.MusicVolume;
        effectsSlider.value = GameSettings.Instance.EffectsVolume;
        masterVolumeSlider.value = GameSettings.Instance.MasterVolume;

        // Conectar botones
        applyButton.onClick.AddListener(ApplySettings);
        resetButton.onClick.AddListener(ResetSettings);
        closeButton.onClick.AddListener(CloseOptionsPanel);
    }

    void ApplySettings()
    {
        GameSettings.Instance.Brightness = brightnessSlider.value;
        GameSettings.Instance.MusicVolume = musicSlider.value;
        GameSettings.Instance.EffectsVolume = effectsSlider.value;
        GameSettings.Instance.MasterVolume = masterVolumeSlider.value;
        GameSettings.Instance.Save();
    }

    void ResetSettings()
    {
        GameSettings.Instance.ResetToDefaults();

        brightnessSlider.value = GameSettings.Instance.Brightness;
        musicSlider.value = GameSettings.Instance.MusicVolume;
        effectsSlider.value = GameSettings.Instance.EffectsVolume;
        masterVolumeSlider.value = GameSettings.Instance.MasterVolume;
    }

    void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }
}
