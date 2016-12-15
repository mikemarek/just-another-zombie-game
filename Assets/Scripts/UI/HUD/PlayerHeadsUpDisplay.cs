/**
**/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHeadsUpDisplay : MonoBehaviour
{
    [Header("Displayed Equipment, Inventory, Health, Progress")]
    public  PlayerEquipmentComponent    equipment;
    public  InventoryComponent          inventory;
    public  HealthComponent             health;
    public  ProgressComponent           progress;

    [Header("Item Text Display")]
    public  Text                        textDisplay;

    [Header("Weapons Slot Display")]
    public  GameObject                  weaponWidget;

    [Header("Progress Bar Display")]
    public  RectTransform               healthBar;
    public  Image                       healthBarImage;

    [Header("Progress Bar Display")]
    public  RectTransform               progressBar;
    public  Image                       progressBarImage;

    [Header("Animation Properties")]
    public  Vector2                     openPosition    = new Vector2(0f, 0f);
    public  Vector2                     closedPosition  = new Vector2(0f, -200f);
    public  float                       tweenSpeed      = 0.15f;

    private bool                        opened;
    //private Vector3                     startingPoint;
    private UIDisplayIcon               healthDisplay;
    private UIDisplayIcon               weaponDisplay;
    private RectTransform               rect;

    private DataManager                 data;

    void Start()
    {
        //opened = false;
        opened = true;

        data = GameObject.Find("Data Manager").GetComponent<DataManager>();

        rect = gameObject.GetComponent<RectTransform>();
        //startingPoint = rect.anchoredPosition3D;
        //rect.anchoredPosition3D += openPosition;
        rect.anchoredPosition = openPosition;

        weaponDisplay = new UIDisplayIcon();
        weaponDisplay.GrabDisplay(weaponWidget);
    }

    void Update()
    {
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

        float scale = health.health / health.maxHealth;
        if (healthBar.localScale.x != scale)
            healthBar.localScale = new Vector3(scale, healthBar.localScale.y, healthBar.localScale.z);

        if (progressBar.localScale.x != progress.progress)
            progressBar.localScale = new Vector3(progress.progress, progressBar.localScale.y, progressBar.localScale.z);
        if (progressBarImage.color != progress.colour)
            progressBarImage.color = progress.colour;

        Item item = equipment.equipped;
        if (item != null)
        {
            if (item.GetType().IsSubclassOf(typeof(Weapon)))
            {
                textDisplay.text = data.names[(int)item.itemType];
                weaponDisplay.icon.sprite = data.icons[(int)item.itemType];
                weaponDisplay.text.text = (item as Weapon).CurrentAmmunition();
            }
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
    **/
    public IEnumerator Open()
    {
        while (Vector3.Distance(rect.anchoredPosition3D, openPosition) > Mathf.Epsilon)
        {
            //rect.anchoredPosition3D = Vector3.Lerp(rect.anchoredPosition3D, startingPoint + openPosition, tweenSpeed);
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, openPosition, tweenSpeed);
            yield return null;
        }
    }

    /**
    **/
    public IEnumerator Close()
    {
        while (Vector3.Distance(rect.anchoredPosition3D, closedPosition) > Mathf.Epsilon)
        {
            //rect.anchoredPosition3D = Vector3.Lerp(rect.anchoredPosition3D, startingPoint + closedPosition, tweenSpeed);
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, closedPosition, tweenSpeed);
            yield return null;
        }
    }
}
