using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UISlot : MonoBehaviour
{

    public void SetIcon(Sprite _icon)
    {
        Image image = GetComponent<Image>();
        image.sprite = _icon;
    }

    public void SetSelected(bool _selected)
    {
        if (_selected)
            transform.localScale = Vector2.one * 1.1f;
        else
            transform.localScale = Vector2.one;
    }
}
