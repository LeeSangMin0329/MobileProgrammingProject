using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { HP, Bomb, Etc} // 체력, 폭탄, 기타

[System.Serializable]
public class Item {
    // 아이템 타입 정의

    public string itemName;
    public int itemValue;
    public int itemPrice;
    public string itemDesc;
    public ItemType itemType;
    public Sprite itemImage;
    public int Count;

    public Item(string _itemName, int _itemValue, int _itemPrice, string _itemDesc, ItemType _itemType, Sprite _itemImage, int _Count)
    {
        itemName = _itemName;
        itemValue = _itemValue;
        itemPrice = _itemPrice;
        itemDesc = _itemDesc;
        itemType = _itemType;
        itemImage = _itemImage;
        Count = _Count;
    }

   public Item()
    {

    }
}
