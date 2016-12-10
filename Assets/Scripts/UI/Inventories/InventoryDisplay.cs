/**
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
    protected   ItemManager         im;

    private     bool                opened;
    private     Item[]              items;

    /**
    **/
    void Start()
    {
        im = GameObject.Find("Item Manager").GetComponent<ItemManager>();
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
    **/
    void Update()
    {
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

        if (items == newitems)
            return;

        items = newitems;
        RefreshInventory();
    }

    /**
    **/
    private void RefreshInventory()
    {
        //set icons and item count text for each tileset in the inventory based on the item
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i > items.Length-1)
            {
                displayWidgets[i].gameObject.SetActive(false);
                continue;
            }
            else if (!displayWidgets[i].gameObject.activeInHierarchy)
                displayWidgets[i].gameObject.SetActive(true);

            if (items[i] != null)
            {
                inventorySlots[i].icon.sprite = im.icons[(int)items[i].itemType];
                inventorySlots[i].text.text = (items[i].stackSize > 0 ? items[i].stackSize.ToString() : "");
            }
            else
            {
                inventorySlots[i].icon.sprite = im.icons[im.icons.Length-1];
                inventorySlots[i].text.text = "";
            }
        }
    }

    /**
    **/
    public virtual void Initialize()
    {
    }

    /**
    **/
    public virtual void UpdateTiles()
    {
    }

    /**
    **/
    public virtual IEnumerator Open()
    {
        yield break;
    }

    /**
    **/
    public virtual IEnumerator Close()
    {
        yield break;
    }
}
