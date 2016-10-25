using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary
{
	[DisallowMultipleComponent]
	public abstract class Actor : MonoBehaviour
	{
		protected virtual void OnDestroy()
		{
			ActorUtililty.RemoveActorFromCache(this);
		}
	}
}