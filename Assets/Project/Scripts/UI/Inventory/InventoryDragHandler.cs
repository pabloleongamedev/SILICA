using UnityEngine;
using UnityEngine.UI;

public class InventoryDragHandler : MonoBehaviour
{
    [SerializeField] private Image ghostIcon;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        ghostIcon.gameObject.SetActive(false);
    }

    public void StartDrag(Sprite icon)
    {
        Debug.Log("START DRAG"); // 👈 agrega esto
        ghostIcon.transform.SetAsLastSibling();
        ghostIcon.sprite = icon;
        Debug.Log(icon.name);
        ghostIcon.gameObject.SetActive(true);
    }

        public void UpdateDrag(Vector2 position)
        {
            ghostIcon.rectTransform.position = position;
        }

    public void EndDrag()
    {
        ghostIcon.gameObject.SetActive(false);
    }
<<<<<<< HEAD
=======
    
>>>>>>> 7ca46c4 (restore scripts interaction system)
}