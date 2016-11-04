﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityCommonLibrary.Utility
{
	public static class GameObjectUtility
	{
		public static bool MoveToGround(this GameObject obj, LayerMask? mask = null)
		{
			Bounds? bounds = null;
			var colliders = obj.GetComponentsInChildren<Collider>(true);
			for(int i = 0; i < colliders.Length; i++)
			{
				if(colliders[i].isTrigger)
				{
					continue;
				}
				if(!bounds.HasValue)
				{
					bounds = colliders[i].bounds;
				}
				else
				{
					bounds.Value.Encapsulate(colliders[i].bounds);
				}
			}
			if(!bounds.HasValue)
			{
				var renderers = obj.GetComponentsInChildren<Renderer>(true);
				for(int i = 0; i < renderers.Length; i++)
				{
					if(!bounds.HasValue)
					{
						bounds = renderers[i].bounds;
					}
					else
					{
						bounds.Value.Encapsulate(renderers[i].bounds);
					}
				}
			}
			if(bounds.HasValue)
			{
				mask = mask.HasValue ? mask : Physics.DefaultRaycastLayers;
				var allHits = Physics.RaycastAll(obj.transform.position + Vector3.up * 0.1f, Vector3.down, 1000f, mask.Value);
				Array.Sort(allHits, (r1, r2) => r1.distance.CompareTo(r2.distance));
				for(int i = 0; i < allHits.Length; i++)
				{
					if(!allHits[i].transform.IsChildOf(obj.transform))
					{
						obj.transform.position = allHits[i].point + Vector3.up * (bounds.Value.extents.y - 0.1f);
						return true;
					}
				}
			}
			return false;
		}
		public static T AddOrGetComponent<T>(this GameObject obj) where T : Component
		{
			T component = obj.GetComponent<T>();
			if(component == null)
			{
				return obj.AddComponent<T>();
			}
			else
			{
				return component;
			}
		}
		public static T AssertComponent<T>(this GameObject obj) where T : Component
		{
			Assert.IsTrue(obj);
			Assert.IsNotNull(obj);
			var component = obj.GetComponent<T>();
			Assert.IsTrue(component);
			Assert.IsNotNull(component);
			return obj.AddComponent<T>();
		}
		public static void SetLayerRecursive(this GameObject obj, int layer)
		{
			obj.layer = layer;
			foreach(Transform child in obj.transform)
			{
				child.gameObject.SetLayerRecursive(layer);
			}
		}
		public static string GetPath(GameObject obj)
		{
			var sb = new System.Text.StringBuilder("/" + obj.name);
			while(obj.transform.parent != null)
			{
				obj = obj.transform.parent.gameObject;
				sb.Insert(0, "/" + obj.name);
			}
			return sb.ToString();
		}
		public static void Toggle(bool enabled, params GameObject[] gameObjects)
		{
			foreach(var go in gameObjects)
			{
				if(go != null)
				{
					go.SetActive(enabled);
				}
			}
		}
	}
}