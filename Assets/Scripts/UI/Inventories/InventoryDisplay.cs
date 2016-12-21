/**
* InventoryDisplay.cs
* Created by Michael Marek (2016)
*
* Manages the layout, position, and display of an object's inventory system. This class links the
* Unity UI graphic to the object's attached inventory component and displays the contents.
*
* This class serves as a base class for others to inherit from so that multiple types of inventory
* displays can be supported (eg. playter inventory display may be different from a car trunk).
**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Displayed Inventory")]
    public      InventoryComponent  inventory;

    [Header("Inventory Display Slots")]
    public      GameObject[]        displayWidgets;

    protected   UIDisplayIcon[]     inventorySlots;
    protected   RectTransform       rect;
    protected   DataManager         data;

    private     bool                opened;
    private     Item[]              items;


    /**
    * Initialize the inventory display.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        data = GameObject.Find("Data Manager").GetComponent<DataManager>();
        rect = gameObject.GetComponent<RectTransform>();

        //create InventorySlot objects for each tileset in the inventory
        inventorySlots = new UIDisplayIcon[displayWidgets.Length];
        for (int i = 0; i < displayWidgets.Length; i++)
        {
            UIDisplayIcon slot = new UIDisplayIcon();
            slot.GrabDisplay(displayWidgets[i]);
            inventorySlots[i] = slot;
        }

        opened = false;
        items = new Item[0];

        Initialize();
    }

    /**
    * Update the inventory display.
    *
    * @param    null
    * @return   null
    **/
    void Update()
    {
        //open or close the inventory display depending if the inventory is being managed
        if (opened && !inventory.beingManaged)
        {
            StopAllCoroutines();
            StartCoroutine(Close());
        }
        else if (!opened && inventory.beingManaged)
        {
            StopAllCoroutines();
            StartCoroutine(Open());
        }

        opened = inventory.beingManaged;

        if (!opened)
            return;

        UpdateTiles();

        Item[] newitems = inventory.GetInventory();

        //no need to update display if inventory hasn't changed
        if (items == newitems)
            return;

        items = newitems;
        RefreshInventory();
    }


    /**
    * Refresh the inventory display icons to represent the current items found in the inventory.
    *
    * @param    null
    * @return   null
    **/
    private void RefreshInventory()
    {
        //set icons and item count text for each tileset in the inventory based on the item
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            //hide excess display tiles if our inventory size is too small
            if (i > items.Length-1)
            {
                displayWidgets[i].gameObject.SetActive(false);
                continue;
            }
            else if (!displayWidgets[i].gameObject.activeInHierarchy)
            {
                displayWidgets[i].gameObject.SetActive(true);
            }

            if (items[i] != null)
            {
                inventorySlots[i].icon.sprite = data.icons[(int)items[i].itemType];
                inventorySlots[i].text.text = (items[i].stackSize > 0 ? items[i].stackSize.ToString() : "");
            }
            else
            {
                inventorySlots[i].icon.sprite = data.icons[data.icons.Length-1];
                inventorySlots[i].text.text = "";
            }
        }
    }


    /**
    * Overridable method for initializing the inventory display.
    *
    * @param    null
    * @return   null
    **/
    public virtual void Initialize()
    {
    }


    /**
    * Overridable method for updating the inventory display tiles every frame.
    *
    * @param    null
    * @return   null
    **/
    public virtual void UpdateTiles()
    {
    }


    /**
    * Overridable method for providing an "opening" animation for the inventory display.
    *
    * @param    null
    * @return   null
    **/
    public virtual IEnumerator Open()
    {
        yield break;
    }


    /**
    * Overridable method for providing a "closing" animation for the inventory display.
    *
    * @param    null
    * @return   null
    **/
    public virtual IEnumerator Close()
    {
        yield break;
    }
}
