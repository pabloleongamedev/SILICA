using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    [Header("UI References")]
    public Toggle fullscreenToggle;
    public Slider brightnessSlider;

    void Start()
    {
        // Cargar valores guardados
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 0.8f);

        ApplyGraphics();
    }

    public void ApplyGraphics()
    {
        // Pantalla completa
        Screen.fullScreen = fullscreenToggle.isOn;

        // Brillo (ajustando luz ambiental)
        RenderSettings.ambientLight = Color.white * brightnessSlider.value;

        // Guardar
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        PlayerPrefs.Save();
    }
}
