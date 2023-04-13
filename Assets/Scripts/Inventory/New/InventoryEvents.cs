using System;
using UnityEngine;

public class InventoryEvents : MonoBehaviour
{
    // UI related events
    public static Action OnInventoryUpdate;
    public static Action<InventorySlotNew> OnShowItemDetails;
    public static Action OnInventoryShow;

    public static void InventoryUpdate()
    {
        OnInventoryUpdate?.Invoke();
    }

    public static void ShowItemDetails(InventorySlotNew slot)
    {
        OnShowItemDetails?.Invoke(slot);
    }

    public static void InventoryShow()
    {
        OnInventoryShow?.Invoke();
    }

// =================================================================================================
   
    // System related events
    public static Action OnMainWeaponEquipped;
    public static Action OnSecondaryWeaponEquipped;
}
