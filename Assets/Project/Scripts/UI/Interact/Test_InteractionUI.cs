using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
     /////////////// ESTO ES BASURA, SOLO PARA TESTEAR, NO SE DEBE USAR ASÍ EN EL JUEGO FINAL!!!! ELIMINAR ///////////////
    [SerializeField] private InteractionDetector detector;
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI text;

    private void Update()
    {
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
    }
}