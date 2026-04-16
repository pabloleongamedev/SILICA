using UnityEngine;
using UnityEngine.UI;

public class GraphicsTest : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Image brightnessOverlay; 
    // Este overlay es un Image negro semi-transparente que cubre la pantalla

    void Start()
    {
        // Inicializar con valores guardados
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 0.75f);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        ApplyBrightness(brightnessSlider.value);
        ApplyFullscreen(fullscreenToggle.isOn);

        // Conectar eventos
        brightnessSlider.onValueChanged.AddListener(ApplyBrightness);
        fullscreenToggle.onValueChanged.AddListener(ApplyFullscreen);
    }

    void ApplyBrightness(float value)
    {
        // Ajusta el alpha del overlay inversamente al brillo
        if (brightnessOverlay != null)
        {
            Color c = brightnessOverlay.color;
            c.a = 1f - value; // más brillo = menos opacidad
            brightnessOverlay.color = c;
        }

        PlayerPrefs.SetFloat("Brightness", value);
        Debug.Log("✅ Brillo aplicado: " + value);
    }

    void ApplyFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        Debug.Log("✅ Pantalla completa: " + isFullscreen);
    }
}
