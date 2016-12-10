/**
**/

using UnityEngine;
using UnityEngine.UI;

public class UIDisplayIcon
{
    public RectTransform    rect;
    public Image            background;
    public Image            icon;
    public Text             text;

    /**
    **/
    public void GrabDisplay(GameObject widget)
    {
        rect = widget.GetComponent<RectTransform>();
        background = widget.transform.Find("Background").GetComponent<Image>();
        icon = widget.transform.Find("Icon").GetComponent<Image>();
        text = widget.transform.Find("Text").GetComponent<Text>();
    }
}
