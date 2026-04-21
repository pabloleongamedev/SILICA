using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private VitalityBarSegments vitalityBar;
    [SerializeField] private MissionTimer missionTimer;
    
    [Header("Textos de Estado")]
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text diagnosticText;

    void OnEnable()
    {
        if (missionTimer != null)
            missionTimer.OnTimeRatioChanged += OnTimeRatioChanged;
    }

    void OnDisable()
    {
        if (missionTimer != null)
            missionTimer.OnTimeRatioChanged -= OnTimeRatioChanged;
    }

    private void OnTimeRatioChanged(float ratio)
    {
        if (vitalityBar != null)
        {
            // Pasamos el ratio y el tiempo restante para que la barra decida su color
            vitalityBar.UpdateVisuals(ratio, missionTimer.CurrentTime);
        }

        ActualizarTextos(ratio);
    }

    private void ActualizarTextos(float ratio)
    {
        if (statusText == null || diagnosticText == null) return;

        if (ratio <= 0.25f)
        {
            statusText.text = "CRÍTICO";
            diagnosticText.text = "SISTEMA: FALLA INMINENTE";
            statusText.color = Color.red;
        }
        else if (ratio <= 0.6f)
        {
            statusText.text = "INESTABLE";
            diagnosticText.text = "ENERGÍA AL 50%";
            statusText.color = Color.yellow;
        }
        else
        {
            statusText.text = "NOMINAL";
            diagnosticText.text = "INTEGRIDAD ÓPTIMA";
            statusText.color = Color.cyan;
        }
    }
}