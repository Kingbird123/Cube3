using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UILayoutMaster", menuName = "Data/Managers/UILayoutMaster", order = 1)]
public class UILayoutMaster : ScriptableObject
{
    public UILayout[] layouts;
}
