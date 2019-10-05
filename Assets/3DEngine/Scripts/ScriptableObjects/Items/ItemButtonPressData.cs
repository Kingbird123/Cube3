using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemButtonPress", menuName = "Data/Items/Tools/ItemButtonPress", order = 1)]
public class ItemButtonPressData : ItemFiniteData
{
    public bool setButton;
    public InputProperty button;
}
