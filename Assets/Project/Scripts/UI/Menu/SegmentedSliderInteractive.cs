using UnityEngine;
using UnityEngine.UI;

public class SegmentedSliderInteractive : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Button[] segmentButtons;
    [SerializeField] private Image[] segmentImages;
    Color activeColor = new Color(0,255,255,255); // Azul claro
    private Color inactiveColor = new Color(190,255,255,255);  // Azul oscuro

    private const string PREF_KEY = "SegmentSelected";

    void Start()
    {
        // Asignar eventos de clic a cada botón-segmento
        for (int i = 0; i < segmentButtons.Length; i++)
        {
            int index = i; // Captura local para evitar problemas de referencia
            segmentButtons[i].onClick.AddListener(() => OnSegmentClicked(index));
        }

        slider.onValueChanged.AddListener(UpdateSegments);

        // Recuperar selección guardada
        int savedIndex = PlayerPrefs.GetInt(PREF_KEY, -1);
        if (savedIndex >= 0 && savedIndex < segmentButtons.Length)
        {
            OnSegmentClicked(savedIndex);
        }
        else
        {
            UpdateSegments(slider.value);
        }
    }

    void OnSegmentClicked(int index)
    {
        // Normaliza el valor del slider según el bloque seleccionado
        float normalizedValue = (index + 1) / (float)segmentButtons.Length;
        slider.value = normalizedValue;

        // Guardar selección
        PlayerPrefs.SetInt(PREF_KEY, index);
        PlayerPrefs.Save();
    }

    void UpdateSegments(float value)
    {
        int activeCount = Mathf.RoundToInt(value * segmentImages.Length);
        for (int i = 0; i < segmentImages.Length; i++)
        {
            segmentImages[i].color = i < activeCount ? activeColor : inactiveColor;
        }
    }
}
