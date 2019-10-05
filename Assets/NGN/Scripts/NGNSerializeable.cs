using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGN
{
    public abstract class NGNSerializeable : ISaveable
    {
        public string DataPath { get; set; }
        protected const string dataPath = "";
        public virtual void ReadData()
        {
            
        }

        public virtual void WriteData()
        {
            
        }
    }
}


