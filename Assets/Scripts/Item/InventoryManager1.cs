using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, GrabableItem
{
    InputAction inventoryAction;
    [SerializeField] private GameObject InventoryCanvas;
    [SerializeField] private int slotCount = 16;
    [SerializeField] private ItemDetailPanel detailsPanel;
    private bool InventoryActive;
    public ItemSlot[] itemSlot;

    void Start()
    {
        CreateSlots();
        if (InventoryCanvas != null)
        {
            InventoryCanvas.SetActive(false);
        }
        
        // Menangani input action dengan aman
        var actionAsset = InputSystem.actions;
        if (actionAsset != null)
        {
            inventoryAction = actionAsset.FindAction("Inventory");
        }
        
        InventoryActive = false;
    }

    void Update()
    {
        if (inventoryAction != null && inventoryAction.triggered && InventoryCanvas != null)
        {
            InventoryActive = !InventoryActive;

            if (InventoryActive)
            {
                Time.timeScale = 0f;
                Debug.Log("Inventory is active");
                InventoryCanvas.SetActive(true);
                ShowFirstItem();
            }
            else
            {
                Time.timeScale = 1f;
                Debug.Log("Inventory is deactivated");
                InventoryCanvas.SetActive(false);
            }
        }
    }

    // Build the slot objects at runtime so no manual wiring is needed in the scene
    private void CreateSlots()
    {
        ItemSlot slotPrefab = Resources.Load<ItemSlot>("ItemSlot");
        if (slotPrefab == null)
        {
            Debug.LogError("ItemSlot prefab was not found in a Resources folder.", this);
            itemSlot = new ItemSlot[0];
            return;
        }

        Transform parent = InventoryCanvas.transform;
        GridLayoutGroup grid = InventoryCanvas.GetComponentInChildren<GridLayoutGroup>(true);
        if (grid != null)
        {
            parent = grid.transform;
        }

        itemSlot = new ItemSlot[slotCount];
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i] = Instantiate(slotPrefab, parent);
            
            // PENTING: Mengambil komponen tombol pada slot yang baru di-instantiate 
            // agar otomatis terhubung ke fungsi SelectedSlot ketika diklik.
            ItemSlot currentSlot = itemSlot[i];
            Button slotButton = currentSlot.GetComponent<Button>();
            if (slotButton != null)
            {
                slotButton.onClick.AddListener(() => SelectedSlot(currentSlot));
            }
        }
    }

    public void AddItem(ItemDetails item)
    {
        Debug.Log("Item added to inventory: " + item.ItemName + " Quantity: " + item.Quantity);

        // Cek stack item yang sama terlebih dahulu
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull && itemSlot[i].CanStackWith(item.ItemName))
            {
                itemSlot[i].AddQuantity(item.Quantity);
                return;
            }
        }
        
        // Masukkan ke slot kosong berikutnya
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (!itemSlot[i].isFull)
            {
                itemSlot[i].AddItem(item);
                return;
            }
        }

        Debug.LogWarning("Inventory is full, '" + item.ItemName + "' was lost.");
    }
   
    public void SelectedSlot(ItemSlot slot)
    {
        // Jangan lakukan apa-apa jika slot kosong
        if (slot.IsEmpty) return;

        DesselectAllSlot();

        if (slot.selectedPanel != null)
        {
            slot.selectedPanel.SetActive(true);
        }
        slot.isItemSelected = true;

        if (detailsPanel != null)
        {
            detailsPanel.Show(slot.ItemDetails, slot.itemQuantity);
        }
    }

    private void ShowFirstItem()
    {
        if (itemSlot != null)
        {
            foreach (ItemSlot slot in itemSlot)
            {
                if (!slot.IsEmpty)
                {
                    SelectedSlot(slot);
                    return;
                }
            }
        }

        // No items yet: leave the panel empty
        if (detailsPanel != null)
        {
            detailsPanel.Clear();
        }
    }

    public void DesselectAllSlot()
    {
        foreach (ItemSlot slot in itemSlot)
        {
            slot.isItemSelected = false;
            if (slot.selectedPanel != null)
            {
                slot.selectedPanel.SetActive(false);
            }
        }
    }
}