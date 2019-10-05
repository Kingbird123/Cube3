using System.Collections;
using System.Collections.Generic;
using System.IO;
using NGN.OdinSerializer;
using UnityEditor;
using UnityEngine;

public class InspectorHistoryData : ScriptableObject
{
    [System.Serializable]
    public class InspectorHistoryContainer
    {
        public int curObject;
        public List<int> prevObjects = new List<int>();
        public List<int> nextObjects = new List<int>();
        public bool goingBack;
        public bool goingForward;
        public int prevInd;
        public int nextInd;
        public int cache = 5;
        public float height;
        public bool enableFavourites = true;
        public int maxFavouritesPerRow = 4;
        public float favouritesWidth = 100;
        public List<string> favourites = new List<string>();
    }

    public InspectorHistoryContainer content = new InspectorHistoryContainer();
    private const string dataPath = "Assets/InspectorHistory/Resources/InspectorHistorySave";

    public void InitializeData()
    {
        GetDataFromFile();
    }

    void GetDataFromFile()
    {
        if (!File.Exists(dataPath))
            WriteDataToFile();
        else
            ReadDataFromFile();
    }

    public void WriteDataToFile()
    {
        byte[] bytes = SerializationUtility.SerializeValue(content, DataFormat.Binary);
        File.WriteAllBytes(dataPath, bytes);
    }

    void ReadDataFromFile()
    {
        //get data from json
        byte[] bytes = File.ReadAllBytes(dataPath);
        content = SerializationUtility.DeserializeValue<InspectorHistoryContainer>(bytes, DataFormat.Binary);
    }

}
