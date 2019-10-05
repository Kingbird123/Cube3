using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class InspectorHistoryEditor : EditorWindow
{

    #region PREFERENCES_WINDOW

    public class InspectorHistoryPropertiesEditor : EditorWindow
    {
        private InspectorHistoryData data;
        private InspectorHistoryData.InspectorHistoryContainer content;

        [MenuItem("Window/Inspector History/Preferences")]
        public static void ShowWindow()
        {
            GetWindow<InspectorHistoryPropertiesEditor>("History Preferences");
        }

        private void OnEnable()
        {
            if (!data)
            {
                data = GetData();
                content = data.content;
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("History Preferences");
            EditorGUILayout.LabelField("-----------------------");
            content.enableFavourites = EditorGUILayout.Toggle("Enable Favourites", content.enableFavourites);
            if (content.enableFavourites)
            {
                content.maxFavouritesPerRow = EditorGUILayout.IntField("Max Favourites Per Row", content.maxFavouritesPerRow);
                if (content.maxFavouritesPerRow < 1)
                    content.maxFavouritesPerRow = 1;
                content.favouritesWidth = EditorGUILayout.FloatField("Favourites Width", content.favouritesWidth);
                if (content.favouritesWidth < 5)
                    content.favouritesWidth = 5;
            }

        }
    }

    #endregion

    private InspectorHistoryData data;
    private InspectorHistoryData.InspectorHistoryContainer content;
    private int curRows;
    private GUISkin skin;
    private GUIStyle buttonStyle;
    private int curSelection;
    
    [MenuItem("Window/Inspector History/History")]
    public static void ShowWindow()
    {
        GetWindow<InspectorHistoryEditor>("History");
    }

    private void OnEnable()
    {
        if (!data)
            data = GetData();
        data.InitializeData();
        content = data.content;

        if (!skin)
            skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/InspectorHistory/Resources/InspectorHistorySkin.guiskin");
        if (buttonStyle == null)
            buttonStyle = skin.GetStyle("button");
    }

    private void OnDisable()
    {
        ClearCache();
    }

    private void OnDestroy()
    {
        ClearCache();
    }

    public static InspectorHistoryData GetData()
    {
        //get data
        var data = AssetDatabase.LoadAssetAtPath<InspectorHistoryData>("Assets/InspectorHistory/Resources/InspectorHistoryData.asset");
        if (data)
            return data;
        else//create if doesnt exist
        {
            var asset = CreateInstance<InspectorHistoryData>();
            AssetDatabase.CreateAsset(asset, "Assets/InspectorHistory/Resources/InspectorHistoryData.asset");
            AssetDatabase.SaveAssets();
            data = asset;
        }
        return data;
    }

    public static GUISkin GetSkin()
    {
        return AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/InspectorHistory/Resources/InspectorHistorySkin.guiskin");
    }

    private void OnGUI()
    {
        CalculateWindowHeight();
        DisplayNavButtons();
        DisplayFavourites();

    }

    void CalculateWindowHeight()
    {
        content.height = 22;
        if (content.enableFavourites)
        {
            for (int i = 0; i < curRows; i++)
            {
                content.height += 24;
            }
        }
        minSize = new Vector2(minSize.x, content.height);
    }

    void DisplayNavButtons()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Cache", GUILayout.Width(40));
        content.cache = EditorGUILayout.IntField(content.cache, GUILayout.Width(30));
        if (GUILayout.Button("Clear", GUILayout.Width(50)))
            ClearCache();
        string prevButton = "◄";
        if (content.prevInd >= content.prevObjects.Count - 1 && !(content.prevObjects.Count > 0 && content.prevInd == 0))
            prevButton = "◄✖";
        if (GUILayout.Button(prevButton))
            Previous();
        string nextButton = "►";
        if (content.nextInd >= content.nextObjects.Count - 1 && !(content.nextObjects.Count > 0 && content.nextInd == 0))
            nextButton = "✖►";
        if (GUILayout.Button(nextButton))
            Next();
        if (content.enableFavourites && Selection.activeObject)
        {
            if (GUILayout.Button("★", GUILayout.Width(50)))
            {
                if (Selection.assetGUIDs.Length > 0)
                    AddFavourite(Selection.assetGUIDs[0]);
                else
                    Debug.Log("Can only favourite project objects at this time due to resets. Sorry!");
            }

        }
        EditorGUILayout.EndHorizontal();
    }

    void DisplayFavourites()
    {
        if (content.enableFavourites && content.favourites.Count > 0)
        {
            //calculate how many row we need based on count and per row
            curRows = (content.favourites.Count + (content.maxFavouritesPerRow - 1)) / content.maxFavouritesPerRow;

            //populate the rows
            for (int i = 0; i < curRows; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int ind = 0; ind < content.favourites.Count; ind++)
                {
                    int min = ((i + 1) * content.maxFavouritesPerRow) - (content.maxFavouritesPerRow + 1);
                    int max = (i + 1) * content.maxFavouritesPerRow;
                    if (i == 0)
                    {
                        min = -1;
                        max = content.maxFavouritesPerRow;
                    }
                    if (ind < max && ind > min)
                    {
                        var obj = GetObject(content.favourites[ind]);
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(content.favouritesWidth + 20));
                        if (GUILayout.Button(obj.name, buttonStyle, GUILayout.Width(content.favouritesWidth)))
                            Selection.activeObject = obj;
                        if (GUILayout.Button("✖", GUILayout.Width(20)))
                            RemoveFavourite(ind);

                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

        }
        else
            curRows = 0;
    }

    Object GetObject(int _id)
    {
        return EditorUtility.InstanceIDToObject(_id);
    }

    Object GetObject(string _guid)
    {
        string path = AssetDatabase.GUIDToAssetPath(_guid);
        if (string.IsNullOrEmpty(path))
            return null;
        var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
        if (obj != null)
            return obj;

        return null;
    }

    void Previous()
    {
        if (content.prevObjects.Count < 1)
        {
            content.prevInd = 0;
            return;
        }

        if (!content.goingBack)
        {
            content.goingBack = true;
            content.goingForward = false;
            content.prevInd = 0;
        }
        else
            content.prevInd++;

        //only add to list if searching isnt maxed out
        if (content.prevInd > content.prevObjects.Count - 1)
            content.prevInd = content.prevObjects.Count - 1;
        else
        {
            //add to next object list

            AddToList(content.nextObjects, content.curObject, true);
            content.nextInd = 0;

            //set current object
            content.curObject = content.prevObjects[content.prevInd];
            Selection.activeObject = GetObject(content.curObject);
        }

    }

    void Next()
    {
        if (content.nextObjects.Count < 1)
        {
            content.nextInd = 0;
            return;
        }

        if (!content.goingForward)
        {
            content.goingForward = true;
            content.goingBack = false;
            content.nextInd = 0;
        }
        else
            content.nextInd++;

        //only add to list if searching isnt maxed out
        if (content.nextInd > content.nextObjects.Count - 1)
            content.nextInd = content.nextObjects.Count - 1;
        else
        {
            //add to previous list
            AddToList(content.prevObjects, content.curObject, true);
            content.prevInd = 0;
            content.curObject = content.nextObjects[content.nextInd];
            Selection.activeObject = GetObject(content.curObject);
        }

    }

    private void OnSelectionChange()
    {
        Repaint();

        if (Selection.activeObject)
            curSelection = Selection.activeInstanceID;
        else
            return;

        if (content.goingBack)
        {
            if (content.prevObjects.Count > 0)
            {
                if (curSelection == content.prevObjects[content.prevInd])
                    return;
                else
                {
                    content.prevInd = 0;
                    content.nextObjects.Clear();
                    content.goingBack = false;
                }
            }

        }
        if (content.goingForward)
        {
            if (content.nextObjects.Count > 0)
            {
                if (Selection.activeObject == GetObject(content.nextObjects[content.nextInd]))
                    return;
                else
                {
                    content.nextInd = 0;
                    content.nextObjects.Clear();
                    content.goingForward = false;
                }
            }

        }

        AddToList(content.prevObjects, content.curObject, true);
        content.curObject = curSelection;
    }

    void RemoveFavourite(int _ind)
    {
        content.favourites.RemoveAt(_ind);
        data.WriteDataToFile();
    }

    void AddFavourite(string _guid)
    {
        if (Selection.activeObject)
        {
            if (!content.favourites.Contains(_guid))
            {
                content.favourites.Add(_guid);
            }
        }
        data.WriteDataToFile();
        CalculateWindowHeight();
    }

    void AddToList(List<int> _list, int _object, bool _onlyAddNew = false)
    {
        if (_onlyAddNew && _list.Contains(_object))
            return;

        if (_list.Count < 1)
            _list.Add(_object);
        else
            _list.Insert(0, _object);

        if (_list.Count > content.cache)
            _list.RemoveRange(content.cache - 1, _list.Count - content.cache);
    }

    void ClearCache()
    {
        content.prevObjects.Clear();
        content.nextObjects.Clear();
        content.prevInd = 0;
        content.nextInd = 0;
        content.goingBack = false;
        content.goingForward = false;

        data.WriteDataToFile();
    }

}
