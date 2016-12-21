/**
* UIDisplayIcon.cs
* Created by Michael Marek (2016)
*
* A container class used to store the various components of a UI item display tile.
*
* These components are referenced by name, and should follow the convension:
* "Background"  - child which holds an Image component; used to display the tile background
* "Icon"        - child which holds an Image component; used to display the item icon
* "Text"        - child which holds a Text component; used to display the item count
**/

using UnityEngine;
using UnityEngine.UI;

public class UIDisplayIcon
{
    public RectTransform    rect;       //parent position of the tile
    public Image            background; //background sprite image
    public Image            icon;       //item icon sprite image
    public Text             text;       //item count display text


    /**
    * Retrieve the various component references needed from a GameObject.
    *
    * @param    GameObject  parent container holding the various display tile components
    * @return   null
    **/
    public void GrabDisplay(GameObject widget)
    {
        rect = widget.GetComponent<RectTransform>();
        background = widget.transform.Find("Background").GetComponent<Image>();
        icon = widget.transform.Find("Icon").GetComponent<Image>();
        text = widget.transform.Find("Text").GetComponent<Text>();
    }
}
