using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SVS.InventorySystem
{
    public class Storage
    {
        int storageLimit = 0;

        public int StorageLimit { get => storageLimit; private set => storageLimit = value; }

        List<StorageItem> storageItems;
        public Storage(int storageSize)
        {
            this.StorageLimit = storageSize;
            storageItems = new List<StorageItem>();
            for (int i = 0; i < this.StorageLimit; i++)
            {
                storageItems.Add(null);
            }
        }

        /// <summary>
        /// Returns 0 if successfuly added. Else return amoun that is left.
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <returns></returns>
        public int AddItem(IInventoryItem inventoryItem)
        {
            int remainingAmount = TryAddingToAnExistingItem(inventoryItem.ID, inventoryItem.Count);
            if (remainingAmount == 0 || CheckIfStorageIsFull())
            {
                return remainingAmount;
            }

            StorageItem newStorageItem = new StorageItem(inventoryItem.ID, remainingAmount, inventoryItem.IsStackable, inventoryItem.StackLimit);

            int nullItemIndex = storageItems.FindIndex(x => x == null);
            
            if (nullItemIndex == -1)
            {
                
                storageItems.Add(newStorageItem);
            }
            else
            {
                storageItems[nullItemIndex] = newStorageItem;
            }
            return 0;
        }

        /// <summary>
        /// Returns 0 if successfuly added. Else return amoun that is left.
        /// </summary>
        /// <param name="inventoryItem"></param>
        /// <returns></returns>
        public int AddItem(string ID, int count, bool isStackable = true, int stackLimit = 100)
        {
            int remainingAmount = TryAddingToAnExistingItem(ID, count);
            if (remainingAmount == 0 || CheckIfStorageIsFull())
            {
                return remainingAmount;
            }
            StorageItem newStorageItem = new StorageItem(ID, remainingAmount, isStackable, stackLimit);

            int nullItemIndex = storageItems.FindIndex(x => x == null);

            if (nullItemIndex == -1)
            {

                storageItems.Add(newStorageItem);
            }
            else
            {
                storageItems[nullItemIndex] = newStorageItem;
            }

            return 0;
        }

        internal int GetItemCount(string ID)
        {
            int quantity = 0;
            foreach (var item in storageItems)
            {
                if (item == null)
                    continue;
                if (item.ID == ID)
                    quantity += item.Count;
            }
            return quantity;
        }

        /// <summary>
        /// Swaps item with Index to provided InventoryItem data
        /// </summary>
        /// <param name="index"></param>
        /// <param name="inventoryItemData"></param>
        public void SwapItemWithIndexFor(int index, IInventoryItem inventoryItemData)
        {
            storageItems[index] = null;
            StorageItem newStorageItem = new StorageItem(inventoryItemData.ID, inventoryItemData.Count, inventoryItemData.IsStackable, inventoryItemData.StackLimit);
            storageItems[index] = newStorageItem;
        }

        
        /// <summary>
        /// Returns false if any element in the storage is null. To add to existing item use AddItem method.
        /// </summary>
        /// <returns></returns>
        public bool CheckIfStorageIsFull()
        {
            return storageItems.Any(x => x == null)==false;
        }

        private int TryAddingToAnExistingItem(string ID, int itemCount)
        {
            if (itemCount == 0)
            {
                return 0;
            }
            if (CheckIfStorageContains(ID))
            {
                foreach (var item in storageItems)
                {
                    if (item != null && item.ID == ID && item.IsFull == false)
                    {
                        itemCount = item.AddToItem(itemCount);
                    }
                    if (itemCount == 0)
                    {
                        return 0;
                    }
                }
            }
            return itemCount;
        }

        /// <summary>
        /// Returns true if storage contains item with DataID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool CheckIfStorageContains(string ID)
        {
            return storageItems.Any(x => x != null && x.ID == ID);
        }

        /// <summary>
        /// If storage contains enought of item with ID it will be subtracted from the storage.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="quantity"></param>
        /// <returns>True if item was taken</returns>
        public bool TakeItemFromStorageIfContaintEnough(string ID, int quantity)
        {
            if (CheckIfStorageHasEnoughOfItemWith(ID, quantity) == false)
            {
                return false;
            }
            for (int i = storageItems.Count - 1; i >= 0; i--)
            {
                if(storageItems[i] == null)
                {
                    continue;
                }
                if (storageItems[i].ID == ID)
                {
                    quantity -= storageItems[i].TakeFromItem(quantity);
                }
                if (storageItems[i].IsEmpty)
                {
                    storageItems[i] = null;
                }
                if (quantity <= 0)
                {
                    return true;
                }
            }
            throw new Exception("Something went wrong. Storage has the amount but didnt return true when deleting items");
        }

        /// <summary>
        /// Take AmountToTake of items from item with Index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="amountToTake"></param>
        /// <returns>How much was taken</returns>
        public int TakeFromItemWith(int index, int amountToTake)
        {
            if(storageItems[index]==null )
            {
                return 0;
            }
            if(storageItems[index].Count < amountToTake)
            {
                storageItems[index] = null;
                return storageItems[index].Count;
            }
            storageItems[index].TakeFromItem(amountToTake);
            if (storageItems[index].IsEmpty)
            {
                storageItems[index] = null;
            }
            return amountToTake;
        }

        /// <summary>
        /// Removes item with Index from storage without any checks.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveItemOfIndex(int index)
        {
            storageItems[index] = null;
        }

        /// <summary>
        /// Check if storage contains AmountToTake of item with Index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="amountToTake"></param>
        /// <returns>True if storage contains enough item</returns>
        public bool CheckIfStorageHasEnoughOfItemWith(string index, int amountToTake)
        {
            int quantity = 0;
            foreach (var item in storageItems)
            {
                if (item == null)
                    continue;
                if (item.ID == index)
                {
                    quantity += item.Count;
                }
                if (quantity >= amountToTake)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Increases the storage size;
        /// </summary>
        /// <param name="capacity"></param>
        public void UpgradeStorage(int capacity)
        {
            StorageLimit += capacity;
            for (int i = 0; i < capacity; i++)
            {
                storageItems.Add(null);
            }
        }

        /// <summary>
        /// Removes all the stoage items.
        /// </summary>
        public void ClearStorage()
        {
            storageItems.Clear();
            for (int i = 0; i < this.StorageLimit; i++)
            {
                storageItems.Add(null);
            }
        }

        /// <summary>
        /// Returns copy of data for all the storage items.
        /// </summary>
        /// <returns>List of ItemData for all items inside storage</returns>
        public List<ItemData> GetItemsData()
        {
            List<ItemData> valueToReturn = new List<ItemData>();
            for (int i = 0; i < this.StorageLimit; i++)
            {
                if (storageItems[i] != null)
                {
                    valueToReturn.Add(new ItemData(i, storageItems[i].Count, storageItems[i].ID, storageItems[i].IsStackable, storageItems[i].StackLimit));
                }
                else
                {
                    valueToReturn.Add(new ItemData(true));
                }
            }

            return valueToReturn;
        }

        /// <summary>
        /// Returns a data copy of item with Index
        /// </summary>
        /// <param name="index"></param>
        /// <returns>ItemData for the item in the storage. Null if not present</returns>
        public ItemData GetItemData(int index)
        {
            if (storageItems[index] == null)
            {
                return new ItemData(true);
            }
            return new ItemData(index, storageItems[index].Count, storageItems[index].ID, storageItems[index].IsStackable, storageItems[index].StackLimit);
        }

        /// <summary>
        /// Returns the ID of the data stored inside the storage item.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>ID of the storad item. null if not present.</returns>
        public string GetIdOfItemWithIndex(int index)
        {
            if (storageItems[index] == null)
            {
                return null;
            }
            return storageItems[index].ID;
        }

        /// <summary>
        /// Returns the Count of the data stored inside the storage item.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Count of the stored item. -1 if not present.</returns>
        public int GetCountOfItemWithIndex(int index)
        {
            if (storageItems[index] == null)
            {
                return -1;
            }
            return storageItems[index].Count;
        }

        /// <summary>
        /// Checks if item with Index is empty.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>True if item is null</returns>
        public bool CheckIfItemIsEmpty(int index)
        {
            return storageItems[index] == null;
        }

        /// <summary>
        /// Gets index of the first element with null value
        /// </summary>
        /// <returns>int index value</returns>
        public int GetIndexOfEmptyStorageSpace()
        {
            return storageItems.IndexOf(null);
        }

        public List<ItemData> GetDataToSave()
        {
            List<ItemData> valueToReturn = new List<ItemData>();
            for (int i = 0; i < this.StorageLimit; i++)
            {
                if (storageItems[i] != null)
                {
                    valueToReturn.Add(new ItemData(i, storageItems[i].Count, storageItems[i].ID, storageItems[i].IsStackable, storageItems[i].StackLimit));
                }
                else
                {
                    valueToReturn.Add(new ItemData(true));
                }
            }

            return valueToReturn;
        }

    }

    /// <summary>
    /// Struct that will be used to pass the data from inventory as a copy - ex. for save system.
    /// </summary>
    [Serializable]
    public struct ItemData : IInventoryItem
    {
        [SerializeField]
        private bool isNull;
        [SerializeField]
        private int storageIndex;
        [SerializeField]
        private string id;
        [SerializeField]
        private int count;
        [SerializeField]
        private bool isStackable;
        [SerializeField]
        private int stackLimit;

        public bool IsNull
        {
            get { return isNull; }
            private set { isNull = value; }
        }
        public int StorageIndex
        {
            get { return storageIndex; }
            private set { storageIndex = value; }
        }
        public string ID
        {
            get { return id; }
            private set { id = value; }
        }

        public int Count
        {
            get { return count; }
            private set { count = value; }
        }

        public bool IsStackable
        {
            get { return isStackable; }
            private set { isStackable = value; }
        }

        public int StackLimit
        {
            get { return stackLimit; }
            private set { stackLimit = value; }
        }

        /// <summary>
        /// Default non-null item constructor
        /// </summary>
        /// <param name="storageIndex"></param>
        /// <param name="count"></param>
        /// <param name="id"></param>
        /// <param name="isStackable"></param>
        /// <param name="stackLimit"></param>
        public ItemData(int storageIndex, int count, string id, bool isStackable, int stackLimit)
        {
            this.id = id;
            this.count = count;
            this.storageIndex = storageIndex;
            this.isStackable = isStackable;
            this.stackLimit = stackLimit;
            this.isNull = false;
        }
        /// <summary>
        /// Default NUll item contructor
        /// </summary>
        public ItemData(bool isNull = true)
        {
            this.id = "";
            this.count = -1;
            this.storageIndex = -1;
            this.isStackable = false;
            this.stackLimit = -1;
            this.isNull = true;
        }
    }
}