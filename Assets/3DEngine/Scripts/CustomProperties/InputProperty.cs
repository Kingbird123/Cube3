using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputProperty
{
    public string stringValue;
    public int indexValue;

    private bool inputDown { get { return Input.GetButtonDown(stringValue) || Input.GetAxisRaw(stringValue) != 0; } }
    private bool lastDownCheck = false;
    private bool lastUpCheck = true;

    public float GetAxis(bool _raw)
    {
        if (_raw)
            return Input.GetAxisRaw(stringValue);
        else
            return Input.GetAxis(stringValue);
    }

    public bool GetInput()
    {
        return inputDown;
    }

    public bool GetInputDown()
    {
        if (inputDown)
        {
            if (inputDown != lastDownCheck)
            {
                lastDownCheck = inputDown;
                return true;
            }

        }
        lastDownCheck = inputDown;
        return false;
    }

    public bool GetInputUp()
    {
        if (!inputDown)
        {
            if (!inputDown != lastUpCheck)
            {
                lastUpCheck = !inputDown;
                return true;
            }

        }
        lastUpCheck = !inputDown;
        return false;
    }
}