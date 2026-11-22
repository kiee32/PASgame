using UnityEngine;

// Data satu jenis item. Buat lewat: klik kanan di Project > Create > Inventory > Item Details
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item Details")]
public class ItemDetails : ScriptableObject
{
    [SerializeField] private string itemName = "Item";
    [SerializeField] private int quantity = 1;
    [SerializeField] private Sprite itemIcon;
    [SerializeField, TextArea] private string description;

    public string ItemName => itemName;
    public int Quantity => quantity;
    public Sprite ItemIcon => itemIcon;
    public string Description => description;
}