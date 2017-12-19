using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public static Inventory instance;
    public Transform slot;
    public List<Slot> slotScripts = new List<Slot>();
    // public Text text;
    //public Transform draggingItem;
    
    public RectTransform InvenRect;

    public float slotSize;
    public float slotGap;
    public int slotCount;

    private float EmptySlot;
    
    void Awake()
    {
        Inventory.instance = this;
    }

    void Start()
    {
        InvenRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (slotSize * slotCount) + (slotGap * (slotCount + 1)));
        InvenRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize + slotGap);
        for (int x = 0; x<slotCount; x++)
        {
            Transform newslot = Instantiate(slot);
            newslot.name = "slot_" + x;
            newslot.SetParent(transform);
            RectTransform slotRect = newslot.GetComponent<RectTransform>();
     
            RectTransform itemslot = slot.transform.GetChild(0).GetComponent<RectTransform>();
            

            slotRect.localPosition = new Vector3((slotSize * x) + (slotGap * (x + 1)), -((slotSize) + slotGap), 0);

            slotRect.localScale = Vector3.one;
            slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
            slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

            itemslot.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize - slotSize * 0.3f);
            itemslot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize - slotSize * 0.3f);

            newslot.GetComponent<Slot>().number = x;
            newslot.GetChild(0).gameObject.SetActive(false);
            slotScripts.Add(newslot.GetComponent<Slot>());
        }
        EmptySlot = slotScripts.Count;
        AddItem(0);
        AddItem(1);

    }

    public void ItemImageChange(Transform _slot)
    {
        if(_slot.GetComponent<Slot>().item == null)
        {
            _slot.GetChild(0).gameObject.SetActive(false);
        }
        else if(!(_slot.GetComponent<Slot>().item.itemValue == 0))
        {
            _slot.GetChild(0).gameObject.SetActive(true);
            _slot.GetChild(0).GetComponent<Image>().sprite = _slot.GetComponent<Slot>().item.itemImage;
        }
        
    }

    public bool ItemNullCheck(Transform _slot)
    {
        if (null == slot.GetComponent<Slot>().item)
        {
            _slot.GetChild(0).gameObject.SetActive(false);
            return false;
        }
        if (_slot.GetComponent<Slot>().item.itemValue == 0)
        {
            _slot.GetChild(0).gameObject.SetActive(false);
            return false;
        }
        else
        {
            _slot.GetChild(0).gameObject.SetActive(true);
            return true;
        }
    }

    public void UpdateInventory(Transform _item, int slotnum)
    {
        ItemNullCheck(slotScripts[slotnum].transform);
        ItemCountChange(slotScripts[slotnum].transform);
        ItemNameChange(slotScripts[slotnum].transform);
        ItemImageChange(slotScripts[slotnum].transform);
    }
    void ItemCountChange(Transform _slot)
    {
        if (!(_slot.GetComponent<Slot>().item.itemValue == 0))
        {
            _slot.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = _slot.GetComponent<Slot>().item.Count.ToString();
        }
    }
    
    void ItemNameChange(Transform _slot)
    {
        if (!(_slot.GetComponent<Slot>().item.itemValue == 0))
        {
            _slot.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = _slot.GetComponent<Slot>().item.itemName;
        }
    }

    //아이템 넣기위해 모든슬롯 검사
    void  AddItem(int number)
    {
       for(int i=0;i<slotScripts.Count;i++)
        {
            if (slotScripts[i].item.itemValue == 0)
            {
                slotScripts[i].item = ItemDatabase.instance.items[number];
                UpdateInventory(slotScripts[i].transform, i);
                break;
            }
        }
    }

    public bool hpItemTrigger = false;
    public void useHpItem()
    {
        hpItemTrigger = true;
    }
}
