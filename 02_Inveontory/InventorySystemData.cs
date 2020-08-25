using SVS.InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventorySystemData
    {
        public Action updateHotbarCallback;
        private Storage storagePlayer, storageHotbar;
        List<int> inventoryUiElementIdList = new List<int>();
        List<int> hotbarUiElementIdList = new List<int>();

        public int selectedItemUIID = -1;

        public InventorySystemData(int playerStroageSize, int hotbarStorageSize)
        {
            storagePlayer = new Storage(playerStroageSize);
            storageHotbar = new Storage(hotbarStorageSize);
        }

        public int PlayerStorageLimit { get => storagePlayer.StorageLimit; }

        public void SetSelectedItemTo(int ui_id)
        {
            selectedItemUIID = ui_id;
        }

        public void ResetSelectedItem()
        {
            selectedItemUIID = -1;
        }

        public void AddHotbarUiElement(int ui_id)
        {
            hotbarUiElementIdList.Add(ui_id);
        }

        public void AddInventoryUiElement(int ui_id)
        {
            inventoryUiElementIdList.Add(ui_id);
        }

        public void ClearInventoryUIElements()
        {
            inventoryUiElementIdList.Clear();
        }

        public int AddToStorage(IInventoryItem item)
        {
            int countLeft = item.Count;
            if (storageHotbar.CheckIfStorageContains(item.ID))
            {
                countLeft = storageHotbar.AddItem(item);
                if (countLeft == 0)
                {
                    updateHotbarCallback.Invoke();
                    return countLeft;
                }
            }
            countLeft = storagePlayer.AddItem(item.ID, countLeft, item.IsStackable, item.StackLimit);
            if(countLeft > 0)
            {
                countLeft = storageHotbar.AddItem(item.ID, countLeft, item.IsStackable, item.StackLimit);
                if (countLeft == 0)
                {
                    updateHotbarCallback.Invoke();
                    return countLeft;
                }
            }
            return countLeft;
        }
    }
}

