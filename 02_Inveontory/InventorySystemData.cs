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

        private int equippedItemStorageIndex = -1;
        private Storage equippedItemStorage = null;
        public bool ItemEquipped { get => equippedItemStorageIndex != -1; }
        public int EquippedUI_ID { 
            get 
            { 
                if(equippedItemStorage == null)
                {
                    return -1;
                }
                if(equippedItemStorage == storageHotbar)
                {
                    return hotbarUiElementIdList[equippedItemStorageIndex];
                }
                else
                {
                    return inventoryUiElementIdList[equippedItemStorageIndex];
                }
            } 
        }

        public string EquippedItemId { get => equippedItemStorage.GetItemData(equippedItemStorageIndex).ID; }

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
                updateHotbarCallback.Invoke();
                if (countLeft == 0)
                {
                    return countLeft;
                }
            }
            countLeft = storagePlayer.AddItem(item.ID, countLeft, item.IsStackable, item.StackLimit);
            if(countLeft > 0)
            {
                countLeft = storageHotbar.AddItem(item.ID, countLeft, item.IsStackable, item.StackLimit);
                updateHotbarCallback.Invoke();
                if (countLeft == 0)
                {
                    return countLeft;
                }
            }
            return countLeft;
        }

        internal int TakeFromItem(string ID, int count)
        {
            int tempCount = 0;
            tempCount += TakeFromStorage(storageHotbar, ID, count);
            if (tempCount == count)
            {
                return count;
            }
            else
            {
                tempCount += TakeFromStorage(storagePlayer, ID, count);
            }
            return tempCount;
        }

        private int TakeFromStorage(Storage storage, string iD, int count)
        {
            var tempQuantity = storage.GetItemCount(iD);
            if(tempQuantity > 0)
            {
                if(tempQuantity >= count)
                {
                    storage.TakeItemFromStorageIfContaintEnough(iD, count);
                    return count;
                }
                else
                {
                    storage.TakeItemFromStorageIfContaintEnough(iD, tempQuantity);
                    return tempQuantity;
                }
            }
            return 0;
        }

        internal bool CheckItemInStorage(string id, int count)
        {
            int quantity = storagePlayer.GetItemCount(id);
            quantity += storageHotbar.GetItemCount(id);
            if(quantity >= count)
            {
                return true;
            }
            return false;
        }

        internal bool CheckIfStorageIsFull()
        {
            return storageHotbar.CheckIfStorageIsFull() && storagePlayer.CheckIfStorageIsFull();
        }

        internal bool CheckIfSelectedItemIsEmpty(int ui_id)
        {
            if (CheckItemForHotbarStorageNotEmpty(ui_id))
            {
                return storageHotbar.CheckIfItemIsEmpty(hotbarUiElementIdList.IndexOf(ui_id));
            }
            else if (CheckItemForInventoryStorageNotEmpty(ui_id))
            {
                return storagePlayer.CheckIfItemIsEmpty(inventoryUiElementIdList.IndexOf(ui_id));
            }
            else
            {
                return true;
            }
        }

        internal void UnequipItem()
        {
            if(equippedItemStorageIndex != -1)
            {
                equippedItemStorageIndex = -1;
                equippedItemStorage = null;
            }
        }

        internal void EquipItem(int ui_id)
        {
            UnequipItem();
            if (hotbarUiElementIdList.Contains(ui_id))
            {
                equippedItemStorageIndex = hotbarUiElementIdList.IndexOf(ui_id);
                equippedItemStorage = storageHotbar;
            }else if (inventoryUiElementIdList.Contains(ui_id) && storagePlayer.CheckIfItemIsEmpty(inventoryUiElementIdList.IndexOf(ui_id))==false)
            {
                equippedItemStorageIndex = inventoryUiElementIdList.IndexOf(ui_id);
                equippedItemStorage = storagePlayer;
            }
            else
            {
                throw new Exception("No item with ui_id " + ui_id);
            }
        }

        internal void TakeOneFromItem(int ui_id)
        {
            if (CheckItemForHotbarStorageNotEmpty(ui_id))
            {
                storageHotbar.TakeFromItemWith(hotbarUiElementIdList.IndexOf(ui_id),1);
                //updateHotbarCallback();
            }
            else if (CheckItemForInventoryStorageNotEmpty(ui_id))
            {
                storagePlayer.TakeFromItemWith(inventoryUiElementIdList.IndexOf(ui_id), 1);
            }
            else
            {
                throw new Exception("No item with ui id " + ui_id);
            }
        }

        internal int GetItemCountFor(int ui_id)
        {
            if (CheckItemForHotbarStorageNotEmpty(ui_id))
            {
                return storageHotbar.GetCountOfItemWithIndex(hotbarUiElementIdList.IndexOf(ui_id));
            }else if (CheckItemForInventoryStorageNotEmpty(ui_id))
            {
                return storagePlayer.GetCountOfItemWithIndex(inventoryUiElementIdList.IndexOf(ui_id));
            }
            else
            {
                return -1;
            }
        }

        internal void RemoveItemFromInventory(int ui_id)
        {
            if (hotbarUiElementIdList.Contains(ui_id))
            {
                storageHotbar.RemoveItemOfIndex(hotbarUiElementIdList.IndexOf(ui_id));
            }else if (inventoryUiElementIdList.Contains(ui_id))
            {
                storagePlayer.RemoveItemOfIndex(inventoryUiElementIdList.IndexOf(ui_id));
            }
            else
            {
                throw new Exception("No item with id " + ui_id);
            }
            ResetSelectedItem();

        }

        public List<ItemData> GetItemsDataForHotbar()
        {
            return storageHotbar.GetItemsData();
        }

        public List<ItemData> GetItemsDataForInventory()
        {
            return storagePlayer.GetItemsData();
        }

        internal void SwapStorageItemsInsideInventory(int droppedItemID, int draggedItemID)
        {
            var storage_IdDraggedItem = inventoryUiElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storagePlayer.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = inventoryUiElementIdList.IndexOf(droppedItemID);

            if (CheckItemForInventoryStorageNotEmpty(droppedItemID))
            {
                
                var storagedata_IdDroppedItem = storagePlayer.GetItemData(storage_IdDroppedItem);

                storagePlayer.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else
            {
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storagePlayer.RemoveItemOfIndex(storage_IdDraggedItem);
            }

        }

        private bool CheckItemForInventoryStorageNotEmpty(int ui_id)
        {
            return inventoryUiElementIdList.Contains(ui_id) && storagePlayer.CheckIfItemIsEmpty(inventoryUiElementIdList.IndexOf(ui_id)) == false;
        }

        internal void SwapStorageItemsInsideHotbar(int droppedItemID, int draggedItemID)
        {
            var storage_IdDraggedItem = hotbarUiElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storageHotbar.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = hotbarUiElementIdList.IndexOf(droppedItemID);

            if (CheckItemForHotbarStorageNotEmpty(droppedItemID))
            {

                var storagedata_IdDroppedItem = storageHotbar.GetItemData(storage_IdDroppedItem);

                storageHotbar.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else
            {
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storageHotbar.RemoveItemOfIndex(storage_IdDraggedItem);
            }

        }

        private bool CheckItemForHotbarStorageNotEmpty(int ui_id)
        {
            return hotbarUiElementIdList.Contains(ui_id) && storageHotbar.CheckIfItemIsEmpty(hotbarUiElementIdList.IndexOf(ui_id)) == false;
        }

        internal void SwapStorageHotbarToInventory(int droppedItemID, int draggedItemID)
        {
            var storage_IdDraggedItem = hotbarUiElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storageHotbar.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = inventoryUiElementIdList.IndexOf(droppedItemID);

            if (CheckItemForInventoryStorageNotEmpty(droppedItemID))
            {

                var storagedata_IdDroppedItem = storagePlayer.GetItemData(storage_IdDroppedItem);

                storageHotbar.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else
            {
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storageHotbar.RemoveItemOfIndex(storage_IdDraggedItem);
            }
        }

        internal void SwapStorageInventoryToHotbar(int droppedItemID, int draggedItemID)
        {
            var storage_IdDraggedItem = inventoryUiElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storagePlayer.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = hotbarUiElementIdList.IndexOf(droppedItemID);

            if (CheckItemForHotbarStorageNotEmpty(droppedItemID))
            {

                var storagedata_IdDroppedItem = storageHotbar.GetItemData(storage_IdDroppedItem);

                storagePlayer.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else
            {
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storagePlayer.RemoveItemOfIndex(storage_IdDraggedItem);
            }
        }

        internal string GetItemIdFor(int ui_id)
        {
            if (CheckItemForInventoryStorageNotEmpty(ui_id))
            {
                return storagePlayer.GetIdOfItemWithIndex(inventoryUiElementIdList.IndexOf(ui_id));
            }else if (CheckItemForHotbarStorageNotEmpty(ui_id))
            {
                return storageHotbar.GetIdOfItemWithIndex(hotbarUiElementIdList.IndexOf(ui_id));
            }
            else
            {
                return null;
            }
        }

        public SavedItemSystemData GetDataToSave()
        {
            return new SavedItemSystemData
            {
                playerStorageItems = storagePlayer.GetDataToSave(),
                hotbarStorageItems = storageHotbar.GetDataToSave(),
                playerStorageSize = storagePlayer.StorageLimit
            };
        }

        public void LoadData(SavedItemSystemData dataToLoad)
        {
            storagePlayer = new Storage(dataToLoad.playerStorageSize);
            storageHotbar.ClearStorage();
            foreach (var item in dataToLoad.playerStorageItems)
            {
                if (item.IsNull == false)
                {
                    storagePlayer.SwapItemWithIndexFor(item.StorageIndex, item);
                }
            }
            foreach (var item in dataToLoad.hotbarStorageItems)
            {
                if (item.IsNull == false)
                {
                    storageHotbar.SwapItemWithIndexFor(item.StorageIndex, item);
                }
            }
            updateHotbarCallback.Invoke();
        }
    }

    [Serializable]
    public struct InventorySaveData
    {
        public List<ItemData> playerStorageItems, hotbarStorageItems;
        public int playerStorageSize;
    }

    [Serializable]
    public struct SavedItemSystemData
    {
        public List<ItemData> playerStorageItems;
        public List<ItemData> hotbarStorageItems;
        public int playerStorageSize;
    }
}



