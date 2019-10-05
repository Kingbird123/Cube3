using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Globalization;

namespace NGN
{
    public static class NGNEditorExtensions
    {

        public static bool GetKeyDown(KeyCode _keyCode)
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    return Event.current.keyCode == _keyCode;
            }
            return false;
        }
    }
}


