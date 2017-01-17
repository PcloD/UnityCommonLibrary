﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary.Utility
{
	public static class TransformUtility
	{
		private static Queue<Transform> bfsSearchQueue = new Queue<Transform>();

		public static void SetPosition(this Transform transform, Space space, float? x = null, float? y = null, float? z = null)
		{
			var newV3 = transform.position;
			newV3.SetXYZ(x, y, z);
			if (space == Space.World)
			{
				transform.position = newV3;
			}
			else
			{
				transform.localPosition = newV3;
			}
		}
		public static void SetLocalScale(this Transform transform, float? x = null, float? y = null, float? z = null)
		{
			var newV3 = transform.localScale;
			newV3.SetXYZ(x, y, z);
			transform.localScale = newV3;
		}
		public static void SetEulerAngles(this Transform transform, Space space, float? x = null, float? y = null, float? z = null)
		{
			var newV3 = transform.eulerAngles;
			newV3.SetXYZ(x, y, z);
			if (space == Space.World)
			{
				transform.rotation = Quaternion.Euler(newV3);
			}
			else
			{
				transform.localRotation = Quaternion.Euler(newV3);
			}
		}
		public static void Reset(this Transform t)
		{
			Reset(t, TransformElement.All, Space.World);
		}
		public static void Reset(this Transform t, TransformElement elements)
		{
			Reset(t, elements, Space.World);
		}
		public static void Reset(this Transform t, Space space)
		{
			Reset(t, TransformElement.All, space);
		}
		public static void Reset(this Transform t, TransformElement elements, Space space)
		{
			if ((elements & TransformElement.Position) != 0)
			{
				if (space == Space.World)
				{
					t.position = Vector3.zero;
				}
				else
				{
					t.localPosition = Vector3.zero;
				}
			}
			if ((elements & TransformElement.Rotation) != 0)
			{
				if (space == Space.World)
				{
					t.rotation = Quaternion.identity;
				}
				else
				{
					t.localRotation = Quaternion.identity;
				}
			}
			if ((elements & TransformElement.Scale) != 0)
			{
				t.localScale = Vector3.one;
			}
		}
		public static void Match(this Transform t, Transform other, TransformElement elements)
		{
			if ((elements & TransformElement.Position) != 0)
			{
				t.position = other.position;
			}
			if ((elements & TransformElement.Rotation) != 0)
			{
				t.rotation = other.rotation;
			}
			if ((elements & TransformElement.Scale) != 0)
			{
				t.localScale = other.localScale;
			}
		}
		public static Vector3 DirectionTo(this Transform t, Transform other)
		{
			return (other.position - t.position).normalized;
		}
		public static Vector3 DirectionTo(this Transform t, Vector3 other)
		{
			return (other - t.position).normalized;
		}
		public static Vector3 DirectionTo(this Vector3 v, Vector3 other)
		{
			return (other - v).normalized;
		}
		public static Vector3 DirectionTo(this Vector3 v, Transform other)
		{
			return (other.position - v).normalized;
		}

		public static Transform FindChildBFS(this Transform t, string search, StringComparison comparison = StringComparison.CurrentCulture, bool tag = false)
		{
			bfsSearchQueue.Clear();
			for (int i = 0; i < t.childCount; i++)
			{
				bfsSearchQueue.Enqueue(t.GetChild(i));
			}
			Transform found = null;
			while (bfsSearchQueue.Count > 0)
			{
				var child = bfsSearchQueue.Dequeue();
				if ((tag && child.tag.Equals(search, comparison)) || child.name.Equals(search, comparison))
				{
					found = child;
					break;
				}
				for (int i = 0; i < child.childCount; i++)
				{
					bfsSearchQueue.Enqueue(child.GetChild(i));
				}
			}
			bfsSearchQueue.Clear();
			return found;
		}
		public static Transform FindChildDFS(this Transform t, string search, StringComparison comparison = StringComparison.CurrentCulture, bool tag = false)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				var child = t.GetChild(i);
				if ((tag && child.tag.Equals(search, comparison)) || child.name.Equals(search, comparison))
				{
					return child;
				}
				child = FindChildDFS(t.GetChild(i), search);
				if (child)
				{
					return child;
				}
			}
			return null;
		}
		public static Transform FindParent(this Transform t, string search, StringComparison comparison = StringComparison.CurrentCulture, bool tag = false)
		{
			Transform transform = t;
			while (transform)
			{
				if ((tag && transform.tag.Equals(search, comparison)) || transform.name.Equals(search, comparison))
				{
					return transform;
				}
				transform = transform.parent;
			}
			return null;
		}
		public static Transform[] GetChildren(this Transform transform)
		{
			var array = new Transform[transform.childCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = transform.GetChild(i);
			}
			return array;
		}
	}
}