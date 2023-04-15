using System;
using UnityEngine;

public class InventoryEvents
{
    // UI related events
    public static Action OnInventoryUpdate;
    public static Action<InventorySlot> OnShowItemDetails;
    public static Action OnInventoryShow;

    public static void InventoryUpdate()
    {
        OnInventoryUpdate?.Invoke();
    }

    public static void ShowItemDetails(InventorySlot slot)
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
    public static Action OnMainWeaponUnequipped;
    public static Action OnSecondaryWeaponEquipped;
    public static Action OnSecondaryWeaponUnequipped;
    public static Action OnMovementItemEquipped;
    public static Action OnMovementItemUnequipped;
}
