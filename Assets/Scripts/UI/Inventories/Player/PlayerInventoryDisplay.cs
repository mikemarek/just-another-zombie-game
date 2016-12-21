/**
* PlayerInventoryDisplay.cs
* Created by Michael Marek (2016)
*
* Manages the layout, position, and display of a players's inventory system. Link the Unity UI
* graphic to the players's attached inventory component and displays the contents.
**/

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInventoryDisplay : InventoryDisplay
{
    [Header("Inventory Slot Highlight Icons")]
    [Tooltip("0:selected  1:equipped  2:hotkeyed")]
    public  GameObject[]                highlights      = new GameObject[3];

    [Header("Item Display Text")]
    public  Text                        textDisplay;
    [Space(8)]
    [Tooltip("Initial position of display text.")]
    public  Vector3                     textOffset;

    [Header("Animation Properties")]
    public  Vector3                     openPosition    = new Vector3(0f, 0, 0f);
    [Space(8)]
    public  Vector3                     closedPosition  = new Vector3(0f, -200f, 0f);
    [Space(8)]
    public  float                       tweenSpeed      = 0.15f;

    private PlayerEquipmentComponent    equipment;
    private int[]                       positions;  //slot highlight positions


    /**
    * Initialize the inventory display.
    *
    * @param    null
    * @return   null
    **/
    public override void Initialize()
    {
        equipment = inventory.gameObject.GetComponent<PlayerEquipmentComponent>();
        positions = new int[highlights.Length];

        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition3D = closedPosition;
    }


    /**
    * Update the layout and look of the inventory display tiles.
    *
    * @param    null
    * @return   null
    **/
    public override void UpdateTiles()
    {
        //calculate highlight icon positions
        positions[0] = Mathf.Abs(equipment.selectedSlot.y) * inventory.size.x + Mathf.Abs(equipment.selectedSlot.x);
        positions[1] = Mathf.Abs(equipment.equippedSlot.y) * inventory.size.x + Mathf.Abs(equipment.equippedSlot.x);
        positions[2] = Mathf.Abs(equipment.hotkeyedSlot.y) * inventory.size.x + Mathf.Abs(equipment.hotkeyedSlot.x);

        //enable/disable highlight icons depending on if they are valid positions
        if (equipment.selectedSlot.x < 0 || equipment.selectedSlot.y < 0)
            highlights[0].gameObject.SetActive(false);
        else
            highlights[0].gameObject.SetActive(true);

        if (equipment.equippedSlot.x < 0 || equipment.equippedSlot.y < 0)
            highlights[1].gameObject.SetActive(false);
        else
            highlights[1].gameObject.SetActive(true);

        if (equipment.hotkeyedSlot.x < 0 || equipment.hotkeyedSlot.y < 0)
            highlights[2].gameObject.SetActive(false);
        else
            highlights[2].gameObject.SetActive(true);

        //disable equipped/hotkeyed highlight icons if selected slot overlaps
        if (equipment.selectedSlot == equipment.equippedSlot)
            highlights[1].gameObject.SetActive(false);
        if (equipment.selectedSlot == equipment.hotkeyedSlot)
            highlights[2].gameObject.SetActive(false);

        //set positions of highlight icons
        for (int i = 0; i < highlights.Length; i++)
        {
            RectTransform hr = highlights[i].GetComponent<RectTransform>();
            hr.anchoredPosition3D = inventorySlots[positions[i]].rect.anchoredPosition3D;
        }

        //set item display text position
        Item[] items = inventory.GetInventory();
        Vector3 textPosition = textDisplay.rectTransform.anchoredPosition3D;
        textPosition.y = inventorySlots[items.Length-1].rect.anchoredPosition3D.y;
        textDisplay.rectTransform.anchoredPosition3D = textPosition + textOffset;

        //set display text based on selected item
        Item selected = equipment.displayedItem;
        if (selected != null)
        {
            string count = "";

            //also display ammunition count if item is a weapon
            if (selected is Weapon)
                count = String.Format(" ({0})", (selected as Weapon).CurrentAmmunition());
            else
                count = selected.stackSize > 0 ? String.Format(" ({0})", selected.stackSize) : "";

            textDisplay.text = data.names[(int)selected.itemType] + count;
        }
        else
        {
            textDisplay.text = data.names[(int)data.names.Length-1];
        }
    }


    /**
    * Tween the inventory display to the designated "open" position.
    *
    * @param    null
    * @return   null
    **/
    public override IEnumerator Open()
    {
        while (Vector3.Distance(rect.anchoredPosition3D, openPosition) > Mathf.Epsilon)
        {
            rect.anchoredPosition3D = Vector3.Lerp(rect.anchoredPosition3D, openPosition, tweenSpeed);
            yield return null;
        }
    }


    /**
    * Tween the inventory display to the designated "closed" position.
    *
    * @param    null
    * @return   null
    **/
    public override IEnumerator Close()
    {
        while (Vector3.Distance(rect.anchoredPosition3D, closedPosition) > Mathf.Epsilon)
        {
            rect.anchoredPosition3D = Vector3.Lerp(rect.anchoredPosition3D, closedPosition, tweenSpeed);
            yield return null;
        }
    }
}
