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

            HealthComponent health = player.Key.GetComponent<HealthComponent>();
            Image image = highlights[playerIndex].GetComponent<Image>();
            image.color = health.colour;

            playerIndex++;
        }

        transform.position = location.position;
    }

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
