/**
* ContainerInventoryDisplay.cs
* Created by Michael Marek (2016)
*
* Manages the layout, position, and display of an item container's inventory system. Link the Unity
* UI graphic to the item container's attached inventory component and displays the contents.
*
* For each player that is currently managing the container inventory, a "highlight" tile is added
* with the same colour denoted in that player's health component, which is used to denote the
* current slot that the player has selected.
**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ContainerInventoryDisplay : InventoryDisplay
{
    [Header("Inventory Highlight Icons")]
    public  GameObject[]    highlights;
    [Space(10)]
    public  Transform       location;
    [Space(10)]
    public  float           fadeSpeed;

    private Container       container;


    /**
    * Initialize the inventory display.
    *
    * @param    null
    * @return   null
    **/
    public override void Initialize()
    {
        container = inventory.gameObject.GetComponent<Container>();

        //fade all text and images out
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        Text[] texts = gameObject.GetComponentsInChildren<Text>();
        foreach (Image image in images)
        {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
        }
        foreach (Text text in texts)
        {
            Color color = text.color;
            color.a = 0f;
            text.color = color;
        }
    }


    /**
    * Update the layout and look of the inventory display tiles.
    *
    * @param    null
    * @return   null
    **/
    public override void UpdateTiles()
    {
        //enable/disable highlight tiles depending on number of players
        for (int i = 0; i < highlights.Length; i++)
            if (i < container.players.Count)
                highlights[i].SetActive(true);
            else
                highlights[i].SetActive(false);

        //position highlight tiles
        int playerIndex = 0;
        foreach (KeyValuePair<GameObject, Position> player in container.players)
        {
            if (!player.Value.Valid())
            {
                highlights[playerIndex].SetActive(false);
                continue;
            }

            highlights[playerIndex].SetActive(true);

            int position = Mathf.Abs(player.Value.y) * inventory.size.x + Mathf.Abs(player.Value.x);
            RectTransform hr = highlights[playerIndex].GetComponent<RectTransform>();
            hr.anchoredPosition3D = inventorySlots[position].rect.anchoredPosition3D;

            //colour player highlight tile
            HealthComponent health = player.Key.GetComponent<HealthComponent>();
            Image image = highlights[playerIndex].GetComponent<Image>();
            image.color = health.colour;

            playerIndex++;
        }

        transform.position = location.position;
    }


    /**
    * Fade in the inventory display when opened.
    *
    * @param    null
    * @return   null
    **/
    public override IEnumerator Open()
    {
        bool faded = true;

        Image[] images = gameObject.GetComponentsInChildren<Image>();
        Text[] texts = gameObject.GetComponentsInChildren<Text>();

        do
        {
            //fade in images
            foreach (Image image in images)
            {
                Color color = image.color;
                if (color.a < 1f)
                    faded = false;
                if (color.a < 1f)
                    color.a += (1f / fadeSpeed) * Time.deltaTime;
                else
                    color.a = 1f;
                image.color = color;
            }

            //fade in text
            foreach (Text text in texts)
            {
                Color color = text.color;
                if (color.a < 1f)
                    faded = false;
                if (color.a < 1f)
                    color.a += (1f / fadeSpeed) * Time.deltaTime;
                else
                    color.a = 1f;
                text.color = color;
            }

            yield return null;

        } while (!faded);
    }


    /**
    * Fade out the inventory display when closed.
    *
    * @param    null
    * @return   null
    **/
    public override IEnumerator Close()
    {
        bool faded = true;

        Image[] images = gameObject.GetComponentsInChildren<Image>();
        Text[] texts = gameObject.GetComponentsInChildren<Text>();

        do
        {
            //fade out images
            foreach (Image image in images)
            {
                Color color = image.color;
                if (color.a > 0f)
                    faded = false;
                if (color.a > 0f)
                    color.a -= (1f / fadeSpeed) * Time.deltaTime;
                else
                    color.a = 0f;
                image.color = color;
            }

            //fade out text
            foreach (Text text in texts)
            {
                Color color = text.color;
                if (color.a > 0f)
                    faded = false;
                if (color.a > 0f)
                    color.a -= (1f / fadeSpeed) * Time.deltaTime;
                else
                    color.a = 0f;
                text.color = color;
            }

            yield return null;

        } while (!faded);
    }
}
