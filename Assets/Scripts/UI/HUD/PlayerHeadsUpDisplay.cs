/**
* PlayerHeadsUpDisplay.cs
* Created by Michael Marek (2016)
*
* Manages the layout, position, and display of a player's heads-up-display (HUD). This class links
* the Unity UI graphic to the player's various attached components and displays vital information,
* such as player health and current equipped items.
**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHeadsUpDisplay : MonoBehaviour
{
    [Header("Displayed Equipment, Inventory, Health, and Progress Components")]
    public  PlayerEquipmentComponent    equipment;
    public  InventoryComponent          inventory;
    public  HealthComponent             health;
    public  ProgressComponent           progress;

    [Header("Item Text Display")]
    public  Text                        textDisplay;

    [Header("Weapons Slot Display")]
    public  GameObject                  weaponWidget;

    [Header("Health Bar Display")]
    public  RectTransform               healthBar;
    public  Image                       healthBarImage;

    [Header("Progress Bar Display")]
    public  RectTransform               progressBar;
    public  Image                       progressBarImage;

    [Header("Animation Properties")]
    public  Vector3                     openPosition    = new Vector3(0f, 0f, 0f);
    [Space(8)]
    public  Vector3                     closedPosition  = new Vector3(0f, -200f, 0f);
    [Space(8)]
    public  float                       tweenSpeed      = 0.15f;

    private bool                        opened;
    private UIDisplayIcon               healthDisplay;
    private UIDisplayIcon               weaponDisplay;
    private RectTransform               rect;

    private DataManager                 data;


    /**
    * Initialize the HUD.
    *
    * @param    null
    * @return   null
    **/
    void Start()
    {
        opened = true;

        data = GameObject.Find("Data Manager").GetComponent<DataManager>();

        rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = openPosition;

        weaponDisplay = new UIDisplayIcon();
        weaponDisplay.GrabDisplay(weaponWidget);
    }


    /**
    * Update the HUD.
    *
    * @param    null
    * @return   null
    **/
    void Update()
    {
        //open or close the HUD depending if the player is managing their inventory
        if (opened && inventory.beingManaged)
        {
            StopAllCoroutines();
            StartCoroutine(Close());
        }
        else if (!opened && !inventory.beingManaged)
        {
            StopAllCoroutines();
            StartCoroutine(Open());
        }

        opened = !inventory.beingManaged;

        if (!opened)
            return;

        //scale player health bar
        float scale = health.health / health.maxHealth;
        if (healthBar.localScale.x != scale)
            healthBar.localScale = new Vector3(scale, healthBar.localScale.y, healthBar.localScale.z);

        //scale player progress meter and colour accordingly
        if (progressBar.localScale.x != progress.progress)
            progressBar.localScale = new Vector3(progress.progress, progressBar.localScale.y, progressBar.localScale.z);
        if (progressBarImage.color != progress.colour)
            progressBarImage.color = progress.colour;

        //display the player's currently equipped item
        Item item = equipment.equipped;
        if (item != null)
        {
            //if holding a weapon, also display ammunition count
            if (item.GetType().IsSubclassOf(typeof(Weapon)))
            {
                textDisplay.text = data.names[(int)item.itemType];
                weaponDisplay.icon.sprite = data.icons[(int)item.itemType];
                weaponDisplay.text.text = (item as Weapon).CurrentAmmunition();
            }
            //if holding a regular item, display the item's stack size instead
            else
            {
                textDisplay.text = data.names[(int)item.itemType];
                weaponDisplay.icon.sprite = data.icons[(int)item.itemType];

                string count = item.stackSize > 0 ? System.String.Format("{0}", item.stackSize) : "";
                weaponDisplay.text.text = count;
            }
        }
        else
        {
            textDisplay.text = "";
            weaponDisplay.icon.sprite = data.icons[data.icons.Length-1];
            weaponDisplay.text.text = "";
        }
    }


    /**
    * Tween the HUD to the designated "open" position.
    *
    * @param    null
    * @return   null
    **/
    public IEnumerator Open()
    {
        while (Vector3.Distance(rect.anchoredPosition3D, openPosition) > Mathf.Epsilon)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, openPosition, tweenSpeed);
            yield return null;
        }
    }


    /**
    * Tween the HUD to the designated "closed" position.
    *
    * @param    null
    * @return   null
    **/
    public IEnumerator Close()
    {
        while (Vector3.Distance(rect.anchoredPosition3D, closedPosition) > Mathf.Epsilon)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, closedPosition, tweenSpeed);
            yield return null;
        }
    }
}
