using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IGrabable
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

        var actionAsset = InputSystem.actions;
        if (actionAsset != null)
        {
            inventoryAction = actionAsset.FindAction("Inventory");
            if (inventoryAction == null)
                Debug.LogWarning("Action 'Inventory' tidak ditemukan di InputSystem_Actions, inventory tidak bisa dibuka.", this);
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
                InventoryCanvas.SetActive(true);
                ShowFirstItem();
            }
            else
            {
                Time.timeScale = 1f;
                InventoryCanvas.SetActive(false);
            }
        }
    }

    // Slot dibuat saat runtime dari prefab Resources/ItemSlot
    private void CreateSlots()
    {
        if (InventoryCanvas == null)
        {
            Debug.LogError("InventoryCanvas belum diisi di Inspector.", this);
            itemSlot = new ItemSlot[0];
            return;
        }

        ItemSlot slotPrefab = Resources.Load<ItemSlot>("ItemSlot");
        if (slotPrefab == null)
        {
            Debug.LogError("Prefab 'ItemSlot' tidak ditemukan di folder Resources.", this);
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
            itemSlot[i].Setup(this);
        }
    }

    // Implementasi IGrabable
    public void AddItem(ItemDetails item)
    {
        if (item == null || itemSlot == null) return;

        Debug.Log("Item added to inventory: " + item.ItemName + " Quantity: " + item.Quantity);

        // Tumpuk ke slot yang isinya sama dulu
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].CanStackWith(item.ItemName))
            {
                itemSlot[i].AddItem(item);
                return;
            }
        }

        // Kalau tidak ada, pakai slot kosong pertama
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].IsEmpty)
            {
                itemSlot[i].AddItem(item);
                return;
            }
        }

        Debug.LogWarning("Inventory penuh, '" + item.ItemName + "' tidak masuk.");
    }

    public void SelectedSlot(ItemSlot slot)
    {
        if (slot == null || slot.IsEmpty) return;

        DesselectAllSlot();
        slot.SetSelected(true);

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

        DesselectAllSlot();
        if (detailsPanel != null)
        {
            detailsPanel.Clear();
        }
    }

    public void DesselectAllSlot()
    {
        if (itemSlot == null) return;

        foreach (ItemSlot slot in itemSlot)
        {
            slot.SetSelected(false);
        }
    }
}