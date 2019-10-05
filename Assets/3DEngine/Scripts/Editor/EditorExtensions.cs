using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Animations;
using System.Linq;
using UnityEditorInternal;
using System.Reflection;
using AnimatorController = UnityEditor.Animations.AnimatorController;
using NGN.OdinSerializer;

public static class EditorExtensions
{

    public static void ParameterField(this ParameterInfo _param, SerializedProperty _paramValueProperty, string _name)
    {
        var type = _param.ParameterType;
        var param = _paramValueProperty.GetRootValue<ParameterValue>();
        if (param == null) return;
        if (type == typeof(AnimationClip))
        {
            var animationClipValue = _paramValueProperty.FindPropertyRelative("animationClipValue");
            animationClipValue.objectReferenceValue = EditorGUILayout.ObjectField(_name, animationClipValue.objectReferenceValue, typeof(AnimationClip), true);
            param.serializedData = SerializationUtility.SerializeValue(animationClipValue.objectReferenceValue, DataFormat.Binary);
        }
        else if (type == typeof(AnimationCurve))
        {
            var animationCurveValue = _paramValueProperty.FindPropertyRelative("animationCurveValue");
            animationCurveValue.animationCurveValue = EditorGUILayout.CurveField(_name, animationCurveValue.animationCurveValue);
            param.serializedData = SerializationUtility.SerializeValue(animationCurveValue.animationCurveValue, DataFormat.Binary);
        }
        else if (type == typeof(bool))
        {
            var boolValue = _paramValueProperty.FindPropertyRelative("boolValue");
            boolValue.boolValue = EditorGUILayout.Toggle(_name, boolValue.boolValue);
            param.serializedData = SerializationUtility.SerializeValue(boolValue.boolValue, DataFormat.Binary);
        }
        else if (type == typeof(Bounds))
        {
            var boundsValue = _paramValueProperty.FindPropertyRelative("boundsValue");
            boundsValue.boundsValue = EditorGUILayout.BoundsField(_name, boundsValue.boundsValue);
            param.serializedData = SerializationUtility.SerializeValue(boundsValue.boundsValue, DataFormat.Binary);
        }
        else if (type == typeof(Color))
        {
            var colorValue = _paramValueProperty.FindPropertyRelative("colorValue");
            colorValue.colorValue = EditorGUILayout.ColorField(_name, colorValue.colorValue);
            param.serializedData = SerializationUtility.SerializeValue(colorValue.colorValue, DataFormat.Binary);
        }
        else if (type.IsEnum)
        {
            var intValue = _paramValueProperty.FindPropertyRelative("intValue");
            intValue.intValue = EditorGUILayout.Popup(_name, intValue.intValue, System.Enum.GetNames(type));
            param.serializedData = SerializationUtility.SerializeValue(intValue.intValue, DataFormat.Binary);
        }
        else if (type == typeof(float))
        {
            var floatValue = _paramValueProperty.FindPropertyRelative("floatValue");
            floatValue.floatValue = EditorGUILayout.FloatField(_name, floatValue.floatValue);
            param.serializedData = SerializationUtility.SerializeValue(floatValue.floatValue, DataFormat.Binary);

        }
        else if (type == typeof(int))
        {
            var intValue = _paramValueProperty.FindPropertyRelative("intValue");
            intValue.intValue = EditorGUILayout.IntField(_name, intValue.intValue);
            param.serializedData = SerializationUtility.SerializeValue(intValue.intValue, DataFormat.Binary);
        }
        else if (type == typeof(LayerMask))
        {
            var layerMaskValue = _paramValueProperty.FindPropertyRelative("intValue");
            int maskInt = EditorGUILayout.MaskField(_name, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMaskValue.intValue), InternalEditorUtility.layers);
            layerMaskValue.intValue = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(maskInt);
            param.serializedData = SerializationUtility.SerializeValue(layerMaskValue.intValue, DataFormat.Binary);
        }
        else if (type == typeof(Quaternion))
        {
            var vector4Value = _paramValueProperty.FindPropertyRelative("vector4Value");
            var quaternionValue = _paramValueProperty.FindPropertyRelative("quaternionValue");
            vector4Value.vector4Value = EditorGUILayout.Vector4Field(_name, vector4Value.vector4Value);
            quaternionValue.quaternionValue = new Quaternion(vector4Value.vector4Value.x, vector4Value.vector4Value.y, vector4Value.vector4Value.z, vector4Value.vector4Value.w);
            param.serializedData = SerializationUtility.SerializeValue(quaternionValue.quaternionValue, DataFormat.Binary);
        }
        else if (type == typeof(Rect))
        {
            var rectValue = _paramValueProperty.FindPropertyRelative("rectValue");
            rectValue.rectValue = EditorGUILayout.RectField(_name, rectValue.rectValue);
            param.serializedData = SerializationUtility.SerializeValue(rectValue.rectValue, DataFormat.Binary);
        }
        else if (type == typeof(string))
        {
            var stringValue = _paramValueProperty.FindPropertyRelative("stringValue");
            stringValue.stringValue = EditorGUILayout.TextField(_name, stringValue.stringValue);
            param.serializedData = SerializationUtility.SerializeValue(stringValue.stringValue, DataFormat.Binary);
        }
        else if (type == typeof(Vector2))
        {
            var vector2Value = _paramValueProperty.FindPropertyRelative("vector2Value");
            vector2Value.vector2Value = EditorGUILayout.Vector2Field(_name, vector2Value.vector2Value);
            param.serializedData = SerializationUtility.SerializeValue(vector2Value.vector2Value, DataFormat.Binary);
        }
        else if (type == typeof(Vector3))
        {
            var vector3Value = _paramValueProperty.FindPropertyRelative("vector3Value");
            vector3Value.vector3Value = EditorGUILayout.Vector3Field(_name, vector3Value.vector3Value);
            param.serializedData = SerializationUtility.SerializeValue(vector3Value.vector3Value, DataFormat.Binary);
        }
        else if (type == typeof(Vector4))
        {
            var vector4Value = _paramValueProperty.FindPropertyRelative("vector4Value");
            vector4Value.vector4Value = EditorGUILayout.Vector4Field(_name, vector4Value.vector4Value);
            param.serializedData = SerializationUtility.SerializeValue(vector4Value.vector4Value, DataFormat.Binary);
        }
        else
        {
            var objectValue = _paramValueProperty.FindPropertyRelative("objectValue");
            objectValue.objectReferenceValue = EditorGUILayout.ObjectField(_name, objectValue.objectReferenceValue, type, true);
            param.serializedData = SerializationUtility.SerializeValue(objectValue.objectReferenceValue, DataFormat.Binary);
        }

    }

    public static void ParameterField(this ParameterInfo _param, SerializedProperty _paramValueProperty, Rect _position, string _name)
    {
        var type = _param.ParameterType;
        var param = _paramValueProperty.GetRootValue<ParameterValue>();

        if (type == typeof(bool))
        {
            var boolValue = _paramValueProperty.FindPropertyRelative("boolValue");
            boolValue.boolValue = EditorGUI.Toggle(_position, _name, boolValue.boolValue);
            param.serializedData = SerializationUtility.SerializeValue(boolValue.boolValue, DataFormat.Binary);
        }
        else if (type == typeof(AnimationCurve))
        {
            var animationCurveValue = _paramValueProperty.FindPropertyRelative("animationCurveValue");
            animationCurveValue.animationCurveValue = EditorGUI.CurveField(_position, _name, animationCurveValue.animationCurveValue);
            param.serializedData = SerializationUtility.SerializeValue(animationCurveValue.animationCurveValue, DataFormat.Binary);
        }
        else if (type == typeof(Color))
        {
            var colorValue = _paramValueProperty.FindPropertyRelative("colorValue");
            colorValue.colorValue = EditorGUI.ColorField(_position, _name, colorValue.colorValue);
            param.serializedData = SerializationUtility.SerializeValue(colorValue.colorValue, DataFormat.Binary);
        }
        else if (type == typeof(int))
        {
            var intValue = _paramValueProperty.FindPropertyRelative("intValue");
            intValue.intValue = EditorGUI.IntField(_position, _name, intValue.intValue);
            param.serializedData = SerializationUtility.SerializeValue(intValue.intValue, DataFormat.Binary);
        }
        else if (type == typeof(float))
        {
            var floatValue = _paramValueProperty.FindPropertyRelative("floatValue");
            floatValue.floatValue = EditorGUI.FloatField(_position, _name, floatValue.floatValue);
            param.serializedData = SerializationUtility.SerializeValue(floatValue.floatValue, DataFormat.Binary);

        }
        else if (type == typeof(Vector2))
        {
            var vector2Value = _paramValueProperty.FindPropertyRelative("vector2Value");
            vector2Value.vector2Value = EditorGUI.Vector2Field(_position, _name, vector2Value.vector2Value);
            param.serializedData = SerializationUtility.SerializeValue(vector2Value.vector2Value, DataFormat.Binary);
        }
        else if (type == typeof(Vector3))
        {
            var vector3Value = _paramValueProperty.FindPropertyRelative("vector3Value");
            vector3Value.vector3Value = EditorGUI.Vector3Field(_position, _name, vector3Value.vector3Value);
            param.serializedData = SerializationUtility.SerializeValue(vector3Value.vector3Value, DataFormat.Binary);
        }
        else if (type == typeof(Vector4))
        {
            var vector4Value = _paramValueProperty.FindPropertyRelative("vector4Value");
            vector4Value.vector4Value = EditorGUI.Vector4Field(_position, _name, vector4Value.vector4Value);
            param.serializedData = SerializationUtility.SerializeValue(vector4Value.vector4Value, DataFormat.Binary);
        }
        else if (type == typeof(Quaternion))
        {
            var vector4Value = _paramValueProperty.FindPropertyRelative("vector4Value");
            var quaternionValue = _paramValueProperty.FindPropertyRelative("quaternionValue");
            vector4Value.vector4Value = EditorGUI.Vector4Field(_position, _name, vector4Value.vector4Value);
            quaternionValue.quaternionValue = new Quaternion(vector4Value.vector4Value.x, vector4Value.vector4Value.y, vector4Value.vector4Value.z, vector4Value.vector4Value.w);
            param.serializedData = SerializationUtility.SerializeValue(quaternionValue.quaternionValue, DataFormat.Binary);
        }
        else if (type == typeof(Rect))
        {
            var rectValue = _paramValueProperty.FindPropertyRelative("rectValue");
            rectValue.rectValue = EditorGUI.RectField(_position, _name, rectValue.rectValue);
            param.serializedData = SerializationUtility.SerializeValue(rectValue.rectValue, DataFormat.Binary);
        }
        else if (type == typeof(string))
        {
            var stringValue = _paramValueProperty.FindPropertyRelative("stringValue");
            stringValue.stringValue = EditorGUI.TextField(_position, _name, stringValue.stringValue);
            param.serializedData = SerializationUtility.SerializeValue(stringValue.stringValue, DataFormat.Binary);
        }
        else if (type == typeof(LayerMask))
        {
            var layerMaskValue = _paramValueProperty.FindPropertyRelative("layerMaskValue");
            int maskInt = EditorGUI.MaskField(_position, _name, InternalEditorUtility.LayerMaskToConcatenatedLayersMask(layerMaskValue.intValue), InternalEditorUtility.layers);
            layerMaskValue.intValue = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(maskInt);
            param.serializedData = SerializationUtility.SerializeValue(layerMaskValue.intValue, DataFormat.Binary);
        }
        else if (type == typeof(Bounds))
        {
            var boundsValue = _paramValueProperty.FindPropertyRelative("boundsValue");
            boundsValue.boundsValue = EditorGUI.BoundsField(_position, _name, boundsValue.boundsValue);
            param.serializedData = SerializationUtility.SerializeValue(boundsValue.boundsValue, DataFormat.Binary);
        }
        else if (type.IsEnum)
        {
            var boundsValue = _paramValueProperty.FindPropertyRelative("boundsValue");
            boundsValue.boundsValue = EditorGUI.BoundsField(_position, _name, boundsValue.boundsValue);
            param.serializedData = SerializationUtility.SerializeValue(boundsValue.boundsValue, DataFormat.Binary);
        }
        else
        {
            var objectValue = _paramValueProperty.FindPropertyRelative("objectValue");
            objectValue.objectReferenceValue = EditorGUI.ObjectField(_position, _name, objectValue.objectReferenceValue, type, true);
            param.serializedData = SerializationUtility.SerializeValue(objectValue.objectReferenceValue, DataFormat.Binary);
        }

    }

    public static ReorderableList ReorderableListCustom(this SerializedProperty _listProperty, SerializedObject _sourceRef, System.Type _elementType, string _header = "", int _addFieldAmount = 0)
    {
        var list = new ReorderableList(_sourceRef, _listProperty, true, true, true, true);

        int fieldAmount = 1;

        list.drawHeaderCallback = (Rect position) =>
        {
            EditorGUI.LabelField(position, _header);
        };

        list.drawElementCallback = (Rect position, int index, bool isActive, bool isFocused) =>
        {
            var element = _listProperty.GetArrayElementAtIndex(index);
            var fieldInfos = _elementType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            fieldAmount = fieldInfos.Length + _addFieldAmount;
            EditorGUI.PropertyField(position, element, true);
            list.elementHeight = fieldAmount * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
        };

        return list;
    }

    public static void ArrayFieldCustom(this SerializedProperty _property, bool _showSize, bool _indent, string _prefixLabel = null)
    {
        _property.isExpanded = EditorGUILayout.Foldout(_property.isExpanded, _property.displayName);
        if (_property.isExpanded)
        {
            if (_indent)
                EditorGUI.indentLevel++;
            if (_showSize)
            {
                _property.arraySize = EditorGUILayout.DelayedIntField("Size", _property.arraySize);
            }

            for (int i = 0; i < _property.arraySize; i++)
            {
                var element = _property.GetArrayElementAtIndex(i);
                var label = new GUIContent
                {
                    text = element.displayName,
                };
                if (_prefixLabel != null)
                    label.text = _prefixLabel + " " + i + ":";
                EditorGUILayout.PropertyField(element, label);
            }
            if (_indent)
                EditorGUI.indentLevel--;
        }

    }

    public static void ArrayFieldCustom(this SerializedProperty _property, bool _showSize, bool _indent, System.Action<SerializedProperty> _propertyMethod)
    {
        _property.isExpanded = EditorGUILayout.Foldout(_property.isExpanded, _property.displayName);
        if (_property.isExpanded)
        {
            if (_indent)
                EditorGUI.indentLevel++;
            if (_showSize)
            {
                _property.arraySize = EditorGUILayout.DelayedIntField("Size", _property.arraySize);
            }

            for (int i = 0; i < _property.arraySize; i++)
            {
                var element = _property.GetArrayElementAtIndex(i);
                element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, element.displayName);
                if (element.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    _propertyMethod.Invoke(element);
                    EditorGUI.indentLevel--;
                }
            }
            if (_indent)
                EditorGUI.indentLevel--;
        }

    }

    public static void ArrayFieldButtons(this SerializedProperty _property, string _label = "Element", bool _indentProperty = true, bool _indentElement = true, 
        bool _foldoutProperty = true, bool _foldoutElements = true, System.Action<SerializedProperty, int> _propertyMethod = null, 
        bool _dockButtonsToRight = false)
    {
        

        EditorGUILayout.BeginHorizontal();
        if (_foldoutProperty)
            _property.isExpanded = EditorGUILayout.Foldout(_property.isExpanded, _property.displayName);
        else
            _property.isExpanded = true;
        if (_property.isExpanded)
        {
            if (!_foldoutProperty)
                GUILayout.FlexibleSpace();
            string label = "Add " + _label;
            var labelWidth = GUI.skin.label.CalcSize(new GUIContent(label)).x;
            float width = labelWidth + (EditorGUI.indentLevel * 15);
            EditorGUILayout.LabelField(label, GUILayout.Width(width));
            if (GUILayout.Button("+", GUILayout.Width(30)))
                _property.arraySize++;
        }
        EditorGUILayout.EndHorizontal();
        if (_property.isExpanded)
        {
            if (_indentProperty)
                EditorGUI.indentLevel++;
            for (int i = 0; i < _property.arraySize; i++)
            {
                if (_dockButtonsToRight)
                    EditorGUILayout.BeginHorizontal();
                var element = _property.GetArrayElementAtIndex(i);
                if (_foldoutElements)
                    element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, element.displayName);
                else
                    element.isExpanded = true;

                if (element.isExpanded)
                {
                    if (_indentElement)
                    EditorGUI.indentLevel++;
                    
                    if (_propertyMethod != null)
                        _propertyMethod.Invoke(element, i);
                    else
                        EditorGUILayout.PropertyField(element);
                    if (!_dockButtonsToRight)
                        EditorGUILayout.BeginHorizontal();

                    GUILayout.FlexibleSpace();
                    string label = "Remove " + _label;
                    var labelWidth = GUI.skin.label.CalcSize(new GUIContent(label)).x;
                    float width = labelWidth + 8;// + (EditorGUI.indentLevel * 15);
                    if (_dockButtonsToRight)
                    {
                        label = "x";
                        width = 30;
                    }
                        
                    if (GUILayout.Button(label, GUILayout.Width(width)))
                        _property.DeleteArrayElementAtIndex(i);
                    if (_property.arraySize > 1 && i > 0)
                    {
                        if (GUILayout.Button("↑", GUILayout.Width(30)))
                            _property.MoveArrayElement(i, i - 1);
                    }
                    if (i < _property.arraySize - 1)
                    {
                        if (GUILayout.Button("↓", GUILayout.Width(30)))
                            _property.MoveArrayElement(i, i + 1);
                    }

                    //if (!_dockButtonsToRight)
                        EditorGUILayout.EndHorizontal();
                    if (_indentElement)
                        EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.Space();
            if (_indentProperty)
                EditorGUI.indentLevel--;

        }

    }

    public static string[] GetDisplayNames(this SerializedProperty _arrayProperty)
    {
        var names = new string[_arrayProperty.arraySize];
        for (int i = 0; i < _arrayProperty.arraySize; i++)
        {
            names[i] = _arrayProperty.GetArrayElementAtIndex(i).displayName;
        }
        return names;
    }

    public static void LabelFieldCustom(string _label, FontStyle _fontStyle = FontStyle.Normal, Color _color = default, int _fontSize = 11, int _width = -1)
    {
        if (_color == default)
            _color = Color.black;
        GUIStyle style = new GUIStyle
        {
            normal = new GUIStyleState
            {
                textColor = _color,
            },
            fontStyle = _fontStyle,
            fontSize = _fontSize,

        };

        if (_width > 0)
        {
            float width = _width + (EditorGUI.indentLevel * 15);
            EditorGUILayout.LabelField(_label, style, GUILayout.Width(width));
        }
        else
            EditorGUILayout.LabelField(_label, style );

    }

    public static void TextFieldCustom(this SerializedProperty _stringProperty, string _defaultString)
    {
        if (_stringProperty.stringValue == "")
            _stringProperty.stringValue = _defaultString;
        EditorGUILayout.PropertyField(_stringProperty);
    }

    public static void IntFieldClamp(this SerializedProperty _intProperty, int _min, int _max)
    {
        _intProperty.intValue = Mathf.Clamp(_intProperty.intValue, _min, _max);
        EditorGUILayout.PropertyField(_intProperty);
    }

    public static void FloatFieldClamp(this SerializedProperty _floatProperty, float _min, float _max)
    {
        _floatProperty.floatValue = Mathf.Clamp(_floatProperty.floatValue, _min, _max);
        EditorGUILayout.PropertyField(_floatProperty);
    }

    public static void SetIndexStrings(IndexStringProperty _indexStringProperty, string[] _strings)
    {
        _indexStringProperty.stringValues = _strings;
    }

    public static void IndexStringPropertyField(this SerializedProperty _indexStringProperty, string[] _strings)
    {
        var prop = _indexStringProperty.GetRootValue<IndexStringProperty>();
        if (prop != null)
        {
            prop.stringValues = _strings;
            EditorGUILayout.PropertyField(_indexStringProperty);
        }
        else
        {
            Debug.Log(_indexStringProperty.displayName + " must be of type " + typeof(IndexStringProperty));
        }

    }

    public static void ChildNamePopUpParentOverride(this SerializedProperty _childNameProperty, SerializedObject _sourceRef, SerializedProperty _parentProperty)
    {
        var parent = _parentProperty.GetRootValue<GameObject>();
        ChildNamePopUpParentOverride(_childNameProperty, _sourceRef, parent, _parentProperty.displayName);

    }

    public static void ChildNamePopUpParentOverride(this SerializedProperty _childNameProperty, SerializedObject _sourceRef, GameObject _parent, string _parentFieldName)
    {
        var childName = _childNameProperty.GetRootValue<ChildName>();
        childName.overrideParent = true;
        if (childName.parent != _parent)
        {
            childName.overridePropertyName = _parentFieldName;
            childName.parent = _parent;
            _sourceRef.Update();
        }
        EditorGUILayout.PropertyField(_childNameProperty);

    }

    public static void DisplayAllChildrenPopup(string _fieldName, SerializedProperty _goProperty, SerializedProperty _indexProperty, SerializedProperty _stringProperty)
    {

        GameObject go = _goProperty.objectReferenceValue as GameObject;
        if (!go)
        {
            EditorGUILayout.LabelField(_fieldName, _goProperty.displayName + " is empty!");
            return;
        }
        else if (go.transform.childCount < 1)
        {
            EditorGUILayout.LabelField(_fieldName, _goProperty.displayName + " Must Have Children!");
            return;
        }

        //put all child names into array
        Transform[] childs = go.GetComponentsInChildren<Transform>();
        var childNames = new string[childs.Length];
        for (int i = 1; i < childs.Length; i++)
        {
            childNames[i] = childs[i].name;
        }

        //display popup
        _indexProperty.intValue = EditorGUILayout.Popup(_fieldName, _indexProperty.intValue, childNames);
        if (_indexProperty.intValue < childNames.Length)
            _stringProperty.stringValue = childNames[_indexProperty.intValue];
    }

    public static void DisplayAllInputAxisPopup(string _fieldName, SerializedProperty _indexProperty, SerializedProperty _stringProperty)
    {
        //put all input managers axis into an array
        var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
        var obj = new SerializedObject(inputManager);
        var axisArray = obj.FindProperty("m_Axes");
        var inputAxisNames = new string[axisArray.arraySize];
        for (int i = 0; i < inputAxisNames.Length; i++)
        {
            inputAxisNames[i] = axisArray.GetArrayElementAtIndex(i).FindPropertyRelative("m_Name").stringValue;
        }

        //display popup
        _indexProperty.intValue = EditorGUILayout.Popup(_fieldName, _indexProperty.intValue, inputAxisNames);
        SerializedProperty elementHor = axisArray.GetArrayElementAtIndex(_indexProperty.intValue);
        _stringProperty.stringValue = elementHor.FindPropertyRelative("m_Name").stringValue;
        obj.Dispose();
    }

    public static void SpritePreviewField(SerializedProperty _sprite, float _width, float _height, bool _stretchFit)
    {
        EditorGUILayout.BeginHorizontal();
        if (_sprite.objectReferenceValue != null)
        {
            var sprite = (Sprite)_sprite.objectReferenceValue;
            if (sprite)
            {
                GUIStyle style = new GUIStyle();
                style.fixedHeight = _height;
                style.fixedWidth = _width;
                style.alignment = TextAnchor.MiddleCenter;
                style.stretchHeight = _stretchFit;
                style.stretchWidth = _stretchFit;
                GUILayout.Box(sprite.texture, style);
            }

        }
        EditorGUILayout.PropertyField(_sprite);
        EditorGUILayout.EndHorizontal();
    }

    public static void DrawHandleTransformPoint(this SerializedProperty _localVector2Property, Object _source, Transform _sourceTrans, bool _lockYPosToSource = false, SerializedProperty _worldVector2Property = null)
    {
        EditorGUI.BeginChangeCheck();

        var localVector2 = _localVector2Property.GetRootValue<Vector2>();
        Vector2 handle = new Vector2();
        if (handle != (Vector2)_sourceTrans.transform.TransformPoint(localVector2))
            handle = Handles.PositionHandle((Vector2)_sourceTrans.transform.TransformPoint(localVector2), Quaternion.identity);
        else
            handle = Handles.PositionHandle(handle, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())//update script values after dragging
        {
            Undo.RecordObject(_source, "Modified " + _source + " properties.");
            var pos = handle;
            if (_lockYPosToSource)
                pos = new Vector2(handle.x, _sourceTrans.transform.position.y);
            _localVector2Property.SetValueOnRoot<Vector2>(_sourceTrans.transform.InverseTransformPoint(pos));
            if (_worldVector2Property != null)
            {
                localVector2 = _localVector2Property.GetRootValue<Vector2>();
                _worldVector2Property.SetValueOnRoot<Vector2>(_sourceTrans.transform.TransformPoint(localVector2));
            }


        }
    }

    public static void DrawArrowedLine(Vector3 _startPos, Vector3 _endPos, float _lineSpacing = 2, float _arrowSize = 0.1f, Color _lineColor = default, Color _arrowColor = default)
    {
        if (_arrowColor != default)
            Handles.color = _arrowColor;
        var dir = (_endPos - _startPos).normalized;
        var rot = Quaternion.FromToRotation(Vector3.up, dir);
        var distance = Vector3.Distance(_startPos, _endPos);
        var spacing = 0.5f;
        var amount = distance / spacing;
        var lastDist = 0f;
        for (int i = 0; i < amount; i++)
        {
            var perc = lastDist / distance;
            var posOnLine = Vector3.Lerp(_startPos, _endPos, perc);
            if (i != 0)
                DrawArrow(posOnLine, rot, _arrowSize);
            else
                DrawWireSphere(posOnLine, rot, _arrowSize, _arrowColor);
            lastDist += spacing;
        }
        if (_lineColor != default)
            Handles.color = _lineColor;
        Handles.DrawDottedLine(_startPos, _endPos, _lineSpacing);
    }

    public static void DrawArrow(Vector3 _pos, Quaternion _rot, float _radius, Color _color = default)
    {
        if (_color != default)
            Handles.color = _color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            //use this answer for a more scalable calculation in the future:
            //https://stackoverflow.com/questions/14096138/find-the-point-on-a-circle-with-given-center-point-radius-and-degree
            Handles.DrawWireDisc(Vector3.zero, Vector3.up, _radius);
            var left = Vector3.left * _radius;
            var right = Vector3.right * _radius;
            var forward = Vector3.forward * _radius;
            var back = Vector3.back * _radius;
            var top = Vector3.up * (_radius * 2);
            Handles.DrawLine(left, top);
            Handles.DrawLine(right, top);
            Handles.DrawLine(forward, top);
            Handles.DrawLine(back, top);
        }
    }

    public static void DrawHandleWorldPosition(this SerializedProperty _vector2Property, Object _source)
    {
        var vector2Source = _vector2Property.GetRootValue<Vector2>();
        Vector2 handle = new Vector2();
        if (handle != vector2Source)
            handle = Handles.PositionHandle(vector2Source, Quaternion.identity);
        else
            handle = Handles.PositionHandle(handle, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())//update script values after dragging
        {
            Undo.RecordObject(_source, "Modified " + _source + " properties.");

            _vector2Property.SetValueOnRoot<Vector2>(handle);
        }
    }

    public static void DrawWireSphere(Vector3 _pos, Quaternion _rot, float _radius, Color _color = default)
    {
        if (_color != default)
            Handles.color = _color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            Handles.DrawWireDisc(Vector3.zero, Vector3.up, _radius);
            Handles.DrawWireDisc(Vector3.zero, Vector3.left, _radius);
            Handles.DrawWireDisc(Vector3.zero, Vector3.back, _radius);
        }
    }

    public static void DrawWireCapsule(Vector3 _pos, Quaternion _rot, float _radius, float _height, Color _color = default)
    {
        if (_color != default)
            Handles.color = _color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            var pointOffset = (_height - (_radius * 2)) / 2;

            //draw sideways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, _radius);
            Handles.DrawLine(new Vector3(0, pointOffset, -_radius), new Vector3(0, -pointOffset, -_radius));
            Handles.DrawLine(new Vector3(0, pointOffset, _radius), new Vector3(0, -pointOffset, _radius));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, _radius);
            //draw frontways
            Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, _radius);
            Handles.DrawLine(new Vector3(-_radius, pointOffset, 0), new Vector3(-_radius, -pointOffset, 0));
            Handles.DrawLine(new Vector3(_radius, pointOffset, 0), new Vector3(_radius, -pointOffset, 0));
            Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, _radius);
            //draw center
            Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, _radius);
            Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, _radius);

        }
    }

    public static void DrawWireCube(Vector3 _pos, Quaternion _rot, Vector3 _size, Color _color = default)
    {
        if (_color != default)
            Handles.color = _color;
        Matrix4x4 angleMatrix = Matrix4x4.TRS(_pos, _rot, Handles.matrix.lossyScale);
        using (new Handles.DrawingScope(angleMatrix))
        {
            Handles.DrawWireCube(Vector3.zero, _size);
        }
    }

    public static void PropertyFieldCustom(SerializedProperty _property, string _label, bool _includeChildren = false, Texture _image = null, string _toolTip = null)
    {
        GUIContent content = new GUIContent()
        {
            text = _label,
            image = _image,
            tooltip = _toolTip
        };

        EditorGUILayout.PropertyField(_property, content, _includeChildren);
    }

    public static void PrefabFieldWithComponent(this SerializedProperty _gameobjectProperty, System.Type _componentType)
    {
        _gameobjectProperty.objectReferenceValue =
            EditorGUILayout.ObjectField(_gameobjectProperty.displayName, _gameobjectProperty.objectReferenceValue, typeof(GameObject), false);

        var prefab = _gameobjectProperty.objectReferenceValue;
        if (prefab)
        {
            var obj = prefab as GameObject;
            if (obj)
            {
                if (!obj.GetComponent(_componentType))
                {
                    Debug.Log(obj.name + " does not have component: " + _componentType.Name + ". " + _gameobjectProperty.displayName +
                        " field requires a prefab with a " + _componentType.Name + " component.");
                    _gameobjectProperty.objectReferenceValue = null;
                }
            }

        }
    }

    public static void ScriptableObjectFieldType(SerializedProperty _scriptableProperty, System.Type _type)
    {
        _scriptableProperty.objectReferenceValue =
            EditorGUILayout.ObjectField(_scriptableProperty.displayName, _scriptableProperty.objectReferenceValue, typeof(ScriptableObject), false);

        var obj = _scriptableProperty.objectReferenceValue;
        if (obj)
        {
            var type = obj.GetType();
            if (type != _type)
            {
                Debug.Log(obj.name + " is not of type: " + _type.Name + ". " + _scriptableProperty.displayName +
                    " field requires to be  " + _type.Name);
                _scriptableProperty.objectReferenceValue = null;
            }

        }
    }

    public static void AnimatorStateField(SerializedProperty _property, Animator _anim)
    {
        var states = new AnimatorState[_anim.runtimeAnimatorController.animationClips.Length];
        states = EditorExtensions.GetAnimatorStates(_anim);
        if (states.Length > 0)
        {
            var stateNames = new string[states.Length];
            for (int i = 0; i < states.Length; i++)
            {
                stateNames[i] = states[i].name;
            }
            var anim = _property.GetRootValue<AnimatorParamStateInfo>();
            if (anim != null)
            {
                anim.indexValue = EditorGUILayout.Popup(_property.displayName, anim.indexValue, stateNames);
                anim.stringValue = stateNames[anim.indexValue];
            }

        }

    }

    public static AnimatorState[] GetAnimatorStates(this Animator _animator)
    {
        AnimatorController controller = _animator ? _animator.runtimeAnimatorController as AnimatorController : null;
        return controller == null ? null : controller.layers.SelectMany(l => l.stateMachine.states).Select(s => s.state).ToArray();
    }

    public static AnimatorState[] GetAnimatorStates(this AnimatorController _animator)
    {
        AnimatorController controller = _animator ? _animator : null;
        return controller == null ? null : controller.layers.SelectMany(l => l.stateMachine.states).Select(s => s.state).ToArray();
    }

    public static string[] GetAnimatorStateNames(this Animator _animator)
    {
        var states = GetAnimatorStates(_animator);
        var names = new string[states.Length];
        for (int i = 0; i < states.Length; i++)
        {
            names[i] = states[i].name;
        }
        return names;
    }

    public static int FindStateLayer(AnimatorController _animCont, string _stateName)
    {
        for (int i = 0; i < _animCont.layers.Length; i++)
        {
            foreach (var child in _animCont.layers[i].stateMachine.states)
            {
                if (child.state.name == _stateName)
                    return i;
            }
        }
        Debug.LogError("could not find layer with state name: " + _stateName + " in: " + _animCont.name);
        return 0;
    }

    public static void IndexStringField(this SerializedProperty _indexStringProperty, string[] _stringArray, AnimatorController _animCont = null, string _label = null)
    {
        if (_stringArray.Length < 1)
            return;

        var indexValue = _indexStringProperty.FindPropertyRelative("indexValue");
        var stringValue = _indexStringProperty.FindPropertyRelative("stringValue");
        var layer = _indexStringProperty.FindPropertyRelative("layer");
        var label = _indexStringProperty.displayName;
        if (_label != null)
            label = _label;
        indexValue.intValue = EditorGUILayout.Popup(label, indexValue.intValue, _stringArray);
        indexValue.intValue = Mathf.Clamp(indexValue.intValue, 0, _stringArray.Length - 1);
        stringValue.stringValue = _stringArray[indexValue.intValue];
        if (_animCont)
        {
            int lay = FindStateLayer(_animCont, stringValue.stringValue);
            layer.intValue = lay;
        }
    }

    public static void ClampArraySize(this SerializedProperty _targetList, int _size)
    {
        if (_targetList.arraySize == _size)
            return;

        while (_targetList.arraySize < _size)
        {
            _targetList.InsertArrayElementAtIndex(_targetList.arraySize);
        }
        while (_targetList.arraySize > _size)
        {
            _targetList.DeleteArrayElementAtIndex(_targetList.arraySize - 1);
        }
    }

    public static string[] GetInputAxisNames()
    {
        //put all input managers axis into an array
        var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
        var obj = new SerializedObject(inputManager);
        var axisArray = obj.FindProperty("m_Axes");
        var inputAxisNames = new string[axisArray.arraySize];
        for (int i = 0; i < inputAxisNames.Length; i++)
        {
            inputAxisNames[i] = axisArray.GetArrayElementAtIndex(i).FindPropertyRelative("m_Name").stringValue;
        }
        obj.Dispose();
        return inputAxisNames;
    }

    public static void InputPropertyField(this SerializedProperty _inputProperty, string[] _stringArray)
    {
        if (_stringArray.Length < 1)
            return;

        var indexValue = _inputProperty.FindPropertyRelative("indexValue");
        var stringValue = _inputProperty.FindPropertyRelative("stringValue");

        indexValue.intValue = EditorGUILayout.Popup(_inputProperty.displayName, indexValue.intValue, _stringArray);
        indexValue.intValue = Mathf.Clamp(indexValue.intValue, 0, _stringArray.Length - 1);
        stringValue.stringValue = _stringArray[indexValue.intValue];
    }

    public static void EndHorizontal(this Rect _position)
    {
        _position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
}
