using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEquip : MonoBehaviour
{
    public enum InputType { None, User }

    [SerializeField] protected InputType inputType;
    public InputType InputOption { get { return inputType; } }
    [SerializeField] protected bool autoEquipItems;
    [SerializeField] protected bool setAllItemsActive;
    [SerializeField] protected int maxItems = 4;
    [SerializeField] protected bool disablePickupIfFull = true;
    [SerializeField] protected ItemDataManager itemManager;
    public ItemDataManager ItemManager { get { return itemManager; } }
    [SerializeField] protected ItemProperty[] itemsToAdd;
    protected ItemData[] itemDatas;
    [SerializeField] protected float dropThrowPower = 5;

    protected Unit unit;
    protected UnitController controller;

    protected Item curItem;
    public Item CurItem { get { return curItem; } }
    protected IUsable curUseable;
    public IUsable CurUseable { get { return curUseable; } }
    public bool CurItemInUse { get { if (curUseable != null) return curUseable.IsInUse; else return false; } }
    protected ItemAimable aimable;
    public ItemAimable Aimable { get { return aimable; } }

    protected ItemProperty[] curItemProperties;
    protected GameObject[] curItems;

    protected int curInd;
    protected bool equipped;

    public bool IsFull { get { return QuickMenuFull(); } }

    public virtual void Start()
    {
        GetComponents();
        SpawnItems();
    }

    public virtual void GetComponents()
    {
        unit = GetComponent<Unit>();
        controller = GetComponent<UnitController>();
    }

    public virtual void SpawnItems()
    {
        SpawnItemsFromData();
    }

    protected virtual void SpawnItemsFromData()
    {
        if (itemsToAdd.Length < 1)
            return;
        itemDatas = new ItemData[itemsToAdd.Length];
        curItems = new GameObject[itemsToAdd.Length];
        curItemProperties = new ItemProperty[itemsToAdd.Length];
        for (int i = 0; i < itemsToAdd.Length; i++)
        {
            if (itemsToAdd[i] != null)
            {
                AddItem(itemManager.GetItem(itemsToAdd[i].indexValue), i);
                if (i == 0)
                    SetCurItem(i);
            }   
            else
            {
                curItems[i] = null;
            }
        }
    }

    public virtual void UseEquippedItem(Collider _target = null)
    {
        if (!curItem)
            return;

        curUseable.Use();
    }

    public virtual void StopUseEquippedItem()
    {
        if (!curItem)
            return;

        curUseable.StopUse();
    }

    public virtual void AddItem(Item _item, int _ind = -1)
    {
        if (_ind != -1)
            SetItemToSlot(_item, _ind);
        else
        {
            for (int i = 0; i < curItems.Length; i++)
            {
                if (curItems[i] == null)
                {
                    SetItemToSlot(_item, i);
                    return;
                }
            }
        }
        
    }

    void SetItemToSlot(Item _item, int _ind)
    {
        if (_item != null)
        {
            var data = _item.Data;
            //add to data list
            itemDatas[_ind] = data;
            curItemProperties[_ind] = itemManager.GetItemProperty(data);
            //set item owner
            _item.SetOwner(unit);

            //add to items prefab list
            curItems[_ind] = _item.gameObject;

            if (!setAllItemsActive)
                SetCurItem(_ind);
            else
                SetAllItemsActive();
        }
        else
        {
            curItems[_ind] = null;
            itemDatas[_ind] = null;
            curItemProperties[_ind] = null;
        }
        
    }

    public virtual void PickupItem(Item _item, int _slotInd = -1)
    {
        if (disablePickupIfFull && QuickMenuFull())
            return;

        if (_slotInd > curItems.Length - 1)
        {
            Debug.Log("Slot: " + _slotInd + " does not exist on unit: " + this);
            return;
        }

        //instantiate not dropped prefab
        var item = Instantiate(_item.Data.connectedPrefab).GetComponent<Item>();
        //copy values from dropped item to this one
        item.CopyValues(_item);
        item.PickUp();
        //add item to the current inventory
        AddItem(item, _slotInd);
        //destroy the dropped item
        if (!_item.gameObject.IsPrefab())
            Destroy(_item.gameObject);
    }

    bool QuickMenuFull()
    {
        bool full = true;
        for (int i = 0; i < curItems.Length; i++)
        {
            if (curItems[i] == null)
                full = false;
        }
        return full;
    }

    //adding default item to list
    public virtual void AddItem(ItemData _itemData, int _ind = -1)
    {
        if (!_itemData)
            return;
        //get item
        var item = Instantiate(_itemData.connectedPrefab).GetComponent<Item>();
        AddItem(item, _ind);
    }

    public virtual void RemoveCurrentItem()
    {
        itemDatas[curInd] = null;
        equipped = false;
        if (curItem)
        {
            Destroy(curItem.gameObject);
            curItems[curInd] = null;
            curItem = null;
        }
        if (autoEquipItems)
        {
            FindItemToEquip();
        }

    }

    public virtual void RemoveItem(ItemData _itemToRemove)
    {
        for (int i = 0; i < itemDatas.Length; i++)
        {
            if (itemDatas[i] == _itemToRemove)
            {
                RemoveItem(i);
                return;
            }

        }
    }

    public virtual void RemoveItem(int _ind)
    {
        itemDatas[_ind] = null;
        curItemProperties[_ind] = null;
        Destroy(curItems[_ind]);
    }

    public virtual void DropCurrentItem()
    {
        if (!curItem)
            return;
        if (!curItem.Data.droppable)
            return;
        //instantiate cloned item and "drop" it
        var dropped = Instantiate(curItem.Data.droppedPrefab, curItem.transform.position, curItem.transform.rotation);
        var item = dropped.GetComponent<ItemFinite>();
        item.CopyValues(curItem);
        item.SetOwner(null);
        item.Drop();
        //push object away if it has rigidbody
        var irb = item.GetComponent<Rigidbody2D>();
        //if (irb)
            //irb.AddForce(controller.AimDirection * dropThrowPower, ForceMode2D.Impulse);
        //remove current item
        RemoveCurrentItem(); 
    }

    void FindItemToEquip()
    {
        for (int i = 0; i < curItems.Length; i++)
        {
            if (curItems[i] != null)
            {
                curInd = i;
                SetCurItem(i);
            }
                
        }
    }

    public virtual void EquipCurItem(bool _equip)
    {
        if (!curItem)
            return;
        equipped = _equip;
        curItem.gameObject.SetActive(equipped);
    }

    public virtual void SetAllItemsActive()
    {
        foreach (var item in curItems)
        {
            if (item != null)
                item.SetActive(true);
        }
        FindItemToEquip();
    }

    protected virtual void SwitchToNextItemForward()
    {
        curInd++;
        if (curInd > curItems.Length - 1)
            curInd = 0;
        SetCurItem(curInd);
    }

    protected virtual void SwitchToNextItemBackward()
    {
        curInd--;
        if (curInd < 0)
            curInd = curItems.Length - 1;
        SetCurItem(curInd);
    }

    protected virtual void SwitchActiveItem(int _itemInd)
    {
        for (int i = 0; i < curItems.Length; i++)
        {
            if (curItems[i] != null)
            {
                curItems[i].SetActive(i == _itemInd);
            }
            
        }
    }

    public virtual void SetCurItem(int _itemInd)
    {
        for (int i = 0; i < curItems.Length; i++)
        {
            if (i == _itemInd)
            {
                curInd = i;
                if (curItems[i])
                {
                    curItem = curItems[i].GetComponent<Item>();
                    aimable = curItem.GetComponent<ItemAimable>();

                    if (autoEquipItems || equipped)
                    {
                        if (!setAllItemsActive)
                            SwitchActiveItem(i);
                        EquipCurItem(true);
                    }
                }
                else
                    curItem = null;

            }     

        }     
    }

    public string[] GetItemNames()
    {
        var names = new string[itemsToAdd.Length];
        for (int i = 0; i < itemsToAdd.Length; i++)
        {
            names[i] = itemsToAdd[i].itemName;
        }
        return names;
    }

}
