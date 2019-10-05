using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EngineValueSelectionEditorExtensions
{
    static SerializedProperty engineValueManager;

    public static void EngineValueSelectionField(this SerializedProperty _valueSelectionProperty, SerializedProperty _engineValueManager)
    {
        engineValueManager = _engineValueManager;
        EngineValueSelectionField(_valueSelectionProperty, 0);
    }

    public static void EngineValueSelectionArrayField(this SerializedProperty _valueSelectionArrayProperty, SerializedProperty _engineValueManager)
    {
        engineValueManager = _engineValueManager;
        _valueSelectionArrayProperty.ArrayFieldButtons("Engine Value", true, true, true, true, EngineValueSelectionField);
    }

    static void EngineValueSelectionField(SerializedProperty _property, int _ind)
    {
        var selectionName = _property.FindPropertyRelative("selectionName");
        var category = _property.FindPropertyRelative("category");
        var engineValue = _property.FindPropertyRelative("engineValue");
        var valueData = _property.FindPropertyRelative("valueData");
        var id = _property.FindPropertyRelative("id");

        var valueMan = engineValueManager.objectReferenceValue as EngineValueDataManager;
        if (valueMan)
        {
            category.IndexStringField(valueMan.GetCategoryNames());
            var catInd = category.FindPropertyRelative("indexValue");
            engineValue.IndexStringField(valueMan.GetEngineValueDataNames(catInd.intValue));
            var valInd = engineValue.FindPropertyRelative("indexValue");
            var valData = valueMan.engineValueCategories[catInd.intValue].engineValueDatas[valInd.intValue];
            if (valData)
            {
                valueData.objectReferenceValue = valData;

                //create ids in editor
                valueMan.RefreshIDs();

                var catString = category.FindPropertyRelative("stringValue");
                selectionName.stringValue = catString.stringValue + " | " + valueData.objectReferenceValue.name;
            }
                
        }

    }
}
