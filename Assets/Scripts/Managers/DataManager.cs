/**
**/

using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour
{
    private string[] itemNamesList;
    private string[] itemIconsList;
    //private string[] itemPickupsList;

    /**
    **/
    public void Initialize()
    {
        TextAsset ta;

        //item display names
        ta = Resources.Load<TextAsset>("Data/Item Names") as TextAsset;
        itemNamesList = ta.text.Split(new string[]{"\r\n", "\n"}, System.StringSplitOptions.RemoveEmptyEntries);

        //item icon sprite resource location
        ta = Resources.Load<TextAsset>("Data/Item Icons") as TextAsset;
        itemIconsList = ta.text.Split(new string[]{"\r\n", "\n"}, System.StringSplitOptions.RemoveEmptyEntries);

        //associated prefabs for item pickups
        ta = Resources.Load<TextAsset>("Data/Item Pickups") as TextAsset;
        itemIconsList = ta.text.Split(new string[]{"\r\n", "\n"}, System.StringSplitOptions.RemoveEmptyEntries);
    }

    /**
    **/
    public string[] itemNames   { get { return itemNamesList;   } }
    public string[] itemIcons   { get { return itemIconsList;   } }
    //public Bullet[] itemPickups { get { return itemPickupsList; } }
}
