using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private InteractionDetector detector;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI text;

    private void Update()
    {
        var interactable = detector.CurrentInteractable;

        if (interactable != null)
        {
            panel.SetActive(true);
            text.text = interactable.GetInteractionText();
        }
        else
        {
            panel.SetActive(false);
        }
    }
}