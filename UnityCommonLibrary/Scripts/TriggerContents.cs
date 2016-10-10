using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
	/// <summary>
	/// TODO: Optimize to use C# events
	/// </summary>
	[RequireComponent(typeof(Collider))]
	public class TriggerContents : MonoBehaviour
	{
		public delegate void OnContentsChanged(bool major);
		public event OnContentsChanged ContentsChanged;

		private HashSet<Collider> _contents = new HashSet<Collider>();

		public bool hasAny
		{
			get
			{
				return _contents.Count > 0;
			}
		}
		public new Collider collider { get; private set; }
		public HashSet<Collider> contents
		{
			get
			{
				// Remove all null entries
				_contents.RemoveWhere(c => !c);
				return _contents;
			}
		}

		private void Awake()
		{
			collider = GetComponent<Collider>();
			if(!collider.isTrigger)
			{
				Debug.LogError("COLLIDER MUST BE TRIGGER!");
			}
		}
		private void OnTriggerEnter(Collider c)
		{
			_contents.Add(c);
			if(ContentsChanged != null)
			{
				ContentsChanged(_contents.Count == 1);
			}
		}
		private void OnTriggerExit(Collider c)
		{
			_contents.Remove(c);
			if(ContentsChanged != null)
			{
				ContentsChanged(_contents.Count == 0);
			}
		}
	}
}
