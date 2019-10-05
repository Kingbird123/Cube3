using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionUI : MonoBehaviour 
{

    [SerializeField] private Text charName = null;

	public void SetCharName(string _name)
    {
        charName.text = _name;
    }
}
