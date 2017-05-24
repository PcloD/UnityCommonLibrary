using System;
using UnityCommonLibrary;
using UnityEditor;

public abstract class MultiTagApplicatorInspector<T> : Editor
    where T : struct, IFormattable, IConvertible, IComparable
{
    private MultiTagApplicator<T> _obj;

    public override void OnInspectorGUI()
    {
        _obj.ApplyRecursively =
            EditorGUILayout.Toggle("Apply Recursively", _obj.ApplyRecursively);
        _obj.Tags = (T) Enum.ToObject(typeof(T),
            EditorGUILayout.EnumMaskField("Tags",
                (Enum) Enum.ToObject(typeof(T), _obj.Tags)));
    }

    private void OnEnable()
    {
        _obj = target as MultiTagApplicator<T>;
    }
}