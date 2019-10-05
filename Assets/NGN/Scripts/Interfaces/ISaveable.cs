using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public interface ISaveable
    {
        string DataPath { get; set; }
        void WriteData();
        void ReadData();
    }
}

