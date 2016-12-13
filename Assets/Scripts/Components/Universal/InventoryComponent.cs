/**
* InventoryComponent.cs
* Created by Michael Marek (2016)
*
* An inventory system for storing and maintaining a collection of items held by the parent Game
* Object.
**/

using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class InventoryComponent : MonoBehaviour
{
    [Header("Toggle Functionality")]
    public  bool                beingManaged    = false;

    [Header("Inventory Dimensions")]
    public  Position            inventorySize   = new Position(3, 1);

    private List<List<Item>>    inventory       = new List<List<Item>>();


    /**
    * Create the initial inventory space.
    *
    * @param    null
    * @return   null
    **/
    void Awake()
    {
        for (int y = 0; y < inventorySize.y; y++)
        {
            inventory.Add(new List<Item>());
            for (int x = 0; x < inventorySize.x; x++)
                inventory[y].Add(null);
        }
    }



    /**
    * Add an existing item to the inventory.
    *
    * @param    Item    the item to be added
    * @return   bool    true if the item was added successfully; false otherwise
    **/
    public bool AddItem(Item item)
    {
        Item transferred = CopyComponent(item, gameObject) as Item;
        bool added = _AddItem(transferred);

        //item couldn't be added; delete duplicate item
        //need to set item's stack size as _AddItem() might have potentially taken some from it
        if (!added)
        {
            item.stackSize = transferred.stackSize;
            Destroy(transferred);
        }

        return added;
    }


    /**
    * Add a new item of a specific type to the inventory.
    *
    * @param    Type    the type of item to add
    * @return   bool    true if the item was added successfully; false otherwise
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
    * Attempt to add an item or a partial stack of an item to the inventory.
    *
    * @param    Item    the item to be added
    * @return   bool    true if the item was added successfully; false otherwise
    **/
    public bool _AddItem(Item item)
    {
        //item is not stackable; try to add item to empty slot
        if (item.maxStack == 0)
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
                if (items[i].stackSize < items[i].maxStack)
                {
                    uint space = items[i].maxStack - items[i].stackSize;

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

            //try to add item stack leftovers to an empty slot
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
    * Drop an item from the inventory.
    *
    * @param    Position    the position in the inventory of the item to be dropped
    * @return   bool        true if the item was dropped successfully; false otherwise
    **/
    public bool DropItem(Position slot)
    {
        Item item = GetItem(slot);

        if (item == null)
            return false;

        item.OnUnequip();

        CreatePickup(item);

        Destroy(inventory[slot.y][slot.x]);
        inventory[slot.y][slot.x] = null;

        return true;
    }


    /**
    * Transfer an item from this inventory to another inventory.
    *
    * @param    Position            the position in the inventory of the item to be transferred
    * @param    InventoryComponent  the inventory the item will be transferred to
    * @return   bool                true if the item was transferred successfully; false otherwise
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
    * Retrieve an item from the inventory.
    *
    * @param    Position    position in the inventory of the item to be retrieved
    * @return   Item        a reference to the item; null if slot was empty
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
    * Get the position in the inventory of the first occurance of a particular type of item.
    *
    * @param    Type        type of item to search for
    * @return   Position    location in the inventory of the item; null if item doesn't exist
    **/
    public Position GetItemSlot(Type type)
    {
        for (int y = 0; y < inventorySize.y; y++)
            for (int x = 0; x < inventorySize.x; x++)
                if (inventory[y][x].GetType() == type)
                    return new Position(x, y);
        return null;
    }


    /**
    * Returns a list of references to all items of a particular type found in the inventory.
    *
    * @param    Type        the type of item to search for
    * @return   List<Item>  the list of all items found
    **/
    public List<Item> GetItems(Type type)
    {
        List<Item> items = new List<Item>();

        if (type == null)
            return items;
        for (int y = 0; y < inventorySize.y; y++)
        {
            for (int x = 0; x < inventorySize.x; x++)
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
    * Delete an item at a specified position from the inventory .
    *
    * @param    Position    the position in the inventory of the item to be deleted
    * @return   bool        true if the item was destroyed successfully; false otherwise
    **/
    public bool DeleteItem(Position slot)
    {
        if (inventory[slot.y][slot.x] != null)
        {
            Destroy(inventory[slot.y][slot.x]);
            inventory[slot.y][slot.x] = null;
            return true;
        }

        return false;
    }


    /**
    * Return the entire contents of the inventory as a flat array (empty slots are null).
    *
    * @param    null
    * @return   Item[]  an array of items contained in the inventory
    **/
    public Item[] GetInventory()
    {
        Item[] items = new Item[inventorySize.x * inventorySize.y];

        for (int y = 0; y < inventorySize.y; y++)
            for (int x = 0; x < inventorySize.x; x++)
                items[inventorySize.x * y + x] = inventory[y][x];

        return items;
    }


    /**
    * Resize the inventory to a new size. If the inventory shrinks, we simply drop any items that
    * can no longer fit.
    *
    * @param    int     new width of the inventory
    * @param    int     new height of the inventory
    * @return   null
    **/
    public void ResizeInventory(int width, int height)
    {
        //make sure the inventory isn't completely empty
        if (width < 1)  width = 1;
        if (height < 1) height = 1;

        //list of items that are removed during resizing; we will try to add them later
        List<Item> leftovers = new List<Item>();

        //shrink/grow inventory column size
        if (height < inventorySize.y)
        {
            for (int y = inventorySize.y-1; y >= height; y--)
            {
                for (int x = 0; x < inventorySize.x; x++)
                    if (inventory[y][x] != null)
                        leftovers.Add(inventory[y][x]);
                inventory.RemoveAt(y);
            }
        }
        else if (height > inventorySize.y)
        {
            for (int y = inventorySize.y; y < height; y++)
            {
                inventory.Add(new List<Item>());
                for (int x = 0; x < inventorySize.x; x++)
                    inventory[y].Add(null);
            }
        }

        inventorySize.y = height;

        //shrink/grow inventory row size
        if (width < inventorySize.x)
        {
            for (int y = 0; y < inventorySize.y; y++)
            {
                for (int x = inventorySize.x-1; x >= width; x--)
                {
                    if (inventory[y][x] != null)
                        leftovers.Add(inventory[y][x]);
                    inventory[y].RemoveAt(x);
                }
            }
        }
        else if (width > inventorySize.x)
        {
            for (int i = 0; i < inventorySize.y; i++)
                for (int j = inventorySize.x; j < width; j++)
                    inventory[i].Add(null);
        }

        inventorySize.x = width;

        //attempt to add back any items that were removed during resizing
        //if we can't add back the leftover items, just throw them on the ground
        while (leftovers.Count > 0)
        {
            if (AddItem(leftovers[0]))
            {
                leftovers.RemoveAt(0);
            }
            else
            {
                CreatePickup(leftovers[0]);
                leftovers.RemoveAt(0);
            }
        }
    }


    /**
    * Check to see if we have any space left in our inventory.
    *
    * @param    null
    * @return   bool    true if there's at least one open slot; false otherwise
    **/
    public bool InventoryFull()
    {
        for (int y = 0; y < inventorySize.y; y++)
            for (int x = 0; x < inventorySize.x; x++)
                if (inventory[y][x] == null)
                    return false;
        return true;
    }


    /**
    * Get the position of the first open slot in the inventory.
    *
    * @param    null
    * @return   Position    position of the open slot; null if no open slot
    **/
    public Position GetOpenSlot()
    {
        for (int y = 0; y < inventorySize.y; y++)
            for (int x = 0; x < inventorySize.x; x++)
                if (inventory[y][x] == null)
                    return new Position(x, y);
        return null;
    }


    /**
    * Return a valid inventory position. If the position falls below zero or exceeds the maximum
    * size of the inventory we simply wrap the value around.
    *
    * @param    Position    position that needs to be validated
    * @return   Position    valid inventory position
    **/
    public Position ValidPosition(Position position)
    {
        Position p = position;

        if (p.x < 0)
            p.x = inventorySize.x-1;
        else if (p.x > inventorySize.x-1)
            p.x = 0;
        if (p.y < 0)
            p.y = inventorySize.y-1;
        else if (p.y > inventorySize.y-1)
            p.y = 0;

        return p;
    }


    /**
    * Convert the contents of the inventory to a formatted string.
    *
    * @param    null
    * @return   string  display string of all items in the inventory
    **/
    public override string ToString()
    {
        string output = "";
        for (int y = 0; y < inventorySize.y; y++)
        {
            for (int x = 0; x < inventorySize.x; x++)
            {
                try
                {
                    string name = (inventory[y][x] != null ? inventory[y][x].ToString() : "");
                    output += name + (x < inventorySize.x-1 ? ", " : "");
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
    * Create a deep copy of a component on a Game Object and add it to another Game Object. This
    * allows us to transfer components (inventory items) between different Game Objects during
    * runtime. Please remember to delete the original component after so we don't have any
    * lingering duplicates of objects lying around!
    *
    * @param    Component   the component we wish to make a copy of
    * @param    GameObject  the GameObject the cloned component will end up on
    * @return   Component   a reference to the newly created component
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
    * Create an interactable pickup object with a specific item attached on it.
    *
    * @param    Item    the item we wish to create a pickup instance of
    * @return   null
    **/
    private void CreatePickup(Item item)
    {
        item.OnDrop();

        SceneManager sm = GameObject.Find("Scene Manager").GetComponent<SceneManager>();
        ItemManager im = GameObject.Find("Item Manager").GetComponent<ItemManager>();

        GameObject dropped = Instantiate(im.prefabs[(int)item.itemType]) as GameObject;
        Pickup pickup = dropped.GetComponent<Pickup>() as Pickup;

        dropped.transform.SetParent(sm.propContainer);
        dropped.transform.position = gameObject.transform.position - Vector3.forward;
        dropped.transform.rotation = UnityEngine.Random.rotation;

        Rigidbody rb = dropped.GetComponent<Rigidbody>();

        //apply small velocity to item and make it shoot into the air
        Vector3 velocity = 2f * UnityEngine.Random.onUnitSphere;
        velocity.z = -Mathf.Abs(velocity.z);
        rb.velocity = velocity;

        //apply small angular velocity to item and make it spin n' stuff
        Vector3 angular = 2f * UnityEngine.Random.insideUnitSphere;
        rb.angularVelocity = angular;

        //destroy default item on prefab and add one from inventory
        Destroy(dropped.GetComponent<Item>());
        pickup.item = CopyComponent(item, dropped) as Item;
    }


    public Position size { get { return new Position(inventorySize.x, inventorySize.y);  } }
}
