using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : UnitEquip
{
    public enum LoadInventoryType { FromUserData, Override }

    [SerializeField] private LoadInventoryType loadInventory = LoadInventoryType.FromUserData;
    [SerializeField] private UserDataManager userDataManager = null;
    [SerializeField] private IndexStringProperty user = null;
    [SerializeField] private InputProperty[] quickMenuButtons = null;
    [SerializeField] private InputProperty interactButton;
    [SerializeField] private AimReticalFX defaultAimFX;
    [SerializeField] private bool enableToggleSwitch = false;
    [SerializeField] private InputProperty toggleForwardsButton = null;
    [SerializeField] private InputProperty toggleBackwardsButton = null;
    [SerializeField] private InputProperty equipButton = null;
    [SerializeField] private InputProperty dropButton = null;

    //private UserDataManager userData;
    private QuickMenuUI quickMenu;
    private Player player;
    private UserDataManager dataManager;

    private void Update()
    {
        GetInputs();
    }

    public override void GetComponents()
    {
        base.GetComponents();
        player = (Player)unit;
        GameManager gm = GameManager.instance;
        dataManager = gm.GetUserDataManager();
        if (player.UI)
            quickMenu = player.UI.QuickMenu;
    }

    public override void SpawnItems()
    {
        base.SpawnItems();
        if (quickMenu)
            quickMenu.UpdateQuickMenuItems(itemDatas);
    }

    protected override void SpawnItemsFromData()
    {
        if (loadInventory == LoadInventoryType.FromUserData)
        {
            itemsToAdd = userDataManager.GetInventoryItems(user.indexValue);
            curItems = new GameObject[itemsToAdd.Length];
            itemDatas = new ItemData[itemsToAdd.Length];
        }
        base.SpawnItemsFromData();
    }

    void GetInputs()
    {
        //loop through all quick commands
        if (curItems != null)
        {

            for (int i = 0; i < curItems.Length; i++)
            {
                if (Input.GetButtonDown(quickMenuButtons[i].stringValue))
                {
                    SetCurItem(i);
                }
            }

            if (enableToggleSwitch)
            {
                if (Input.GetButtonDown(toggleForwardsButton.stringValue))
                    SwitchToNextItemForward();
                if (Input.GetButtonDown(toggleBackwardsButton.stringValue))
                    SwitchToNextItemBackward();
            }

        }

        if (Input.GetButtonDown(equipButton.stringValue) && !autoEquipItems)
        {
            EquipCurItem(!equipped);
        }

        if (equipped)
        {

            if (dropButton.GetInputDown())
                DropCurrentItem();

        }

    }

    public override void AddItem(Item _item, int _ind = -1)
    {
        base.AddItem(_item, _ind);
        if (quickMenu)
            quickMenu.UpdateQuickMenuItems(itemDatas);
        SaveItemsToData();
    }

    public override void RemoveCurrentItem()
    {
        base.RemoveCurrentItem();
        //update ui
        if (quickMenu)
            quickMenu.EquipToSlot(null, curInd);
        SaveItemsToData();
    }

    public override void RemoveItem(int _ind)
    {
        base.RemoveItem(_ind);
        //update ui
        if (quickMenu)
            quickMenu.EquipToSlot(null, _ind);
        SaveItemsToData();
    }

    void SaveItemsToData()
    {
        if (dataManager && loadInventory == LoadInventoryType.FromUserData)
            dataManager.SetInventoryItems(curItemProperties, user.indexValue);
    }

    public override void SetCurItem(int _itemInd)
    {
        base.SetCurItem(_itemInd);
        //update ui
        if (quickMenu)
            quickMenu.SetActiveSlot(_itemInd);
    }

    protected override void SwitchActiveItem(int _itemInd)
    {
        base.SwitchActiveItem(_itemInd);
        //ui
        if (quickMenu)
            quickMenu.SetActiveSlot(_itemInd);
    }

}
