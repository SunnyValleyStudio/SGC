using System;


namespace SVS.InventorySystem
{
    /// <summary>
    /// Interface providing a basic outline for the Data stored in the ItemStorage.
    /// </summary>
    public interface IInventoryItem 
    {
        string ID { get;}
        int Count { get;}
        bool IsStackable { get;}
        int StackLimit { get;}

        //void SetCount(int value);

    }
}