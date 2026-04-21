using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescriptionPanelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemDescription;

    void OnEnable()
    {
        Clear();
    }

    public void Show(InventorySlot slot)
    {
        if (slot == null || slot.IsEmpty)
        {
            Clear();
            return;
        }

        var data = slot.Item.Data;

        itemName.text = data.displayName;
        itemIcon.sprite = data.icon;
        itemIcon.enabled = true;
        itemDescription.text = data.description;
    }

    public void Clear()
    {
        itemName.text = "";
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        itemDescription.text = "";
    }
    
}