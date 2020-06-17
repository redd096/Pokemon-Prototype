using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModel : IGetName
{
    public ItemData itemData;// { get; private set; }

    public int stack;

    public ItemModel(ItemData itemData)
    {
        //set data
        this.itemData = itemData;

        //add one stack
        AddItem(1);
    }

    public string GetButtonName()
    {
        return itemData.ItemName + " - x" + stack;
    }

    public string GetObjectName()
    {
        return itemData.ItemName;
    }

    public void AddItem(int quantity)
    {
        stack += quantity;
    }

    public void RemoveItem()
    {
        stack--;
    }
}
