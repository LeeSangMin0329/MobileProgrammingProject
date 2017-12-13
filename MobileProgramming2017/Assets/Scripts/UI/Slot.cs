using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{

    public int number;
    public Item item;
    public static Inventory instance;

    public void OnPointerClick(PointerEventData data)
    {
        ItemUse();
    }

    public void ItemUse()
    {
        if (item == null)
            return;
        else
        {
            if (item.Count > 0)
            {
                item.Count--;
            }
            if (item.Count == 0)
            {
                item = null;
            }
            for (int i = 0; i < Inventory.instance.slotScripts.Count; i++)
            {
                if (Inventory.instance.slotScripts[i].item != null && item != null)
                {
                    if (Inventory.instance.slotScripts[i].item.itemName == item.itemName)
                    {
                        Inventory.instance.UpdateInventory(Inventory.instance.slotScripts[i].transform, i);
                    }
                }
                if(Inventory.instance.slotScripts[i].item == null)
                {
                    Inventory.instance.ItemImageChange(Inventory.instance.slotScripts[i].transform);
                }
            }
        }
        


    }


    /*public void OnDrag(PointerEventData data)
    {
        if(transform.childCount > 0)
        {
            transform.GetChild(0).parent = Inventory.instance.draggingItem;
        }
        Inventory.instance.draggingItem.GetChild(0).position = data.position;
    }*/

    /* public Stack<Item> slot;
     public Text text;
     public Sprite DefaultImg;

     private Image ItemImg;
     private bool isSlot;

     public Item ItemReturn() { return slot.Peek(); }
     public bool ItemMax(Item item) { return ItemReturn().MaxCount > slot.Count; }
     public bool isSlots() { return isSlot; }
     public void SetSlots(bool isSlot) { this.isSlot = isSlot; }


     // Use this for initialization
     void Start () {
         slot = new Stack<Item>();

         isSlot = false;

         float Size = text.gameObject.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
         text.fontSize = (int)(Size * 0.5f);

         ItemImg = transform.GetChild(0).GetComponent<Image>();
     }

     public void AddItem(Item item)
     {
         slot.Push(item);
        // UpdateInfo(true, item.DefaultImg);
     }

     public void ItemUse()
     {
         if(!isSlot)
         {
             return;
         }
         if(slot.Count == 1)
         {
             slot.Clear();
             UpdateInfo(false, DefaultImg);
             return;
         }

         slot.Pop();
         UpdateInfo(isSlot, ItemImg.sprite);
     }

     public void UpdateInfo(bool isSlot, Sprite sprite)
     {
         SetSlots(isSlot);
         ItemImg.sprite = sprite;
         text.text = slot.Count > 1 ? slot.Count.ToString() : "";
         //ItemIO.SaveDate();

     }*/
}
