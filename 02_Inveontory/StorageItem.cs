using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SVS.InventorySystem
{
    public class StorageItem : EqualityComparer<StorageItem>, IInventoryItem, IEquatable<StorageItem>
    {
        private string _id;

        public string ID
        {
            get { return _id; }
            private set { _id = value; }
        }

        private int _count;

        public int Count
        {
            get { return _count; }
            set
            {
                if (value == 0)
                {
                    _count = 0;
                    IsEmpty = true;
                    return;
                }
                if (IsStackable)
                {
                    if (value >= StackLimit)
                    {
                        _count = StackLimit;
                        IsFull = true;
                    }
                    else
                    {
                        _count = value;
                    }

                }
                else
                {
                    _count = (int)Mathf.Clamp01(value);
                }
            }
        }

        private int _stackLimit;

        public int StackLimit
        {
            get { return _stackLimit; }
            private set { _stackLimit = value; }
        }

        private bool _isStackable;

        public bool IsStackable
        {
            get { return _isStackable; }
            private set
            {
                _isStackable = value;
                if (!value)
                    IsFull = true;
            }
        }


        private bool _isFull;

        public bool IsFull
        {
            get { return _isFull; }
            private set
            {
                _isFull = value;
                if (value)
                    _isEmpty = false;
            }
        }


        private bool _isEmpty;

        public bool IsEmpty
        {
            get { return _isEmpty; }
            private set
            {
                _isEmpty = value;
                if (value)
                    _isFull = false;
            }
        }



        public StorageItem(string id, int count, bool isStackable = true, int stackLimit = 100)
        {
            ID = id;
            IsStackable = isStackable;
            StackLimit = stackLimit;
            Count = count;
            StackLimit = stackLimit;
        }
        /// <summary>
        /// Adds to item given amount. If overflowing returns quantity of unadded elements. 
        /// </summary>
        /// <param name="quantityToAdd"></param>
        /// <returns></returns>
        public virtual int AddToItem(int quantityToAdd)
        {
            if (IsStackable == false || IsFull)
            {
                return quantityToAdd;
            }
            int availableStorage = StackLimit - Count;
            if (availableStorage - quantityToAdd < 0)
            {
                Count = StackLimit;
                return quantityToAdd - availableStorage;
            }
            Count += quantityToAdd;
            return 0;
        }

        /// <summary>
        /// Reduces item Count by quantity..
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns>Returns how much was taken</returns>
        public virtual int TakeFromItem(int quantity)
        {
            if (quantity < Count)
            {
                Count -= quantity;
                return quantity;
            }
            else
            {
                var temp = Count;
                Count = 0;
                return temp;
            }
        }

        public virtual void ChangeStackLimit(int newLimit)
        {
            if (newLimit == 0)
            {
                throw new Exception("Stack limit cant be 0");
            }
            StackLimit = newLimit;
            if (Count >= StackLimit)
            {
                Count = StackLimit;
            }
            else
            {
                IsFull = false;
            }

        }

        public bool Equals(StorageItem other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(StorageItem x, StorageItem y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        public override int GetHashCode(StorageItem obj)
        {
            return obj.GetHashCode();
        }
    }

    
}