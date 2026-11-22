using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Panel detail item di UI inventory (ikon, nama, jumlah, deskripsi)
public class ItemDetailPanel : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text descriptionText;

    public void Show(ItemDetails item, int quantity)
    {
        if (item == null)
        {
            Clear();
            return;
        }

        if (iconImage != null)
        {
            iconImage.sprite = item.ItemIcon;
            iconImage.enabled = item.ItemIcon != null;
        }
        if (nameText != null) nameText.text = item.ItemName;
        if (quantityText != null) quantityText.text = "x" + quantity;
        if (descriptionText != null) descriptionText.text = item.Description;
    }

    public void Clear()
    {
        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
        if (nameText != null) nameText.text = "";
        if (quantityText != null) quantityText.text = "";
        if (descriptionText != null) descriptionText.text = "";
    }
}