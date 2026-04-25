using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance { get; private set; }

    [Header("Referencia al AudioSource (SFX/UI)")]
    [SerializeField] private AudioSource uiAudioSource;

    [Header("Clips de UI")]
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip applyClip;
    [SerializeField] private AudioClip resetClip;
    [SerializeField] private AudioClip sliderTickClip;
    [SerializeField] private AudioClip sliderLimitClip;
    [SerializeField] private AudioClip errorClip;

    private void Awake()
    {
        // Singleton básico para que solo exista un UIAudioManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Si no se asignó desde el Inspector, intenta encontrar el AudioSource en el mismo GameObject
        if (uiAudioSource == null)
            uiAudioSource = GetComponent<AudioSource>();
    }

    // ===== Métodos públicos para enlazar desde botones y sliders =====

    public void PlayHover()
    {
        PlayOneShot(hoverClip);
    }

    public void PlayClick()
    {
        PlayOneShot(clickClip);
    }

    public void PlayApply()
    {
        PlayOneShot(applyClip != null ? applyClip : clickClip);
    }

    public void PlayReset()
    {
        PlayOneShot(resetClip != null ? resetClip : clickClip);
    }

    public void PlaySliderTick()
    {
        PlayOneShot(sliderTickClip);
    }

    public void PlaySliderLimit()
    {
        PlayOneShot(sliderLimitClip != null ? sliderLimitClip : sliderTickClip);
    }

    public void PlayError()
    {
        PlayOneShot(errorClip);
    }

    // ===== Implementación interna =====

    private void PlayOneShot(AudioClip clip)
    {
        if (uiAudioSource == null || clip == null)
            return;

        uiAudioSource.PlayOneShot(clip);
    }
}