/**
**/

using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class InventoryComponent : MonoBehaviour
{
    [Header("Toggle Functionality")]
    public bool beingManaged    = false;

    [Header("Inventory Dimensions")]
    public int  inventoryWidth  = 3;
    public int  inventoryHeight = 2;

    private List<List<Item>> inventory = new List<List<Item>>();

    /**
    **/
    void Awake()
    {
        //create initial inventory space
        for (int y = 0; y < inventoryHeight; y++)
        {
            inventory.Add(new List<Item>());
            for (int x = 0; x < inventoryWidth; x++)
                inventory[y].Add(null);
        }
    }

    /**
    **/
    public bool AddItem(Item item)
    {
        Item transferred = CopyComponent(item, gameObject) as Item;
        bool added = _AddItem(transferred);

        if (added)
        {
            //Destroy(item.gameObject);
        }
        else
        {
            item.stackSize = transferred.stackSize;
            Destroy(transferred);
        }

        return added;
    }

    /**
    **/
    public bool AddItem(Type type)
    {
        Item item = gameObject.AddComponent(type) as Item;
        bool added = _AddItem(item);

        if (!added)
            Destroy(item);

        return added;
    }

    /**
    **/
    public bool _AddItem(Item item)
    {
        if (item.maxStackSize == 0)
        {
            if (InventoryFull())
                return false;

            Position slot = GetOpenSlot();
            inventory[slot.y][slot.x] = item;
            item.OnPickup();
            return true;
        }

        List<Item> items = GetItems(item.GetType());

        //items of same type exist in inventory - see if we can add to an item stack
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].stackSize < items[i].maxStackSize)
                {
                    uint space = items[i].maxStackSize - items[i].stackSize;

                    if (item.stackSize <= space)
                    {
                        items[i].stackSize += item.stackSize;
                        item.OnPickup();
                        Destroy(item);
                        return true;
                    }
                    else
                    {
                        items[i].stackSize += space;
                        item.stackSize -= space;
                    }
                }
            }

            //try to add item stack leftovers to a new slot
            if (item.stackSize > 0)
            {
                if (InventoryFull())
                    return false;

                Position slot = GetOpenSlot();
                inventory[slot.y][slot.x] = item;
                item.OnPickup();
                return true;
            }

            return false;
        }
        //no items of same type exist in inventory - add item to empty slot
        else
        {
            if (InventoryFull())
                return false;

            Position slot = GetOpenSlot();
            inventory[slot.y][slot.x] = item;
            item.OnPickup();
            return true;
        }
    }

    /**
    **/
    public bool DropItem(Position slot)
    {
        Item item = GetItem(slot);

        if (item == null)
            return false;

        item.OnUnequip();
        item.OnDrop();

        GameManager gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        ItemManager im = GameObject.Find("Item Manager").GetComponent<ItemManager>();

        GameObject dropped = Instantiate(im.prefabs[(int)item.itemType]) as GameObject;
        Pickup pickup = dropped.GetComponent<Pickup>() as Pickup;

        dropped.transform.SetParent(gm.propContainer);
        dropped.transform.position = gameObject.transform.position - Vector3.forward;
        dropped.transform.rotation = UnityEngine.Random.rotation;

        Rigidbody rb = dropped.GetComponent<Rigidbody>();

        Vector3 velocity = 2f * UnityEngine.Random.onUnitSphere;
        velocity.z = -Mathf.Abs(velocity.z);
        rb.velocity = velocity;

        Vector3 angular = 2f * UnityEngine.Random.insideUnitSphere;
        rb.angularVelocity = angular;

        //destroy default item on prefab and add one from inventory
        Destroy(dropped.GetComponent<Item>());
        pickup.item = CopyComponent(item, dropped) as Item;

        Destroy(inventory[slot.y][slot.x]);
        inventory[slot.y][slot.x] = null;
        return true;
    }

    /**
    **/
    public bool TransferItem(Position slot, InventoryComponent other_inventory)
    {
        Item item = GetItem(slot);

        if (item == null)
            return false;

        bool added = other_inventory.AddItem(item);

        if (!added)
            return false;

        item.OnUnequip();
        item.OnDrop();

        Destroy(inventory[slot.y][slot.x]);
        inventory[slot.y][slot.x] = null;
        return true;
    }

    /**
    **/
    public Item GetItem(Position slot)
    {
        if (!slot.Valid())
            return null;
        if (slot.y >= inventory.Count)
            if (slot.x >= inventory[slot.y].Count)
                return null;
        return inventory[slot.y][slot.x];
    }

    /**
    **/
    public Position GetItemSlot(Type type)
    {
        for (int y = 0; y < inventoryHeight; y++)
            for (int x = 0; x < inventoryWidth; x++)
                if (inventory[y][x].GetType() == type)
                    return new Position(x, y);
        return null;
    }

    /**
    **/
    public List<Item> GetItems(Type type)
    {
        List<Item> items = new List<Item>();

        if (type == null)
            return items;
        for (int y = 0; y < inventoryHeight; y++)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                if (inventory[y][x] == null)
                    continue;
                if (inventory[y][x].GetType() == type)
                    items.Add(inventory[y][x]);
            }
        }
        return items;
    }

    /**
    **/
    public void DeleteItem(Item item)
    {
        for (int y = 0; y < inventoryHeight; y++)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                if (inventory[y][x] == item)
                {
                    Destroy(inventory[y][x]);
                    inventory[y][x] = null;
                    return;
                }
            }
        }
    }

    /**
    **/
    public Item[] GetInventory()
    {
        Item[] items = new Item[inventoryWidth * inventoryHeight];

        for (int y = 0; y < inventoryHeight; y++)
            for (int x = 0; x < inventoryWidth; x++)
                items[inventoryWidth * y + x] = inventory[y][x];

        return items;
    }

    /**
    **/
    public void ResizeInventory(int width, int height)
    {
        if (width < 0)
            width = 1;
        if (height < 0)
            height = 1;

        //shrink/grow inventory column size
        if (height < inventoryHeight)
        {
            for (int y = inventoryHeight-1; y >= height; y--)
            {
                inventory.RemoveAt(y);
            }
        }
        else if (height > inventoryHeight)
        {
            for (int y = inventoryHeight; y < height; y++)
            {
                inventory.Add(new List<Item>());
                for (int x = 0; x < inventoryWidth; x++)
                    inventory[y].Add(null);
            }
        }

        inventoryHeight = height;

        //shrink/grow inventory row size
        if (width < inventoryWidth)
        {
            for (int y = 0; y < inventoryHeight; y++)
            {
                for (int x = inventoryWidth-1; x >= width; x--)
                {
                    //DropItem(i, j);
                    inventory[y].RemoveAt(x);
                }
            }
        }
        else if (width > inventoryWidth)
        {
            for (int i = 0; i < inventoryHeight; i++)
                for (int j = inventoryWidth; j < width; j++)
                    inventory[i].Add(null);
        }

        inventoryWidth = width;
    }

    /**
    **/
    public bool InventoryFull()
    {
        for (int y = 0; y < inventoryHeight; y++)
            for (int x = 0; x < inventoryWidth; x++)
                if (inventory[y][x] == null)
                    return false;
        return true;
    }

    /**
    **/
    public bool ItemExists(Position slot)
    {
        if (!slot.Valid())
            return false;
        if (inventory[slot.y][slot.x] == null)
            return false;
        return true;
    }

    /**
    **/
    public Position GetOpenSlot()
    {
        for (int y = 0; y < inventoryHeight; y++)
            for (int x = 0; x < inventoryWidth; x++)
                if (inventory[y][x] == null)
                    return new Position(x, y);
        return new Position(-1, -1);
    }

    /**
    **/
    public Position ValidPosition(Position position)
    {
        Position p = position;

        if (p.x < 0)
            p.x = inventoryWidth-1;
        else if (p.x > inventoryWidth-1)
            p.x = 0;
        if (p.y < 0)
            p.y = inventoryHeight-1;
        else if (p.y > inventoryHeight-1)
            p.y = 0;

        return p;
    }

    /**
    **/
    public override string ToString()
    {
        string output = "";
        for (int y = 0; y < inventoryHeight; y++)
        {
            for (int x = 0; x < inventoryWidth; x++)
            {
                try
                {
                    string name = (inventory[y][x] != null ? inventory[y][x].ToString() : "");
                    output += name + (x < inventoryWidth-1 ? ", " : "");
                }
                catch
                {
                    //do nothing
                }
            }
            output += "\n";
        }
        return output;
    }

    /**
    **/
    /*private Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        BindingFlags flags = BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Default|BindingFlags.DeclaredOnly;
        System.Reflection.FieldInfo[] fields = type.GetFields();
        PropertyInfo[] properties = type.GetProperties(flags);

        foreach (FieldInfo field in fields)
            field.SetValue(copy, field.GetValue(original));

        foreach (PropertyInfo property in properties)
            if (property.CanWrite)
                try {
                    property.SetValue(copy, property.GetValue(original, null), null);
                }
                catch { }

        return copy;
    }*/

    /**
    **/
    private Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
            field.SetValue(copy, field.GetValue(original));
        return copy;
    }

    /**
    **/
    public Position size { get { return new Position(inventoryWidth, inventoryHeight);  } }
}
