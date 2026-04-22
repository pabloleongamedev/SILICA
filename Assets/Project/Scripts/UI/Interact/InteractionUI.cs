<<<<<<< HEAD
using TMPro;
using UnityEngine;
=======
using UnityEngine;
using TMPro;
>>>>>>> 7ca46c4 (restore scripts interaction system)

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private InteractionDetector detector;
<<<<<<< HEAD
    [SerializeField] private GameObject panel;
=======
    [SerializeField] private GameObject container;
>>>>>>> 7ca46c4 (restore scripts interaction system)
    [SerializeField] private TextMeshProUGUI text;

    private void Update()
    {
<<<<<<< HEAD
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
=======
        if (detector == null)
        {
            container.SetActive(false);
            return;
        }

        var interactable = detector.CurrentInteractable;

        // 🔥 FIX CLAVE: validar null real (Unity destroyed object)
        if (interactable == null)
        {
            container.SetActive(false);
            return;
        }

        string message = interactable.GetInteractionText();

        if (string.IsNullOrEmpty(message))
        {
            container.SetActive(false);
            return;
        }

        container.SetActive(true);
        text.text = message;
>>>>>>> 7ca46c4 (restore scripts interaction system)
    }
}