using System;
using UnityCommonLibrary;
using UnityEditor;

public abstract class MultiTagApplicatorInspector<T> : Editor where T : struct, IFormattable, IConvertible, IComparable
{
	private MultiTagApplicator<T> obj;

	private void OnEnable()
	{
		obj = target as MultiTagApplicator<T>;
	}
	public override void OnInspectorGUI()
	{
		obj.applyRecursively = EditorGUILayout.Toggle("Apply Recursively", obj.applyRecursively);
		obj.tags = (T)Enum.ToObject(typeof(T), EditorGUILayout.EnumMaskField("Tags", (Enum)Enum.ToObject(typeof(T), obj.tags)));
	}
}