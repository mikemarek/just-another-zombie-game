/**
**/

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInventoryDisplay : InventoryDisplay
{
    [Header("Inventory Highlight Icons")]
    [Tooltip("0:selected  1:equipped  2:hotkeyed")]
    public  GameObject[]                highlights;

    [Header("Display Text")]
    public  Text                        textDisplay;
    [Tooltip("Initial position of display text.")]
    public  Vector3                     textOffset;

    [Header("Animation Properties")]
    public  Vector3                     openPosition    = new Vector3(0f, 0, 0f);
    public  Vector3                     closedPosition  = new Vector3(0f, -200f, 0f);
    public  float                       tweenSpeed      = 0.15f;

    private PlayerEquipmentComponent    equipment;

    /**
    **/
    public override void Initialize()
    {
        equipment = inventory.gameObject.GetComponent<PlayerEquipmentComponent>();

        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition3D = closedPosition;
    }

    /**
    **/
    public override void UpdateTiles()
    {
        //calculate highlight positions
        int[] positions = new int[3]
        {
            Mathf.Abs(equipment.selectedSlot.y) * inventory.size.x + Mathf.Abs(equipment.selectedSlot.x),
            Mathf.Abs(equipment.equippedSlot.y) * inventory.size.x + Mathf.Abs(equipment.equippedSlot.x),
            Mathf.Abs(equipment.hotkeyedSlot.y) * inventory.size.x + Mathf.Abs(equipment.hotkeyedSlot.x)
        };

        //enable/disable highlights depending on if they are valid positions
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

        //disable equipped/hotkeyed highlights if slot is selected
        //if (equipment.selectedSlot == equipment.equippedSlot)
        //    highlights[1].gameObject.SetActive(false);
        //if (equipment.selectedSlot == equipment.hotkeyedSlot)
        //    highlights[2].gameObject.SetActive(false);

        //set positions of highlights
        for (int i = 0; i < highlights.Length; i++)
        {
            RectTransform hr = highlights[i].GetComponent<RectTransform>();
            hr.anchoredPosition3D = inventorySlots[positions[i]].rect.anchoredPosition3D;
        }

        //set display text position
        Item[] items = inventory.GetInventory();
        Vector3 textPosition = textDisplay.rectTransform.anchoredPosition3D;
        textPosition.y = inventorySlots[items.Length-1].rect.anchoredPosition3D.y;
        textDisplay.rectTransform.anchoredPosition3D = textPosition + textOffset;

        //set display text based on selected item
        Item selected = equipment.displayedItem; //inventory.GetItem(equipment.selectedSlot);
        if (selected != null)
        {
            string count = "";
            if (selected is Weapon)
                count = String.Format(" ({0})", (selected as Weapon).CurrentAmmunition());
            else
                count = selected.stackSize > 0 ? String.Format(" ({0})", selected.stackSize) : "";
            textDisplay.text = im.names[(int)selected.itemType] + count;
        }
        else
            textDisplay.text = im.names[(int)im.names.Length-1];
    }

    /**
    **/
    public override IEnumerator Open()
    {
        while (Vector3.Distance(rect.anchoredPosition3D, openPosition) > Mathf.Epsilon)
        {
            //rect.anchoredPosition3D = Vector3.Lerp(rect.anchoredPosition3D, startingPoint + openPosition, tweenSpeed);
            rect.anchoredPosition3D = Vector3.Lerp(rect.anchoredPosition3D, openPosition, tweenSpeed);
            yield return null;
        }
    }

    /**
    **/
    public override IEnumerator Close()
    {
        while (Vector3.Distance(rect.anchoredPosition3D, closedPosition) > Mathf.Epsilon)
        {
            //rect.anchoredPosition3D = Vector3.Lerp(rect.anchoredPosition3D, startingPoint + closedPosition, tweenSpeed);
            rect.anchoredPosition3D = Vector3.Lerp(rect.anchoredPosition3D, closedPosition, tweenSpeed);
            yield return null;
        }
    }
}
