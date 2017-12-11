using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {
    public static ItemDatabase instance;
    public List<Item> items = new List<Item>();
    
    void Awake()
    {
        instance = this;
    }
	// Use this for initialization
	void Start () {
        Add("hp", 10, 500, "hp potion", ItemType.HP,5);
        Add("coins", 1, 500, "coin", ItemType.Etc, 5);
	}
	
    void Add(string itemName,int itemValue, int itemPrice, string itemDesc, ItemType itemType, int maxcount)
    {
        items.Add(new Item(itemName, itemValue, itemPrice, itemDesc, itemType, Resources.Load<Sprite>("ItemImages/" + itemName),maxcount));
    }
    
}
